using Common;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;

namespace Hscm.UI.Notifications.Window
{
    public class GameWindowFoundNotification : MessageBase
    {
        public GameWindowFoundNotification()
        {
        }

        public GameClientInfo Info { get; set; }

        public IEnumerable<string> EnsembleMembers { get; set; }
    }
}