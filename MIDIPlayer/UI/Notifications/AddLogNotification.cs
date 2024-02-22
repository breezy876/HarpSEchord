using GalaSoft.MvvmLight.Messaging;

namespace Hscm.UI.Notifications
{
    public class AddLogNotification : MessageBase
    {
        public AddLogNotification()
        {
        }

        public string Text { get; set; }

        public string ServiceName { get; set; }
    }
}