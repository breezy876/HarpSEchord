using Common;
using Common.FFXIV;
using Common.Helpers;
using Common.Interop;
using Common.Messaging;
using Common.Messaging.Settings;
using Common.Midi;
using Common.Models;
using Common.Models.FFXIV;
using Common.Models.Settings;
using Common.Music;
using Hscm.UI.Notifications;
using Hscm.UI.Notifications.Playlist;
using Hscm.UI.Notifications.Settings;
using Hscm.UI.Notifications.Settings.Pipe;
using Hscm.UI.Notifications.Tracks;
using Hscm.UI.Notifications.Window;
using Hscm.UI.ViewModels;
using Hscm.UI.ViewModels.Ensemble;
using Hscm.UI.ViewModels.MainWindow;
using Hscm.UI.ViewModels.Settings;
using GalaSoft.MvvmLight.Messaging;
using Melanchall.DryWetMidi.Common;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;
using Hscm.IPC;
using static System.Net.Mime.MediaTypeNames;
using Common.IO;

namespace Hscm.UI
{
    public partial class MainWindow
    {
        #region misc

        public enum LayoutType { Info, Log, Settings, Ensemble, Advanced }


        private void UpdateSelectedSequence()
        {
            if (this.viewModel.SelectedSequence == null || string.IsNullOrEmpty(this.viewModel.SelectedSequence.Info.Title))
                return;

            this.viewModel.UpdateSettings(this.viewModel.SelectedSequence);

            tracksControl.InitializeTracks(this.viewModel.SelectedSequence);
        }


        private void CreateAppDirectories()
        {
            if (!Directory.Exists(Common.Paths.PlaylistPath))
                Directory.CreateDirectory(Common.Paths.PlaylistPath);

            if (!Directory.Exists(Common.Paths.MidiFilePath))
                Directory.CreateDirectory(Common.Paths.MidiFilePath);
        }

        private void Initialize()
        {
            Common.Settings.InitializeAppSettings();

            Common.Settings.LoadAppSettings();

            CreateAppDirectories();

            SetupCore();

            Dispatcher.Invoke(() => CreateChildWindows());

            InitializeNotifications();
        }

        #region info display
        private void UpdateSequenceInfoText(MidiSequence sequence)
        {

            var lengthText = $"Length: {sequence.Duration}";

            //var bpmText = $"BPM: {(int)(sequence.TempoBpm * (this.viewModel.Tempo / 100.0))}";

            //var divisionText = $"Division: {sequence.Division}";

            var trackCountText = $"Total Tracks: {sequence.Tracks.Count()}";

            var noteCountText = $"Total Notes: {sequence.TotalNotes}";

            var msg = new UpdateInfoNotification()
            {
                LengthText = lengthText,
                TrackCountText = trackCountText,
                NoteCountText = noteCountText,
                NoteRangeText = sequence.Range
            };

            Messenger.Default.Send(msg);
        }

        #endregion

        private void Cleanup()
        {

            //if (serverPipe != null)
            //{
            //    serverPipe.Stop();
            //    serverPipe = null;
            //}        

            ClientManager.Dispose();
        }

        public void AppendLog(string serviceName, string text)
        {
           logControl.AppendLog(serviceName, text);
        }

        private GameClientInfo GetClientInfoByWindowHandle(IntPtr windowHandle)
        {
            if (gameClients.IsNullOrEmpty())
                return null;

            var clientInfo = this.gameClients.Values.FirstOrDefault(c => c.WindowHandle == windowHandle);

            return clientInfo;
        }

