using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hscm.UI.Notifications;
using Hscm.UI.Notifications.Window;
using Hscm.UI.Notifications.Tracks;
using Hscm.UI.Notifications.Settings;
using Hscm.UI.Notifications.Playlist;
using System.Windows.Controls;
using Common;
using Hscm.UI.Notifications.Settings.Pipe;
using Hscm.UI.ViewModels.MainWindow;
using Common.Models.Ensemble;
using Common.Helpers;
using System.Windows.Media;
using System.Windows.Threading;
using Common.Messaging.Player;
using System.Windows;
using Common.Messaging;
using Common.Messaging.Settings;
using GalaSoft.MvvmLight.Messaging;
using System.Diagnostics;
using System.Windows.Input;
using Common.Music;
using Common.Interop;
using static Common.Interop.WindowHelpers;
using System.Runtime.InteropServices;
using Common.Midi;
using System.Threading;

namespace Hscm.UI
{
    public partial class MainWindow
    {
        private async void AutofillOptionChangedNotificationReceived(AutofillOptionChangedNotification obj)
        {
            tracksControl.UpdateTracks(viewModel.SelectedSequence);

            await SaveSettings(true, false);
            await SavePlaylistSettings();
        }

        private void ItemsChangedReceived(ItemsChangedNotification obj)
        {
            HandleItemsChanged();
        }


        private async void TrimTracksNotificationReceived(TrimTracksNotification obj)
        {
            await ExecuteRemoveAllTracks();
            await ExecuteSplitPercussion(false);
        }

        private async void RemoveAllTracksNotificationReceived(RemoveAllTracksNotification obj)
        {
            await ExecuteRemoveAllTracks();

        }

        private async void RemoveTracksNotificationReceived(RemoveTracksNotification obj)
        {
            foreach (int index in obj.TrackIndexes)
                if (viewModel.SelectedSequence.Tracks[index].Cloned)
                    viewModel.SelectedSequence.Tracks.Remove(index);

            viewModel.SelectedSequence.RefreshTracks();

            this.tracksControl.UpdateTracks(viewModel.SelectedSequence);

            await SavePlaylistSettings();
        }

        private async void SplitPercussionNotificationReceived(SplitPercussionNotification obj)
        {
            await ExecuteSplitPercussion(obj.Split);
        }


        private async void DuplicateTrackNotificationReceived(DuplicateTracksNotification obj)
        {
            foreach(var trackIndex in obj.TrackIndexes)
            {
                var track = viewModel.SelectedSequence.CloneTrack(trackIndex);

                viewModel.SelectedSequence.RefreshTracks();

                if (track == null)
                    return;

            }

            this.tracksControl.UpdateTracks(viewModel.SelectedSequence);


            await ApplySettingsChanges(false, true);

        }

        private  void UpdateSelectedSequenceNotificationReceived(UpdateSelectedSequenceNotification obj)
        {
            UpdateSelectedSequence();
        }

        private void LayoutChangedNotificationReceived(LayoutChangedNotification obj)
        {
            //layoutChanged = true;
            //UpdateLayout(obj.Type);
        }

  
        private async void LoadCurrentSongSettingsNotificationReceived(LoadCurrentSongSettingsNotification obj)
        {
            await LoadSelectedSongSettings();
        }

        private async void LoadSongSettingsNotificationReceived(LoadSongSettingsNotification obj)
        {
            await ShowLoadSongSettingsDialog();
        }

        private async void ShowSaveSongSettingsDialogNotificationReceived(ShowSaveSongSettingsDialogNotification obj)
        {
             await ShowSaveSongSettingsDialog();

        }

        private async void SaveSongSettingsNotificationReceived(SaveSongSettingsNotification obj)
        {
            await SavePlaylistSettings();

        }


        private void ShowProgressNotificationReceived(ShowProgressNotification obj)
        {

        }

        private void HideProgressNotificationReceived(HideProgressNotification obj)
        {

        }


        private void ShowMessageNotificationReceived(ShowMessageNotification msg)
        {
            ShowPopupMessage(msg.Text);
        }


        private async void PlaylistSequenceSelectedNotificationReceived(PlaylistSequenceSelectedNotification msg)
        {
            await  HandleSelectedSequence(msg.Sequence);
        }

        private  async void SaveSettingsNotificationReceived(SaveSettingsNotification msg)
        {
             await ApplySettingsChanges( msg.SaveAppSettings, msg.SaveSongSettings);
        }

        private async void SaveAppSettingsNotificationReceived(SaveAppSettingsNotification msg)
        {
            await SaveSettings(true, false);
        }


        private async void SequenceChordSettingsChangedNotificationReceived(SequenceChordSettingsChangedNotification msg)
        {
                await ApplySettingsChanges(false, true);
        }

        private void PlaylistEmptyNotificationReceived(PlaylistEmptyNotification msg)
        {

            //this.viewModel.TracksVisible = false;
        }



        private async void ReloadSequenceNotificationReceived(ReloadSequenceNotification msg)
        {
            await this.ReloadSequence();
        }

        private void ResetTrackNotificationReceived(TrackHasResetNotification msg)
        {
            //this.Dispatcher.Invoke(() => this.pianoControl.ReleaseAllKeys());
        }

        private void EnableTrackNotificationReceived(EnableTrackNotification msg)
        {
            var sequence = GetCurrentSequence();
            sequence.Tracks[msg.TrackIndex].Muted = false;
        }

        private void DisableTrackNotificationReceived(DisableTrackNotification msg)
        {
            var sequence = GetCurrentSequence();
            sequence.Tracks[msg.TrackIndex].Muted = true;
        }

        private void ShowTracksNotificationReceived(ShowTracksNotification msg)
        {
            Dispatcher.Invoke((Action)(() =>
            {

                this.tracksControl.Show();
            }));
        }

        private void HideTracksNotificationReceived(HideTracksNotification msg)
        {
          
            Dispatcher.Invoke((Action)(() =>
            {
                this.tracksControl.Hide();
            }));
        }

    }
}
