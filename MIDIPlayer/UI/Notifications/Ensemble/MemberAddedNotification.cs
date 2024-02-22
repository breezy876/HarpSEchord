using GalaSoft.MvvmLight.Messaging;

namespace Hscm.UI.Notifications
{
    public class MemberAddedNotification : MessageBase
    {
        public MemberAddedNotification()
        {
        }

        public string CharacterName { get; set; }
    }
}