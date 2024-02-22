using Common;
using Common.Music;
using Hscm.UI.Controls;
using Hscm.UI.Notifications;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Hscm.UI
{
    public partial class MainWindow
    {

        #region midi processor events
        private void SequenceLoadedOrProcessed(MidiSequence e)
        {
            this.viewModel.PlayerToolbar.IsVisible = true;
        }

        private void OnLoadOrProcessStarted(MidiSequence e)
        {
            this.viewModel.PlayerToolbar.IsVisible = false;

            this.Dispatcher.Invoke(() =>
            {
                tracksControl.Show();

            });
        }

        private void OnProcessStarted(object sender, MidiSequence e)
        {
            this.infoControl.ShowSpinner(e.Info.Title);
            this.tracksControl.ShowSpinner();
            this.OnLoadOrProcessStarted(e);
        }

        private void OnProcessFinished(object sender, MidiSequence e)
        {

            this.infoControl.HideSpinner(playerState == PlayerState.Playing);
            this.tracksControl.HideSpinner();

            this.SequenceLoadedOrProcessed(e);

            isReload = false;
        }

        private void OnLoadStarted(object sender, MidiSequence e)
        {
            this.OnLoadOrProcessStarted(e);
        }

        private void OnSequenceLoaded(object sender, MidiSequence e)
        {
            this.SequenceLoadedOrProcessed(e);

        }
        #endregion
    }
}
