using GalaSoft.MvvmLight.Messaging;

namespace Hscm.UI.Notifications.Playlist
{
    internal class ChangeSettingsFileNotification : MessageBase
    {
        public ChangeSettingsFileNotification()
        {
        }

        public bool SavePlaylist { get; set;  }

        public string FilePath { get; set; }
    }
}