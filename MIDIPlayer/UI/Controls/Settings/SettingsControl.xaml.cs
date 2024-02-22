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

    public partial class SettingsControl : StackPanel
    {
        public SettingsControl()
        {
            InitializeComponent();
        }

        SettingsViewModel viewModel;

        public void Initialize(SettingsViewModel viewModel)
        {
            this.viewModel = viewModel;

            this.DataContext = viewModel;

            //Messenger.Default.Register<PipeSettingsWindowChangeNotification>(this, PipeSettingsChangeNotificationReceived); Messenger.Default.Register<AddLogNotification>(this, AddLogNotificationReceived);
        }

        public SettingsControl(SettingsViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = viewModel;
            this.DataContext = this.viewModel;

            //RegisterNotifications();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
