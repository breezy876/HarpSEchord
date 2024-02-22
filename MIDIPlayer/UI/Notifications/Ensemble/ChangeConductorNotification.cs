using GalaSoft.MvvmLight.Messaging;

namespace Hscm.UI.Notifications
{
    public class ChangeConductorNotification : MessageBase
    {
        public ChangeConductorNotification()
        {
        }

        public string ConductorName { get; set; }
    }
}