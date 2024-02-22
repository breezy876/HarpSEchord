
using Common.Midi;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Common.FFXIV.Enums;

namespace Common.Music
{
    /// <summary>
    /// wrapper for a MIDI sequence
    /// </summary>

    [JsonObject(MemberSerialization.OptIn)]
    public class MidiSequence : IDisposable
    {
        private const int DefaultBpm = 120;

        public MidiSequence(MidiSequence midiSequence)
        {
            Info = midiSequence.Info;
            PlayAll = midiSequence.PlayAll;

            ReduceMaxNotes = midiSequence.ReduceMaxNotes;

            KeyOffset = midiSequence.KeyOffset;
            OctaveOffset = midiSequence.OctaveOffset;

            Tempo = midiSequence.Tempo;

            Division = midiSequence.Division;

            Duration = midiSequence.Duration;

             HighestOnly = midiSequence.HighestOnly;

            TempoMap = midiSequence.TempoMap;

            TotalNotes = midiSequence.TotalNotes;

            HighestNote = midiSequence.HighestNote;
            LowestNote = midiSequence.LowestNote;

            Processed = midiSequence.Processed;
        }

        public MidiSequence()
        {
            Info = new SequenceInfo();
            Tracks = new Dictionary<int, Music.Track>();

            PlayAll = true;

            ReduceMaxNotes = 2;
            ReduceType = 1;

            KeyOffset = 0;
            OctaveOffset = 0;

            Tempo = 100;
        }

        public MidiSequence(string filePath) : this()
        {
            Info = new SequenceInfo(filePath);
        }

        public MidiSequence(int division) : this()
        {
            Division = division;
        }

        ~MidiSequence()
        {
            Cleanup();
        }

        #region properties
        [JsonProperty]
        public double Tempo { get; set; }

        [JsonProperty]
        public bool HoldLongNotes { get; set; }

        [JsonProperty]
        public Dictionary<int, Music.Track> Tracks { get; set; }

        public TempoMap TempoMap { get; set; }

        [JsonProperty]
        public SequenceInfo Info { get; private set; }

        public int Division { get; set; }

        public int TempoBpm { get; set; }

        public int LengthMs => (int)this.Duration.Current.TotalMilliseconds;

        [JsonProperty]
        public bool Autofilled { get; set; }

        [JsonProperty]
        public SequenceTimeSpan Duration { get; set; }

        [JsonProperty]
        public int KeyOffset { get; set; }

        [JsonProperty]
        public int OctaveOffset { get; set; }

        public int HighestChordSize { get; set; }

        [JsonProperty]
        public int ReduceMaxNotes { get; set; }

        [JsonProperty]
        public int ReduceType { get; set; }

        [JsonProperty]
        public bool HighestOnly { get; set; }

        [JsonProperty]
        public bool PlayAll { get; set; }


        [JsonProperty]
        public string Instrument { get;  set; }

        [JsonProperty]
        public int HighestNote { get; set; }

        [JsonProperty]
        public int LowestNote { get; set; }

        public string Range =>  Common.Midi.Helpers.GetNoteRangeText(this);

        [JsonProperty]
        public int TotalNotes { get; set; }

        [JsonProperty]
        public bool Processed { get; set; }

        #endregion

        #region public methods


        public Track CloneTrack(int trackIndex)
        {
            if (!Tracks.ContainsKey(trackIndex))
                return null;

            var newTrack = Tracks[trackIndex].Clone();

            int index = Tracks.Keys.Max() + 1;
            newTrack.Index = index;

            newTrack.ParentIndex = trackIndex;
            Tracks.Add(index, newTrack);

            return newTrack;
        }

       public void RefreshTracks()
        {
            var tracks = this.Tracks.Values.Select((t, i) => new { Key = i, Value = t }).ToDictionary(t => t.Key, t => t.Value);
            foreach (var track in tracks)
                track.Value.Index = track.Key;
            Tracks = tracks;
        }

        public void Dispose()
        {
            Cleanup();
        }
        #endregion


        #region private methods


        private void Cleanup()
        {
            this.Tracks = null;
            this.Info = null;
        }

        public string InfoText()
        {

                var sb = new StringBuilder();

                sb.AppendLine(Range);
                sb.AppendLine($"Length: {Duration}");
                sb.AppendLine($"Notes: {TotalNotes}");
                sb.AppendLine($"Tracks: {Tracks.Count}");

                return sb.ToString();

        }
        #endregion

    }
}
