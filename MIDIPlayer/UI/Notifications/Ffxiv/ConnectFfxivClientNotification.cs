using GalaSoft.MvvmLight.Messaging;

namespace Hscm.UI.Notifications
{
    public class ConnectFfxivClientNotification : MessageBase
    {
        public ConnectFfxivClientNotification()
        {
        }

        public string CharacterName { get; set; }

        public bool Connect { get; set; }


    }
}