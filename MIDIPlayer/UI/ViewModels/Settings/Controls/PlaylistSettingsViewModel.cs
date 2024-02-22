
using Common;
using Hscm.UI;
using Hscm.UI.Notifications;
using Hscm.UI.Notifications.Playlist;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Hscm.UI.ViewModels.Settings
{
    public class PlaylistSettingsViewModel : ObservableViewModel
    { 
        public PlaylistSettingsViewModel() : base()
        {
 
        }
        public bool SavePlaylistSettings
        {
            get { return Common.Settings.AppSettings.PlaylistSettings.SavePlaylistSettings; }
            set
            {
                Common.Settings.AppSettings.PlaylistSettings.SavePlaylistSettings = value;
                RaisePropertyChanged();
            }
        }

        public bool SavePlaylist
        {
            get { return Common.Settings.AppSettings.PlaylistSettings.SavePlaylist; }
            set
            {
                Common.Settings.AppSettings.PlaylistSettings.SavePlaylist = value;
                RaisePropertyChanged();
            }
        }


        public bool LoadPlaylistSettings
        {
            get { return Common.Settings.AppSettings.PlaylistSettings.LoadPlaylistSettings; }
            set
            {
                Common.Settings.AppSettings.PlaylistSettings.LoadPlaylistSettings = value;
                RaisePropertyChanged();
            }
        }

        public bool LoadPrevPlaylist
        {
            get { return Common.Settings.AppSettings.PlaylistSettings.LoadPrevPlaylist; }
            set
            {
                Common.Settings.AppSettings.PlaylistSettings.LoadPrevPlaylist = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand SaveSettingsCommand { get { return new RelayCommand(ExecuteSaveSettingsCommand); } }

 
        private void ExecuteSaveSettingsCommand()
        {
            var notification = new SaveSettingsNotification() { SaveAppSettings = true, SaveSongSettings = true, NotifyPlayerService = true };
            Messenger.Default.Send(notification);
        }
    }
}
