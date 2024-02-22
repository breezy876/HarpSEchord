using Common;
using Common.Helpers;
using Common.Messaging;
using Common.Messaging.Settings;
using Common.Models.Settings;
using Common.Music;
using Hscm.UI.Notifications;
using Hscm.UI.Notifications.Playlist;
using Hscm.UI.Notifications.Settings;
using Hscm.UI.Notifications.Settings.Pipe;
using Hscm.UI.Notifications.Tracks;
using Hscm.UI.Notifications.Window;
using Hscm.UI.ViewModels.MainWindow;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Hscm.UI
{
    public partial class MainWindow 
    {

        #region settings

        async Task ApplySettingsChanges(bool saveAppSettings = false, bool saveSongSettings = false)
        {
           await SaveSettings(saveAppSettings, saveSongSettings);
        }

        private void ApplySettings()
        {
            this.viewModel.ShowLog = Common.Settings.AppSettings.ShowLog;

            this.viewModel.ShowTracks = Common.Settings.AppSettings.ShowTracks;
            this.viewModel.TracksVisible = Common.Settings.AppSettings.ShowTracks;

            this.viewModel.ShowInfo = Common.Settings.AppSettings.ShowInfo;

            this.viewModel.Playlist.SettingsVisible = Common.Settings.AppSettings.PlaylistSettings.SettingsVisible;

            this.viewModel.Playlist.RepeatMode = Common.Settings.AppSettings.PlaylistSettings.RepeatMode;

            this.viewModel.ShowSongSettings = false;

            this.tracksViewModel.ShowSettings = Common.Settings.AppSettings.TrackSettings.ShowSettings;

            this.viewModel.SettingsVisible = Common.Settings.AppSettings.SettingsVisible;

            this.viewModel.ShowFfxiv = Common.Settings.AppSettings.ShowFfxiv;

            this.viewModel.ShowAdvancedLayout = Common.Settings.AppSettings.ShowAdvancedLayout;

            viewModel.Initialize();
        }

        private async Task SaveSettings(bool saveAppSettings = false, bool saveSongSettings = true)
        {
            try
            {
                if (saveAppSettings)
                  await Common.Settings.SaveAppSettings();
            }
            catch (Exception ex)
            {
                AppendLog("", "Error: unable to save application settings.");
            }

            if (!saveSongSettings)
                return;

            //Common.Settings.SongSettings.Settings.Clear();

            await SavePlaylistAndSettings();
        }
        #endregion
    }
}
