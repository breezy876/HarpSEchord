using Melanchall.DryWetMidi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Midi
{

    [Serializable]
    public class PlayerEventArgs
    {
        public PlayerEventArgs(PlayerInfo info, NoteEvent noteEv = null)
        {
            this.Info = info;
            this.NoteEvent = noteEv;
        }
        public PlayerInfo Info { get; private set; }
        public NoteEvent NoteEvent { get; private set; }
    }
}
