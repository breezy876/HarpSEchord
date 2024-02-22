using Common.Music;
using GalaSoft.MvvmLight.Messaging;

namespace Hscm.UI
{

    public class UpdatePlaylistItemDurationNotification : MessageBase
    {
        public string Title { get; set; }

        public string Duration { get; set; }
    }
}