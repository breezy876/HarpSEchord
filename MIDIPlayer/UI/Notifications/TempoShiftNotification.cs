using Hscm.UI.Notifications;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI.Notifications
{
    public class TempoShiftNotification : MessageBase
    {
        public bool IsDown { get; set; }
    }
}
