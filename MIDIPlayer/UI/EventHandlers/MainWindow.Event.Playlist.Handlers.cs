using Common;
using Hscm.UI.Controls;
using Hscm.UI.Notifications.Playlist;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hscm.UI.Services;

namespace Hscm.UI
{
    public partial class MainWindow
    {
  

        #region playlist events
        private void OnFileAddStarted(object snder, EventArgs e)
        {

        }

        private  async void OnFileAddComplete(object snder, FileAddCompleteEventArgs e)
        {
            if (!e.LoadingPlaylist)
                await playlistViewModel.Save();

            this.viewModel.PlayerToolbar.IsVisible = true;

            //if (playerState == PlayerState.Playing)
            //{
            //    this.viewModel.PlayerToolbar.ShowPlayButton = true;
            //    this.viewModel.PlayerToolbar.ShowPauseButton = false;
            //}
            //else
            //{
            //    this.viewModel.PlayerToolbar.ShowPlayButton = false;
            //    this.viewModel.PlayerToolbar.ShowPauseButton = true;
            //}

            //this.viewModel.ShowProgress = false;
            this.viewModel.ShowTimer = true;

            this.viewModel.ShowSeekSlider = true;

            //if (!e.LoadingPlaylist)
            //    await SaveSettings(null, false, true);
        }
        #endregion
    }
}
