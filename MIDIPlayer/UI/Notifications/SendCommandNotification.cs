using GalaSoft.MvvmLight.Messaging;

namespace Hscm.UI.Notifications
{
    internal class SendGameCommandNotification : MessageBase
    {
        public SendGameCommandNotification()
        {
        }

        public string CommandText { get; set; }
    }
}