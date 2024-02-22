using GalaSoft.MvvmLight.Messaging;

namespace Hscm.UI.Notifications
{
    public class FfxivReloadPlaylistNotification : MessageBase
    {
        public FfxivReloadPlaylistNotification()
        {
        }

        public bool ReloadSettings { get; set; }


    }
}