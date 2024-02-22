
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI.Notifications.Tracks
{
    public class ShowInstrumentsPopupNotification : MessageBase
    {
        public int TrackIndex { get; set; }

        public object Control { get; set; }
    }
}
