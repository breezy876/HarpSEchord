using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI
{
    public class PianoKeyEventArgs : EventArgs
    {
        private int noteID;

        public PianoKeyEventArgs(int noteID)
        {
            this.noteID = noteID;
        }

        public int NoteID
        {
            get
            {
                return noteID;
            }
        }
    }
}

