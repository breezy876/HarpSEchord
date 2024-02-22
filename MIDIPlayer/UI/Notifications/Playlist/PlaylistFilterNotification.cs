using Hscm.UI.Notifications;
using GalaSoft.MvvmLight.Messaging;

namespace Hscm.UI
{
    public class PlaylistFilterNotification : MessageBase
    {
        public string FilterText { get; set; }
    }
}