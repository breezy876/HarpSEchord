using Hscm.UI.Notifications;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI.Notifications
{
    public class UpdateInfoNotification : MessageBase
    {
        public string NoteRangeText { get; set; }

        public string BpmText { get; set; }

        public string DivisionText { get; set; }

        public string TrackCountText { get; set; }

        public string NoteCountText { get; set; }
        public string LengthText { get; internal set; }
    }
}
