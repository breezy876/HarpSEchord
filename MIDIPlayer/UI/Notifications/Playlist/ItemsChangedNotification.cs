using GalaSoft.MvvmLight.Messaging;

namespace Hscm.UI
{
    public class ItemsChangedNotification : MessageBase
    {
        public bool HasItems { get; set; }
    }
}