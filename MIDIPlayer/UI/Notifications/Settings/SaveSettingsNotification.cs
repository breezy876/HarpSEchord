
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI.Notifications
{
    public class SaveSettingsNotification : MessageBase
    {
        public bool SaveSongSettings { get; set; }
        public bool SaveAppSettings { get; set; }

        public bool NotifyPlayerService { get; set; }
    }
}
