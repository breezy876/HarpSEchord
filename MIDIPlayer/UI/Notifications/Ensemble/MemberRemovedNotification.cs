using GalaSoft.MvvmLight.Messaging;

namespace Hscm.UI.Notifications
{
    public class MemberRemovedNotification : MessageBase
    {
        public MemberRemovedNotification()
        {
        }
        public string CharacterName { get; set; }
    }
}