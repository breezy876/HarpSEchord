
using GalaSoft.MvvmLight.Messaging;
using Melanchall.DryWetMidi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI.Notifications
{
    public class SendPianoNotification : MessageBase
    {
        public NoteEvent Event { get; set; }
    }
}
