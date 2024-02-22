using Common;
using Common.Helpers;
using Common.Midi;
using Common.Models.Ensemble;
using Common.Models.Playlist;
using Common.Models.Settings;
using Common.Music;
using Hscm.UI.Controls;
using Hscm.UI.Notifications;
using Hscm.UI.Notifications.Playlist;
using Hscm.UI.Notifications.Settings.Pipe;
using Hscm.UI.Notifications.Tracks;
using Hscm.UI.Notifications.Window;
using Hscm.UI.ViewModels;
using Hscm.UI.ViewModels.MainWindow;
using Hscm.UI.ViewModels.Settings;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interop;
using Path = System.IO.Path;

namespace Hscm.UI
{

    public partial class PlaylistSettingsControl : StackPanel
    {
        public PlaylistSettingsControl()
        {
            InitializeComponent();
        }

        PlaylistSettingsViewModel viewModel;

        public void Initialize(PlaylistSettingsViewModel viewModel)
        {
            this.viewModel = viewModel;

            this.DataContext = viewModel;
        }


        public RelayCommand SaveSettingsCommand { get { return new RelayCommand(ExecuteSaveSettingsCommand); } }

        private void ExecuteSaveSettingsCommand()
        {
            var notification = new SaveSettingsNotification() { SaveAppSettings = true, SaveSongSettings = true, NotifyPlayerService = true };
            Messenger.Default.Send(notification);
        }


    }
}
