

using Common;
using Common.Messaging.Player;
using Common.Midi;
using Common.Music;
using GalaSoft.MvvmLight.Messaging;
using Hscm.IPC;
using Hscm.UI.Notifications.Playlist;
using Hscm.UI.Notifications.Tracks;
using Melanchall.DryWetMidi.Interaction;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hscm.UI
{
    public partial class MainWindow
    {

        private PlayerState playerState;
        private MidiSequence currentSequence;
        private long position;
        private long duration;


        private TimeSpan timeElapsed;

        private void MoveTo(int index)
        {
            this.viewModel.Playlist.MoveTo(index);

        }

        private void UpdatePlaybackDisplay()
        {
            if (timeElapsed != null)
            {
                this.viewModel.TimeElapsed = new SequenceTimeSpan(timeElapsed).ToString();
                //this.timeElapsed.Content = this.viewModel.TimeElapsed;//needed because this WPF crap doesnt work with events
            }

            if (currentSequence.Duration != null)
            {
                this.viewModel.TimeLeft = currentSequence.Duration.ToString();
                //this.timeLeft.Content = this.viewModel.TimeLeft;//needed because this WPF crap doesnt work with events
            }

            this.viewModel.SeekValue = position;
        }


        #region player
        async Task Play()
        {
            try
            {
                await ClientManager.Play();
                //playlistControl.PlayCurrent();
            }

            catch
            {
                AppendLog("", $"Error: unable to resume playback.");
            }
        }

        private async Task Stop()
        {
            try
            {
                await ClientManager.Stop();
            }
            catch (Exception ex)
            {
                AppendLog("", $"Error: unable to stop playback.");
            }
        }

        private async Task Pause()
        {
            try
            {
                await ClientManager.Pause();
            }
            catch
            {
                AppendLog("", $"Error: unable to pause playback.");
            }
        }

        private async Task Next()
        {

            try
            {
                await ClientManager.Next();
            }
            catch
            {
                AppendLog("", $"Error: unable to skip to next MIDI.");
            }
        }

        private async Task Previous()
        {

            try
            {
                await ClientManager.Previous();
            }
            catch
            {
                AppendLog("", $"Error: unable to skip to previous MIDI.");
            }

        }


        private async Task CorrectSync()
        {
            await Seek(position);
        }

        private async Task Seek(long time)
        {
            try
            {
               await ClientManager.Seek(time);
            }
            catch
            {
                AppendLog("", $"Error: unable to seek to '{time}'.");
            }
        }


        private int GetTempoBpm()
        {
            var ts = TimeConverter.ConvertTo<MetricTimeSpan>(0, this.viewModel.SelectedSequence.TempoMap);
            return (int)(this.viewModel.SelectedSequence.TempoMap.GetTempoAtTime(ts).BeatsPerMinute * this.viewModel.Tempo);
        }

        private async Task<bool> ChangeTempo(double tempo)
        {
            try
            {
                if (this.viewModel.SelectedSequence == null)
                    return false;

                this.viewModel.Tempo = tempo;
                //this.viewModel.SelectedSequence.TempoBpm = GetTempoBpm();

                viewModel.TempoText = $"Tempo: {viewModel.Tempo}%";


                if (currentSequence == null)
                    return false;
                if (!currentSequence.Info.Title.Equals(this.viewModel.SelectedSequence.Info.Title))
                    return false;

                await ClientManager.ChangeSpeed((float)((float)this.viewModel.Tempo / 100.0));

                return true;

                //SavePlaylistSettings(false);

                //UpdateSequenceInfoText(this.viewModel.SelectedSequence);

            }
            catch
            {
                AppendLog("", $"Error: unable to change tempo to '{this.viewModel.Tempo}'.");
                return false;
            }
        }
        #endregion
    }
}