        private void SetupCore()
        {

            gameClients = new Dictionary<Int64, GameClientInfo>();

            processes = new Dictionary<string, Process>();

            midiProcessor = new MidiProcessor();

            midiProcessor.LoadStarted += OnLoadStarted;
            midiProcessor.SequenceLoaded += OnSequenceLoaded;
            midiProcessor.ProcessStarted += OnProcessStarted;
            midiProcessor.ProcessFinished += OnProcessFinished;

            MessageHandler.ConnectMessageReceived += OnConnectMessageReceived;
            MessageHandler.DisconnectMessageReceived += OnDisconnectMessageReceived;
            MessageHandler.LoadStartedMessageReceived += OnLoadStartedMessageReceived;
            MessageHandler.LoadFinishedMessageReceived += OnLoadFinishedMessageReceived;
            MessageHandler.PlaybackStartedMessageReceived += OnPlaybackStartedMessageReceived;
            MessageHandler.PlaybackFinishedMessageReceived += OnPlaybackFinishedMessageReceived;
            MessageHandler.PausedMessageReceived += OnPausedMessageReceived;
            MessageHandler.StoppedMessageReceived += OnStoppedMessageReceived;
            MessageHandler.TickedMessageReceived += OnTickedMessageReceived;
        }


        private void InitializeCore()
        {
            ClientManager.Connected += IpcClient_Connected;
            ClientManager.Disconnected += IpcClient_Disconnected;

            ClientManager.Initialize();
        }

        private void CreateChildWindows()
        {
                this.playlistSettingsViewModel = new PlaylistSettingsViewModel();
                this.playlistViewModel = new PlaylistViewModel(Common.Settings.Playlist);
                this.tracksViewModel = new TracksViewModel();
                this.infoViewModel = new InfoViewModel();
                this.logViewModel = new LogViewModel();

                this.ffxivViewModel = new ViewModels.Ffxiv.FfxivViewModel();

                this.viewModel = new MainWindowViewModel(settingsViewModel, playlistViewModel, tracksViewModel, ffxivViewModel, infoViewModel, logViewModel);
                this.settingsViewModel = new SettingsViewModel();

                this.playlistControl.FileAddStarted += this.OnFileAddStarted;
                this.playlistControl.FileAddComplete += this.OnFileAddComplete;

                this.DataContext = this.viewModel;

                this.viewModel.SeekValue = 0;
        }

        private void InitializeChildWindows()
        {
            this.playlistControl.Initialize(playlistViewModel, playlistSettingsViewModel, this.midiProcessor);
            this.tracksControl.Initialize(tracksViewModel, this);
            FfxivControl.Initialize(ffxivViewModel);
            this.infoControl.Initialize(infoViewModel);
            this.logControl.Initialize(logViewModel);
            this.settingsControl.Initialize(settingsViewModel);

        }

        private void CenterWindow(Window window)
        {
            window.Left = System.Windows.Application.Current.MainWindow.Left + (System.Windows.Application.Current.MainWindow.Width - window.ActualWidth) / 2;
            window.Top = System.Windows.Application.Current.MainWindow.Top + (System.Windows.Application.Current.MainWindow.Height - window.ActualHeight) / 2;
        }

        private void ShowPopupMessage(string msg)
        {
            //Dispatcher.Invoke(() => {

            //    var txtBlock = VisualTreeHelper.GetChild(msgPopup.Child, 0) as System.Windows.Controls.Label;
            //    txtBlock.Content = msg;
            //    this.msgPopup.IsOpen = true;
            //    this.msgPopup.StaysOpen = false;

            //    msgTimer = new DispatcherTimer(System.Windows.Threading.DispatcherPriority.Normal);

            //    msgTimer.Interval = TimeSpan.FromSeconds(3);
            //    msgTimer.Tick += new EventHandler(msgTimer_Tick);
            //    msgTimer.Start();

            //});

        }

        private void msgTimer_Tick(object sender, EventArgs e)
        {
            //msgPopup.IsOpen = false;
            //msgTimer.Stop();
        }


        private void IncreaseOctaveOffset()
        {
            if (this.viewModel.OctaveOffset < 2)
                this.viewModel.OctaveOffset++;

        }

        private void DecreaseOctaveOffset()
        {
            if (this.viewModel.OctaveOffset > -2)
                this.viewModel.OctaveOffset--;

        }

        private void IncreaseKeyOffset()
        {
            if (this.viewModel.KeyOffset < 32)
                this.viewModel.KeyOffset++;

        }

        private void DecreaseKeyOffset()
        {
            if (this.viewModel.KeyOffset > -32)
                this.viewModel.KeyOffset--;

        }

        #endregion
    }
}
