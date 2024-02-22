//using Common.Music;
//using Melanchall.DryWetMidi.Core;
//using Melanchall.DryWetMidi.Interaction;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Common.Midi
//{
//    public class MidiIO
//    {
//        public static void Save(MidiSequence sequence)
//        {
//            // Fill a midi file with the new track chunks
//            var newMidiFile = new MidiFile();
//            newMidiFile.TimeDivision = new TicksPerQuarterNoteTimeDivision((short)sequence.Division);

//            var chunks = GetTrackChunks(sequence);

//            var tempoEv = sequence.Events.Select(ev=>ev.Event).OfType<SetTempoEvent>().OrderBy(ev => ev.Time).FirstOrDefault();

//            newMidiFile.Chunks.AddRange(chunks);

//            using (TempoMapManager tempoManager = newMidiFile.ManageTempoMap())
//                tempoManager.SetTempo(0, new Tempo(tempoEv != null ? tempoEv.MicrosecondsPerQuarterNote : 500000));

//            newMidiFile.Write(sequence.Info.FilePath, true, MidiFileFormat.MultiTrack, new WritingSettings { CompressionPolicy = CompressionPolicy.NoCompression  });

//            // Write the midi file out into a memory stream and pass that to sanford to create a sanford sequence object
//            using (var stream = new MemoryStream())
//            {
//                newMidiFile.Write(stream, MidiFileFormat.MultiTrack, new WritingSettings { CompressionPolicy = CompressionPolicy.NoCompression });
//            }
//        }

//        private static IEnumerable<TrackChunk> GetTrackChunks(MidiSequence sequence)
//        {
//            var chunks = new List<TrackChunk>();

//            var groups = sequence.Events.DictionaryGroupBy(ev => ev.TrackIndex);

//            foreach (var group in groups)
//            {
//                var chunk = TimedEventsManagingUtilities.ToTrackChunk(group.Value);
//                chunks.Add(chunk);
//            }

//            return chunks;
//        }
//    }
//}
