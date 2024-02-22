using Common;
using Common.Helpers;
using Common.Interop;
using Common.Midi;
using Common.Music;
using Hscm.UI.Controls;
using Hscm.UI.Notifications;
using Hscm.UI.Notifications.Playlist;
using GalaSoft.MvvmLight.Messaging;
using Melanchall.DryWetMidi.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hscm.IPC;

namespace Hscm.UI
{
    public partial class MainWindow
    {

        #region client events
        public void OnDisconnectMessageReceived(object sender, int index)
        {
            Reset();
            ClientManager.Disconnect(index);
        }

        public void OnConnectMessageReceived(object sender, int index)
        {
            ClientManager.Connect(index);
        }

        public void OnLoadStartedMessageReceived(object sender, int index)
        {
            AppendLog("Plugin", $"Load started for {index} received");

            switch (playerState)
            {
                case PlayerState.Stopped:
                case PlayerState.Finished:
                    position = 0;
                    playerState = PlayerState.NotStarted;
                    timeElapsed = new TimeSpan(0, 0, 0);
                    break;
            }

            playerState = PlayerState.NotStarted;

        }

        public void OnLoadFinishedMessageReceived(object sender, (int index, long length) args)
        {
            //if (playerState == PlayerState.Playing)
            //    return;

            AppendLog("Plugin", $"Load finished for {args.index} received");
            duration = args.length / 1000;
            MoveTo(args.index);
        }

        public void OnPlaybackStartedMessageReceived(object sender, int index)
        {
            currentSequence = this.viewModel.Playlist.GetCurrent().Sequence;

            AppendLog("Plugin", "Playback started message received");


            AppendLog("Plugin", $"Length: {currentSequence.LengthMs}");

            this.viewModel.IsSeekSliderEnabled = true;

            this.viewModel.ShowTimer = true;

            this.viewModel.PlayerToolbar.StopButtonEnabled = true;
            this.viewModel.PlayerToolbar.ShowPlayButton = false;
            this.viewModel.PlayerToolbar.ShowPauseButton = true;

            if (playerState == PlayerState.NotStarted && !isReload)
                position = 0;

            playerState = PlayerState.Playing;

            viewModel.SequenceLength = duration;
            UpdateDuration(index, viewModel.SequenceLength);
            playlistControl.PlayCurrent();
            viewModel.UpdateProgress(position, (long)currentSequence.Duration.Current.TotalMilliseconds);
            this.infoControl.UpdateStatus(currentSequence.Info.Title, true, viewModel.Progress);
            this.UpdatePlaybackDisplay();
        }

        public  void OnPlaybackFinishedMessageReceived(object sender, EventArgs e)
        {

            AppendLog("Plugin", "Playback finished message received");


            this.viewModel.IsSeekSliderEnabled = false;

            this.viewModel.PlayerToolbar.ShowPlayButton = true;
            this.viewModel.PlayerToolbar.ShowPauseButton = false;
            this.viewModel.PlayerToolbar.StopButtonEnabled = false;

            position = 0;
            timeElapsed = new TimeSpan(0, 0, 0);
            playerState = PlayerState.Finished;
            viewModel.UpdateProgress(position, (long)currentSequence.Duration.Current.TotalMilliseconds);
            this.infoControl.UpdateStatus(currentSequence.Info.Title, false, viewModel.Progress);
            this.UpdatePlaybackDisplay();
        }

        public  void OnPausedMessageReceived(object sender, EventArgs e)
        {
            AppendLog("Plugin", "Paused message received");


            this.viewModel.PlayerToolbar.ShowPauseButton = false;
            this.viewModel.PlayerToolbar.ShowPlayButton = true;

            playerState = PlayerState.Paused;
            viewModel.UpdateProgress(position, (long)currentSequence.Duration.Current.TotalMilliseconds);
            this.infoControl.UpdateStatus(null, false, viewModel.Progress);
            this.UpdatePlaybackDisplay();
        }

        public  void OnStoppedMessageReceived(object sender, EventArgs e)
        {
            AppendLog("Plugin", "Stopped message received");

            Reset();
        }


        public  void OnTickedMessageReceived(object sender, (TimeSpan ts, int position) args)
        {
            if (sliderChangeBegin)
                return;

            //AppendLog("Plugin", $"Ticked message {args.ts.Minutes}:{args.ts.Seconds} {args.position} received");

            position = args.position/1000;
            timeElapsed = args.ts;

            viewModel.UpdateProgress(position, (long)currentSequence.Duration.Current.TotalMilliseconds);
            this.infoControl.UpdateStatus(currentSequence.Info.Title, true, viewModel.Progress);
            this.UpdatePlaybackDisplay();
        }

        private void Reset()
        {

            this.viewModel.IsSeekSliderEnabled = false;

            this.viewModel.PlayerToolbar.StopButtonEnabled = false;
            this.viewModel.PlayerToolbar.ShowPlayButton = true;
            this.viewModel.PlayerToolbar.ShowPauseButton = false;

            position = 0;
            timeElapsed = new TimeSpan(0, 0, 0);
            playerState = PlayerState.Stopped;
            viewModel.UpdateProgress(position, (long)currentSequence.Duration.Current.TotalMilliseconds);
            this.infoControl.UpdateStatus(null, false, viewModel.Progress);
            this.UpdatePlaybackDisplay();
        }

        //private void OnSkipStart(PlayerEventArgs args)
        //{
        //    //this.Dispatcher.Invoke(() => this.pianoControl.ReleaseAllKeys());

        //    UpdatePlaybackDisplay(args.Info);
        //}

        //private void OnSkipEnd(PlayerEventArgs args)
        //{
        //    //this.Dispatcher.Invoke(() => this.pianoControl.ReleaseAllKeys());

        //    playerState = args.Info.PlayerState;
        //    playerInfo = args.Info;

        //    UpdatePlaybackDisplay(args.Info);
        //}

        //private void OnPositionChanged(PlayerEventArgs args)
        //{
        //    //this.Dispatcher.Invoke(() => this.pianoControl.ReleaseAllKeys());

        //    playerState = args.Info.PlayerState;
        //    playerInfo = args.Info;

        //    UpdatePlaybackDisplay(args.Info);
        //}

        //private void OnTempoChanged(PlayerEventArgs args)
        //{
        //    playerState = args.Info.PlayerState;
        //    playerInfo = args.Info;
        //    //if (manualTempoChange)
        //    //this.player.GotoPrevious();
        //}
        #endregion

    }
}
