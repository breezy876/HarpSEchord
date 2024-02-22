
using Common.Music;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI.Notifications.Tracks
{
    public class KeyOffsetChangedNotification : MessageBase
    {

        public int Offset { get; set; }
    }
}
