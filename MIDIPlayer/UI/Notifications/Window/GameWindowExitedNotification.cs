using Common;
using GalaSoft.MvvmLight.Messaging;
using System.Collections;
using System.Collections.Generic;

namespace Hscm.UI.Notifications.Window
{
    public class GameWindowLoggedOutOrExitedNotification : MessageBase
    {
        public GameWindowLoggedOutOrExitedNotification()
        {
        }

        public IEnumerable<string> EnsembleMembers { get; set; }

        public GameClientInfo Info { get; set; }
    }
}