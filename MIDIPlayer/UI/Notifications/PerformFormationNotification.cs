using GalaSoft.MvvmLight.Messaging;

namespace Hscm.UI.Notifications
{
    public class PerformFormationNotification : MessageBase
    {
        public PerformFormationNotification()
        {
        }

        public string Formation { get; set; }

    }
}