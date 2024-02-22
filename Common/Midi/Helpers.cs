using Common.Music;
using Melanchall.DryWetMidi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Midi
{
    public class Helpers
    {
        private static string GetNoteText(int note)
        {
            string regularText = NoteUtilities.GetNoteText(new SevenBitNumber((byte)(note)));

            string text = $"{regularText}";

            return text;
        }

        public static string GetNoteRangeText(MidiSequence sequence)
        {

            string lowestText = GetNoteText(sequence.LowestNote);
            string highestText = GetNoteText(sequence.HighestNote);

            string text = $"Range: {lowestText} / {highestText}";

            return text;
        }

        public static string GetNoteRangeText(Track track)
        {

            string lowestText = GetNoteText(track.LowestNote);

            string highestText = GetNoteText(track.HighestNote);

            string text = $"Range: {lowestText} / {highestText}";

            return text;
        }
    }
}
