using Hscm.UI.Controls;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI.Notifications.Window
{
    public class ReportProgressNotification : MessageBase
    {
        public ProgressInfo ProgressInfo { get; set; }
    }
}
