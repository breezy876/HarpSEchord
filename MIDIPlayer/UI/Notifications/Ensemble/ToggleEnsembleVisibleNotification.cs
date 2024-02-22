using GalaSoft.MvvmLight.Messaging;

namespace Hscm.UI.Notifications
{
    public class ToggleEnsembleVisibleNotification : MessageBase
    {
        public ToggleEnsembleVisibleNotification()
        {
        }

        public bool IsVisible { get; set; }

    }
}