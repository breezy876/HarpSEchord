
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI.Notifications
{
    public class SequenceTempoChangedNotification : MessageBase
    {
        public double Tempo { get; set; }
    }
}
