using Common;
using Common.Helpers;
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI
{
    public partial class MainWindow
    {

        #region playlist
        private void HandleItemsChanged()
        {
            viewModel.HasItems = viewModel.Playlist.HasItems;
            if (!viewModel.HasItems)
            {
                viewModel.TracksVisible = false;
                viewModel.ShowSongSettings = false;
            }
            else
                viewModel.TracksVisible = viewModel.ShowTracks;
        }

        private void UpdatePlaylistSequence(string title, MidiSequence sequence)
        {
            var item = Common.Settings.Playlist.Items.First(i => i.Sequence.Info.Title == title);
            item.Sequence = sequence;
        }

        private async Task ShowLoadSongSettingsDialog()
        {
            var openFileDialog = new CommonOpenFileDialog();
            openFileDialog.Title = "Open MIDI Settings";
            openFileDialog.Filters.Add(new CommonFileDialogFilter("MIDI Settings Files", "*.json"));
            openFileDialog.DefaultExtension = "json";

            openFileDialog.InitialDirectory = Common.Settings.AppSettings.PrevPlaylistPath.ToAbsolutePath();
            openFileDialog.DefaultDirectory = Common.Settings.AppSettings.PrevPlaylistPath.ToAbsolutePath();

            var result = openFileDialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                Common.Settings.AppSettings.PrevPlaylistPath = Path.GetDirectoryName(openFileDialog.FileName);
                await LoadSongSettings (openFileDialog.FileName);
            }
        }

        private async Task ShowSaveSongSettingsDialog()
        {
            var saveFileDialog = new CommonSaveFileDialog();
            saveFileDialog.Title = "Save MIDI Settings";
            saveFileDialog.Filters.Add(new CommonFileDialogFilter("MIDI setting files", "*.json"));
            saveFileDialog.DefaultExtension = "json";
            saveFileDialog.DefaultFileName = this.viewModel.SelectedSequence.Info.Title;

            saveFileDialog.InitialDirectory = Common.Settings.AppSettings.PrevPlaylistPath.ToAbsolutePath();
            saveFileDialog.DefaultDirectory = Common.Settings.AppSettings.PrevPlaylistPath.ToAbsolutePath();

            var result = saveFileDialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                Common.Settings.AppSettings.PrevPlaylistPath = Path.GetDirectoryName(saveFileDialog.FileName);

                var filePath = Common.Helpers.AppHelpers.GetAppRelativePath(saveFileDialog.FileName);
                await SaveSongSettings(filePath);
            }
        }

        private async Task LoadSelectedSongSettings()
        {
            var filePath = this.viewModel.SelectedSequence.Info.FilePath.ToLower().Replace(AppHelpers.GetAppAbsolutePath().ToLower() + "\\midis\\", AppHelpers.GetAppAbsolutePath().ToLower() + "\\playlists\\").Replace(".mid", ".json");

            AppendLog("", $"Loading file '{filePath}'.");

            await LoadSongSettings(filePath);
        }

        private async Task LoadSongSettings(string filePath)
        {
            try
            {
                var settings = await Task.Run(() => FileHelpers.Load<MidiSequence>(filePath));

                if (settings == null)
                    return;

                //ignore index
                //var plSettings = Common.Settings.PlaylistSettings.Settings[settings.Info.Title];
                //settings.Index = plSettings.Index;

                foreach(var track in settings.Tracks)
                {
                    if (track.Value.Muted && track.Value.EnsembleMember != -1)
                        track.Value.EnsembleMember = -1;
                }

                midiProcessor.UpdateSequenceAndTrackSettings(this.viewModel.SelectedSequence, settings, true);

                //this.viewModel.UpdateSettings(this.viewModel.SelectedSequence);

                //var tracksMsg = new SequenceProcessedNotification() { Sequence = this.viewModel.SelectedSequence, EnsembleMembers = GetEnsembleMembers() };

                //Messenger.Default.Send(tracksMsg);

                if (Common.Settings.AppSettings.PlaylistSettings.SavePlaylistSettings)
                    await SavePlaylistSettings();

                UpdateSelectedSequence();

                //await PopulateClientInstrumentKeybinds(this.viewModel.SelectedSequence);

            }
            catch (Exception ex)
            {
                AppendLog("", $"Error: unable to load MIDI settings '{filePath}'.");
            }
        }

        private async Task SaveSongSettings(string filePath)
        {
            try
            {
                //this.viewModel.SelectedSequence.FixTracks();

                await Task.Run(() => FileHelpers.Save(this.viewModel.SelectedSequence, filePath));


                //SavePlaylistSettings();
            }
            catch (Exception ex)
            {
                AppendLog("", $"Error: unable to save MIDI settings '{filePath}'.");
            }
        }


        private bool IsMidiProcessed(string title) => Common.Settings.PlaylistSettings.Settings.ContainsKey(title) && Common.Settings.PlaylistSettings.Settings[title].Processed;


        private async Task SavePlaylistAndSettings()
        {

            try
            {
                if (Common.Settings.AppSettings.PlaylistSettings.SavePlaylistSettings)
                    await SavePlaylistSettings();
            }
            catch (Exception ex)
            {
                AppendLog("", "Error: unable to save playlist settings.");
            }

            try
            {
                if (Common.Settings.AppSettings.PlaylistSettings.SavePlaylist)
                    await playlistControl.Save();
            }
            catch (Exception ex)
            {
                AppendLog("", "Error: unable to save playlist.");
            }

        }

        private async Task SavePlaylistSettings()
        {

            //await Common.Settings.SaveCharConfig();

            await playlistViewModel.SavePlaylistSettings();
        
        }
      
        #endregion
    }
}
