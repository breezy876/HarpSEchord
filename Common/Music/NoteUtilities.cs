
using Common.Music;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.MusicTheory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.FFXIV.Enums;

namespace Common.Music
{
    public class NoteUtilities
    {

        const int NotesInOctave = 12;

        const int C6 = 84;
        const int C3 = 48;

        public static string GetNoteText(SevenBitNumber noteNum)
        {
            string noteName = Melanchall.DryWetMidi.MusicTheory.NoteUtilities.GetNoteName(noteNum).ToString().Replace("Sharp", "#");
            int octave = Melanchall.DryWetMidi.MusicTheory.NoteUtilities.GetNoteOctave(noteNum);

            return $"{noteName}{octave}";
        }

    }
}
