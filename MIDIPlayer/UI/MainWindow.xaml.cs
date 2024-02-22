
using Common;
using Common.FFXIV;
using Common.Helpers;
using Common.Interop;
using Common.Messaging;
using Common.Messaging.Player;
using Common.Messaging.Settings;
using Common.Midi;
using Common.Models.Ensemble;
using Common.Models.Playlist;
using Common.Models.Settings;
using Common.Music;
using Hscm.UI.Notifications.Settings.Pipe;
using Hscm.UI.Notifications.Tracks;
using Hscm.UI.Notifications.Window;
using Hscm.UI.ViewModels;
using Hscm.UI.ViewModels.Ensemble;
using Hscm.UI.ViewModels.MainWindow;
using Hscm.UI.ViewModels.Settings;
using GalaSoft.MvvmLight.Messaging;

using Hscm.UI.Notifications;
using Hscm.UI.Notifications.Playlist;
using Hscm.UI.Notifications.Settings;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using static Common.FFXIV.FFXIVKeybindDat;
using Common.Models.FFXIV;
using Hscm.UI.ViewModels.Ffxiv;

namespace Hscm.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region fields
        const string PipeName = "HSC.pipe";

        MidiProcessor midiProcessor;

        MainWindowViewModel viewModel;
        TracksViewModel tracksViewModel;
        PlaylistViewModel playlistViewModel;
        PlaylistSettingsViewModel playlistSettingsViewModel;
        FfxivViewModel ffxivViewModel;
        SettingsViewModel settingsViewModel;
        InfoViewModel infoViewModel;
        LogViewModel logViewModel;

        System.Windows.Controls.ToolTip toolTip;

        bool sliderChangeBegin;
        bool isReload;
        bool shown;

        bool mouseWheelShiftDown;

        private bool tempoRevertStarted;

        private Dictionary<HotKeyType, System.Action> hotkeyActions;

        double prevTempo;


        private Dictionary<string, Process> processes;

        private Dictionary<Int64, GameClientInfo> gameClients;

        private bool manualTempoChange;

        private bool finished;


        private bool workerServiceRunning;

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            InitToolTip();
        }

        private void InitToolTip()
        {
            toolTip = new System.Windows.Controls.ToolTip();

            toolTip.Opened += async delegate (object o, RoutedEventArgs args)
            {
                var s = o as System.Windows.Controls.ToolTip;
                // let the tooltip display for 1 second
                await Task.Delay(2000);
                s.IsOpen = false;

            };
        }

    }
}
