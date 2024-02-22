using GalaSoft.MvvmLight.Messaging;

namespace Hscm.UI.Notifications
{
    public class ConductEnsembleNotification : MessageBase
    {
        public ConductEnsembleNotification()
        {
        }

        public bool IsLocal { get; set; }

        public string ConductorName { get; set; }
    }
}