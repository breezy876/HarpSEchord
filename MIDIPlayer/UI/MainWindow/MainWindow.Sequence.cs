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
using Melanchall.DryWetMidi.Standards;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hscm.IPC;

namespace Hscm.UI
{
    public partial class MainWindow 
    {

        #region sequence

        private async Task ExecuteRemoveAllTracks()
        {
            var trackIndexes = viewModel.SelectedSequence.Tracks
                     .Where(t => t.Value.Cloned).Select(t => t.Key).ToArray();

            foreach (int index in trackIndexes)
                viewModel.SelectedSequence.Tracks.Remove(index);

            viewModel.SelectedSequence.RefreshTracks();

            this.tracksControl.UpdateTracks(viewModel.SelectedSequence);

            await SavePlaylistSettings();
        }

        private void MapPercussionToTracks(MidiSequence sequence, Track[] drumTracks)
        {
            if (!Common.Settings.AppSettings.TrackSettings.EnableChooseDrums)
                return;

            foreach (var track in drumTracks)
            {
                track.EnsembleInstrument = DrumMapper.GetMappedInstrument((GeneralMidiPercussion)track.PercussionNote).ToString();
                track.EnsembleMember = 0;
            }
        }

        private Track[] FilterToMappedDrums(MidiSequence sequence, Track[] drumTracks)
        {

            var newTracks = new List<Track>();

            foreach (var track in drumTracks)
            {
                if (!DrumMapper.IsMapped((GeneralMidiPercussion)track.PercussionNote))
                {
                    sequence.Tracks.Remove(track.Index);
                }
                else
                    newTracks.Add(track);
            }

            return newTracks.ToArray();
        }

        private async Task ExecuteSplitPercussion(bool split)
        {
            if (split)
            {
                var drumTracks = await midiProcessor.SplitPercussion(viewModel.SelectedSequence);
                drumTracks = FilterToMappedDrums(viewModel.SelectedSequence, drumTracks);
                MapPercussionToTracks(viewModel.SelectedSequence, drumTracks);
            }
            else
                midiProcessor.UnsplitPercussion(viewModel.SelectedSequence);

            viewModel.SelectedSequence.RefreshTracks();

            this.tracksControl.UpdateTracks(viewModel.SelectedSequence);

            await SavePlaylistSettings();
        }

        //private async Task ProcessSequence(int index)
        //{
        //    var sequence = viewModel.Playlist.GetByIndex(index).Sequence;
        //    await ProcessSequence(sequence);
        //}

        //private async Task ProcessSequence(MidiSequence sequence)
        //{

        //    bool processed = IsMidiProcessed(sequence.Info.Title);

        //    var task = await this.midiProcessor.LoadMidiFile(sequence.Info.FilePath, !processed || Common.Settings.AppSettings.GeneralSettings.LoadMidiFiles, true);

        //    //update cache
        //    Common.Settings.PlaylistSettings.Settings[sequence.Info.Title] = task.sequence;

        //    this.viewModel.Playlist.Update(task.sequence);
        //}

        private void UpdateDuration(int index, long length)
        {
            var sequence = viewModel.Playlist.GetByIndex(index).Sequence;
            sequence.Duration = new SequenceTimeSpan(length);
            this.viewModel.Playlist.Update(sequence);
        }


        private async Task HandleSelectedSequence(MidiSequence sequence)
        {

            if (playlistControl.DoingDragDrop)
                return;

            try
            {
                //if (Common.Settings.AppSettings.PlaylistSettings.LoadPlaylistSettings)
                //    Common.Settings.LoadPlaylistSettings();
            }
            catch (Exception ex)
            {
                AppendLog("", "Error: unable to load playlist settings.");
            }

            try
            {
                int index = playlistViewModel.GetSelectedIndex();

                //Common.Settings.AppSettings.CurrentSong = sequence.Info.Title;
                //Common.Settings.AppSettings.CurrentSongIndex = index;

                bool processed = IsMidiProcessed(sequence.Info.Title);

                var task = await this.midiProcessor.LoadMidiFile(sequence.Info.FilePath, !Common.Settings.AppSettings.GeneralSettings.UseMidiCache, true);

                //update cache
                Common.Settings.PlaylistSettings.Settings[sequence.Info.Title] = task.sequence;

                ChangeSequence(task.sequence, index);

                this.viewModel.ShowSongSettings = true;

                //if (task.save)
                //    await SavePlaylistSettings();

                ClientManager.ChangeSong(index);


            }
            catch (Exception ex)
            {
                AppendLog("", $"Error: unable to process MIDI '{this.viewModel.SelectedSequence}'.");
            }

        }

        private void ChangeSequence(MidiSequence sequence, int index)
        {

            this.viewModel.SelectedSequence = sequence;

            this.viewModel.Playlist.Update(sequence);

            UpdateSequenceInfoText(sequence);

            this.tracksControl.InitializeTracks(sequence);

            viewModel.UpdateSettings(sequence);

        }


        private MidiSequence GetCurrentSequence()
        {
            return this.viewModel.SelectedSequence;
        }

        private async Task ReloadSequence()
        {
            try
            {
                var sequence = this.viewModel.SelectedSequence;

                var task = await this.midiProcessor.LoadMidiFile(sequence.Info.FilePath, true, false);

                this.viewModel.SelectedSequence = task.sequence;

                this.viewModel.Playlist.Update(this.viewModel.SelectedSequence);

                Dispatcher.Invoke(() =>
                {
                    viewModel.UpdateSettings(this.viewModel.SelectedSequence);
                });


                this.tracksControl.InitializeTracks(this.viewModel.SelectedSequence);

                await ApplySettingsChanges(false, true);

            }
            catch (Exception ex)
            {
                AppendLog("", $"Error: unable to reload MIDI '{this.viewModel.SelectedSequence}'.");
            }
        }


        #endregion
    }
}
