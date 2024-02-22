using Common;
using GalaSoft.MvvmLight.Messaging;
using System.Collections;
using System.Collections.Generic;
using static Hscm.UI.MainWindow;

namespace Hscm.UI.Notifications.Window
{
    public class LayoutChangedNotification : MessageBase
    {
        public LayoutChangedNotification()
        {
        }

        public LayoutType Type { get; set; }
    }
}