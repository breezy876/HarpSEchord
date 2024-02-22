using Common.Midi.MidiBard.HSC;
using Common.Music;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Standards;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Midi
{
    public class MidiProcessor
    {
        private const int MicrosecondsPerMinute = 60000000;
        private const int DefaultTempo = 500000;

        private int bpm;

        public MidiProcessor()
        {
            bpm = 80;
        }

        public event EventHandler<MidiSequence> ProcessStarted;

        public event EventHandler<MidiSequence> ProcessFinished;

        public event EventHandler<MidiSequence> LoadStarted;

        public event EventHandler<MidiSequence> SequenceLoaded;

        /// <summary>
        /// loads midi file info for playlist only
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>


        #region private methods


        private static string GetPercussionName(int program)
        {
            var programText = ((GeneralMidiPercussion)program).ToString();
            return string.Concat(programText.Select(x => Char.IsUpper(x) || Char.IsDigit(x) ? " " + x : x.ToString())).TrimStart(' ');
        }
        private static string GetProgramName(int program)
        {
            var programText = ((GeneralMidiProgram)program).ToString();
            return string.Concat(programText.Select(x => Char.IsUpper(x) || Char.IsDigit(x) ? " " + x : x.ToString())).TrimStart(' ');
        }

        #region workers
        private MidiFile LoadMidi(string filePath)
        {
            using (var f = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var midiData = MidiFile.Read(f, new ReadingSettings
                {
                    NoHeaderChunkPolicy = NoHeaderChunkPolicy.Ignore,
                    NotEnoughBytesPolicy = NotEnoughBytesPolicy.Ignore,
                    InvalidChannelEventParameterValuePolicy = InvalidChannelEventParameterValuePolicy.ReadValid,
                    InvalidChunkSizePolicy = InvalidChunkSizePolicy.Ignore,
                    InvalidMetaEventParameterValuePolicy = InvalidMetaEventParameterValuePolicy.SnapToLimits,
                    MissedEndOfTrackPolicy = MissedEndOfTrackPolicy.Ignore,
                    UnexpectedTrackChunksCountPolicy = UnexpectedTrackChunksCountPolicy.Ignore,
                    ExtraTrackChunkPolicy = ExtraTrackChunkPolicy.Read,
                    UnknownChunkIdPolicy = UnknownChunkIdPolicy.ReadAsUnknownChunk,
                    SilentNoteOnPolicy = SilentNoteOnPolicy.NoteOff,
                    TextEncoding = Encoding.Default
                });

                return midiData;
            }
        }

        private async Task<NoteEventWithTrackIndex[]> GetSequenceEvents(string filePath)
        {
            return await Task.Run(() =>
            {
                var midiData = LoadMidi(filePath);

                var noteEvs = midiData.GetTrackChunks()
                .SelectMany((c, i) => c.Events.OfType<NoteOnEvent>().Select(ev => new NoteEventWithTrackIndex(ev.EventType, ev.NoteNumber) { TrackIndex = i, Channel = ev .Channel })).ToArray();
                return noteEvs;
            });
        }


        private Note[] UpdateNoteInfo(MidiSequence midiSequence, IEnumerable<TrackChunk> trackChunks)
        {

            var notes = trackChunks.GetNotes().ToArray();
            var noteNos = notes.Select(n => n.NoteNumber);

            midiSequence.TotalNotes = notes.ToArray().Length;

            midiSequence.HighestNote = noteNos.Max();
            midiSequence.LowestNote = noteNos.Min();

            return notes;
        }

        private void UpdateTimeInfo(MidiSequence midiSequence, MidiFile midiData, IEnumerable<Note> notes)
        {
            var tempoMap = midiData.GetTempoMap();

            var timedNoteOffEvent = notes.LastOrDefault()?.GetTimedNoteOffEvent();
            double midiFileDuration = (timedNoteOffEvent?.TimeAs<MetricTimeSpan>(tempoMap) ?? new MetricTimeSpan()).TotalMilliseconds;

            midiSequence.TempoMap = tempoMap;
            midiSequence.Duration = new SequenceTimeSpan((int)Math.Round(midiFileDuration));
        }

        public async Task<(MidiSequence sequence, TimedEvent[] timedEvents, Note[] notes, bool save)> LoadMidiFile(
            string filePath,
            bool loadMidi = false,
            bool loadSettings = false)
        {
            return await Task.Run(() =>
            {
                bool save = false;

                MidiSequence midiSequence = new MidiSequence();

                this.LoadStarted?.Invoke(this, midiSequence);

                MidiFile midiData = null;

                Note[] notes = null;
                TrackChunk[] trackChunks = null;
                TimedEvent[] timedEvents = null;

                midiSequence.Info.Title = Path.GetFileNameWithoutExtension(filePath);
                midiSequence.Info.FileSize = new System.IO.FileInfo(filePath).Length;
                midiSequence.Info.FilePath = filePath;

                if (loadSettings)
                    LoadSequenceSettings(midiSequence, false);

                this.SequenceLoaded?.Invoke(this, midiSequence);


                if (loadMidi)
                {
                    this.ProcessStarted?.Invoke(this, midiSequence);

                    midiData = LoadMidi(filePath);

                    trackChunks = midiData.GetTrackChunks().Where(c => c.Events.Any(e => e is NoteOnEvent)).ToArray();


                    //timedEvents = trackChunks.GetTimedEvents().Where(ev => ev.Event is NoteEvent || ev.Event is TextEvent).ToArray();


                    notes = UpdateNoteInfo(midiSequence, trackChunks);
                    UpdateTimeInfo(midiSequence, midiData, notes);


                    midiSequence.Tracks = GetSequenceTracks(midiSequence, trackChunks);

                    midiSequence.Processed = true;
                    save = true;

                    this.ProcessFinished?.Invoke(this, midiSequence);
                }

                if (loadSettings)
                    LoadSequenceSettings(midiSequence, true);

                //ChordTrimmer.Trim(midiData, midiSequence, 2, false, true);

                return (midiSequence, timedEvents, notes, save);
            });
        }


        private Func<NoteEvent, bool> isPercussionNote = (noteEv) => noteEv.Channel == 9 && (int)noteEv.NoteNumber >= (int) GeneralMidiPercussion.AcousticBassDrum &&
                            (int)noteEv.NoteNumber <= (int) GeneralMidiPercussion.OpenTriangle;


        public async Task<Track[]> SplitPercussion(MidiSequence seq)
        {
            var timedEvs = await GetSequenceEvents(seq.Info.FilePath);

            return SplitPercussion(seq, timedEvs).ToArray();
        }


        public void UnsplitPercussion(MidiSequence seq)
        {

            var drumTracks = seq.Tracks.Where(t => t.Value.PercussionNote.HasValue && t.Value.ParentIndex.HasValue).ToArray();
            var tracksToEnable = drumTracks.Select(t => t.Value.ParentIndex.Value).Distinct();

            foreach (var track in drumTracks)
            {
                seq.Tracks.Remove(track.Key);
            }

            foreach (var track in seq.Tracks.Values.Where(t => t.IsPercussion))
            {
                track.Enabled = true;
            }
        }


        private Track[] SplitPercussion(MidiSequence seq, IEnumerable<NoteEventWithTrackIndex> events)
        {

            var noteTracks = events
                .GroupBy(t => t.TrackIndex)
                .Select((g, i) => new {
                    TrackIndex = i,
                    Events = g
                });

            var tracks = noteTracks.Where(
                t =>
                    t.Events.Any(ev => isPercussionNote(ev))
                ).Select(
                g => new { 
                    TrackIndex = g.TrackIndex, 
                    Notes = g.Events.Where(ev => isPercussionNote(ev))
                    .Select(ev => ev.NoteNumber) })
                .ToDictionary(g => g.TrackIndex, g => g.Notes);

            int index = seq.Tracks.Keys.Max() + 1;
;

            var drumtracks = new List<Track>();

            foreach (var track in tracks)
            {
                foreach (var percNote in track.Value.Distinct())
                {
                    var percTrack = new Track();
                    var percName = MidiProcessor.GetPercussionName(percNote);

                    percTrack.Title = $"Percussion - {percName}";
                    percTrack.Index = index;
                    percTrack.PercussionNote = (int)percNote;
                    percTrack.ParentIndex = track.Key;
                    percTrack.IsPercussion = true;
                    percTrack.IsSplit = true;
                    percTrack.HighestNote = percNote;
                    percTrack.LowestNote = percNote;
                    percTrack.TotalNotes = track.Value.Count();

                    seq.Tracks[track.Key].Enabled = false;

                    seq.Tracks.Add(index, percTrack);

                    drumtracks.Add(percTrack);

                    index++;
                }
            }

            return drumtracks.ToArray();

        }

        private Dictionary<int, Track> GetSequenceTracks(MidiSequence midiSequence, IEnumerable<TrackChunk> chunks)
        {

            return GetTracks(chunks, midiSequence.TempoMap);

        }


        #endregion

        #region settings
        public MidiSequence GetSequenceSettings(MidiSequence sequence)
        {
            var sequenceSettings = Common.Settings.PlaylistSettings;

            if (sequenceSettings.Settings.IsNullOrEmpty())
                return null;

            if (!sequenceSettings.Settings.ContainsKey(sequence.Info.Title))
                return null;

            var settings = sequenceSettings.Settings[sequence.Info.Title];

            return settings;

        }

        private void LoadSequenceSettings(MidiSequence sequence, bool loadTracks = false)
        {
            var settings = GetSequenceSettings(sequence);

            if (settings == null)
                return;

            if (!loadTracks)
            UpdateSequenceFromSettings(sequence, settings, false);

            if (loadTracks)
                LoadSequenceTracksFromSettings(sequence, settings);
        }

        public void LoadSequenceTracksFromSettings(MidiSequence sequence, MidiSequence settings)
        {
            if (settings == null || settings.Tracks.IsNullOrEmpty())
                return;

            var trackIndexes = sequence.Tracks.Keys;

            var settingTracks = settings.Tracks.Values.Select((t, i) => new { t, i }).ToDictionary(t => t.i, t=> t.t);

            foreach (var trackSettings in settingTracks)
            {
                int index = trackSettings.Key;

                Track track = new Track();
                track.Index = index;

                if (sequence.Tracks.ContainsKey(index))
                {
                    track = sequence.Tracks[index];

                    UpdateSequenceTrackFromSettings(track, trackSettings.Value);
                }
                else
                {
                        UpdateSequenceTrackFromSettings(track, trackSettings.Value);
                    sequence.Tracks.AddOrUpdate(index, track);

                }
                
            }
        }

        public void UpdateSequenceAndTrackSettings(MidiSequence sequence, MidiSequence settings, bool ignoreIndex = false)
        {
            UpdateSequenceFromSettings(sequence, settings, ignoreIndex);
            LoadSequenceTracksFromSettings(sequence, settings);
        }


        private void UpdateSequenceFromSettings(MidiSequence sequence, MidiSequence sequenceSettings, bool ignoreIndex = false)
        {
            if (sequenceSettings == null)
                return;

            sequence.Tempo = sequenceSettings.Tempo;
            sequence.HoldLongNotes = sequenceSettings.HoldLongNotes;

            sequence.KeyOffset = sequenceSettings.KeyOffset;
            sequence.OctaveOffset = sequenceSettings.OctaveOffset;

            sequence.HighestOnly = sequenceSettings.HighestOnly;
            sequence.PlayAll = sequenceSettings.PlayAll;

            sequence.ReduceMaxNotes = sequenceSettings.ReduceMaxNotes;

            sequence.Instrument = sequenceSettings.Instrument;

                sequence.TotalNotes = sequenceSettings.TotalNotes;
                sequence.Duration = sequenceSettings.Duration;
                sequence.HighestNote = sequenceSettings.HighestNote;
                sequence.LowestNote = sequenceSettings.LowestNote;
           

            sequence.Processed = sequenceSettings.Processed;
        }

        private void UpdateSequenceTrackFromSettings(Track track, Track trackSettings)
        {
            if (trackSettings == null)
                return;

            if (trackSettings.ParentIndex.HasValue)
            {
                track.Cloned = trackSettings.Cloned;
                track.IsSplit = trackSettings.IsSplit;

            }

            track.Title = string.IsNullOrEmpty(track.Title) ? trackSettings.Title : track.Title;

            //track.Index = trackSettings.Index;
            track.OctaveOffset = trackSettings.OctaveOffset;
            track.KeyOffset = trackSettings.KeyOffset;
            track.Muted = trackSettings.Muted;
            track.AutofilledMember = trackSettings.AutofilledMember;
            track.AutofilledInstrument = trackSettings.AutofilledInstrument;
            track.EnsembleMember = trackSettings.EnsembleMember;
            track.EnsembleInstrument = trackSettings.EnsembleInstrument;
            track.HoldLongNotes = trackSettings.HoldLongNotes;
            track.HighestOnly = trackSettings.HighestOnly;
            track.PlayAll = trackSettings.PlayAll;
            track.ReduceMaxNotes = trackSettings.ReduceMaxNotes;
            track.TimeOffset = trackSettings.TimeOffset;
            track.ParentIndex = trackSettings.ParentIndex;
            track.PercussionNote = trackSettings.PercussionNote;
            track.IsPercussion = trackSettings.IsPercussion;
            track.TimeOffset = trackSettings.TimeOffset;

            track.TotalNotes = trackSettings.TotalNotes;
            track.HighestNote = trackSettings.HighestNote;
            track.LowestNote = trackSettings.LowestNote;

            track.Instrument = trackSettings.Instrument;

            track.Enabled = trackSettings.Enabled;

        }
        #endregion

        #region tracks

        private Track GetTrack(TrackChunk trackChunk, int index)
        {
            var notes = trackChunk.GetNotes().ToArray();
            var noteNos = notes.Select(n => n.NoteNumber);
            var highestNote = notes.FirstOrDefault(n => n.NoteNumber == noteNos.Max());
            var lowestNote = notes.FirstOrDefault(n => n.NoteNumber == noteNos.Min());

            var evs = trackChunk.Events;

            var progChangeEv = evs.OfType<ProgramChangeEvent>().FirstOrDefault();

            var trackNameEv = evs.OfType<SequenceTrackNameEvent>().FirstOrDefault();
            var trackName = trackNameEv != null && !string.IsNullOrEmpty(trackNameEv.Text) ? trackNameEv.Text : "Untitled";

            var track = new Music.Track(trackName, index, highestNote.NoteNumber, lowestNote.NoteNumber, notes.Length);

            track.Instrument = progChangeEv != null ? GetProgramName(progChangeEv.ProgramNumber) : "Untitled Instrument";

            if (string.IsNullOrEmpty(track.Instrument))
                track.Instrument = "Untitled Instrument";

            track.IsPercussion = trackChunk.Events.OfType<NoteEvent>().Any(ev => isPercussionNote(ev));

            return track;

        }
        private Dictionary<int, Music.Track> GetTracks(IEnumerable<TrackChunk> trackChunks, TempoMap tempoMap)
        {
            var tracks = new ConcurrentDictionary<int, Music.Track>();
            var trackDic = trackChunks.Select((t, i) => new { track = t, index = i });

            Parallel.ForEach(trackDic, t => {
                var track = GetTrack(t.track, t.index);
                tracks.AddOrUpdate(t.index, track, (i, tu) => tu);
            });

            return tracks.ToDictionary(t => t.Key, t => t.Value);
        }

        #endregion






        #endregion

    }
}
