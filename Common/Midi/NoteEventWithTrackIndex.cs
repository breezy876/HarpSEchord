using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Midi
{
    public class NoteEventWithTrackIndex : Melanchall.DryWetMidi.Core.NoteEvent
    {
        public int TrackIndex { get; set; }

        public NoteEventWithTrackIndex(MidiEventType type, SevenBitNumber note) : base(type, note, (SevenBitNumber)0)
        {

        }

        protected override MidiEvent CloneEvent()
        {
            throw new NotImplementedException();
        }
    }
}
