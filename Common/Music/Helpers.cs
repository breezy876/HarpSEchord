

using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Music
{
    public class Helpers
    {

        public static NoteEvent Clone(NoteEvent noteEv)
        {
            NoteEvent clone = null;

            if (noteEv.EventType == MidiEventType.NoteOn)
                clone = new NoteOnEvent(new SevenBitNumber(noteEv.NoteNumber), new SevenBitNumber(noteEv.Velocity));

            else if (noteEv.EventType == MidiEventType.NoteOff)
                clone = new NoteOffEvent(new SevenBitNumber(noteEv.NoteNumber), new SevenBitNumber(noteEv.Velocity));

            return clone;
        }

        public static IEnumerable<NoteEvent> GetNoteEvents(IEnumerable<TimedEvent> midiEvs)
        {
            var noteEvents = midiEvs
                .Where(ev => ev.Event.EventType == MidiEventType.NoteOn || ev.Event.EventType == MidiEventType.NoteOff)
                .Select(ev => ev.Event)
                .OfType<NoteEvent>();

            return noteEvents;
        }


    }
}
