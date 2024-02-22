using GalaSoft.MvvmLight.Messaging;

namespace Hscm.UI.Notifications
{
    public class SelectGameMenuNotification : MessageBase
    {
        public SelectGameMenuNotification()
        {
        }

        public string Option { get; set; }
    }
}