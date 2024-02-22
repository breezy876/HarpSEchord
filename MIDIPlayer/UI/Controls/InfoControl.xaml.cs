using Hscm.UI.Notifications;
using Hscm.UI.Notifications.Playlist;
using Hscm.UI.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hscm.UI
{
    /// <summary>
    /// Interaction logic for InfoControl.xaml
    /// </summary>
    public partial class InfoControl : StackPanel
    {
        private InfoViewModel viewModel;

        public InfoControl()
        {
            InitializeComponent();
        }

        public void Initialize(InfoViewModel viewModel)
        {
            this.viewModel = viewModel;

            this.DataContext = viewModel;

            this.viewModel.StatusText = "Not playing";

            Messenger.Default.Register<UpdateInfoNotification>(this, UpdateInfoNotificationReceived);
            Messenger.Default.Register<PlaylistEmptyNotification>(this, PlaylistEmptyNotificationReceived);
        }

        private void UpdateInfoNotificationReceived(UpdateInfoNotification msg)
        {
            this.viewModel.NoteRangeText = msg.NoteRangeText;
            this.viewModel.BpmText = msg.BpmText;
            this.viewModel.LengthText = msg.LengthText;
            this.viewModel.DivisionText = msg.DivisionText;
            this.viewModel.TrackCountText = msg.TrackCountText;
            this.viewModel.NoteCountText = msg.NoteCountText;
        }

        public void ShowSpinner(string title)
        {
            this.viewModel.StatusText = $"Loading {title}...";
            this.viewModel.SpinnerVisible = true;
        }

        public void HideSpinner(bool playing = false)
        {
            if (!playing)
                this.viewModel.StatusText = $"Not playing";
            this.viewModel.SpinnerVisible = false;
        }

        public void UpdateStatus(string title, bool playing = false, string progress = "")
        {
            this.viewModel.StatusText = playing ? $"Playing {title} ({progress})" : "Not playing";
        }

        private void PlaylistEmptyNotificationReceived(PlaylistEmptyNotification msg)
        {
            Dispatcher.Invoke(() =>
            {
                this.viewModel.Clear();
            });
        }

    }
}
