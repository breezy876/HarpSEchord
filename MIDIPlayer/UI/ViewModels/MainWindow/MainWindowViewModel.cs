
using Hscm.UI.ViewModels;
using Hscm.UI.Notifications;
using Hscm.UI.Notifications.Window;
using Hscm.UI.ViewModels.Settings;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Hscm.UI.Notifications.Playlist;
using Hscm.UI.Notifications.Tracks;
using Common.Music;
using Hscm.UI.ViewModels.Ensemble;
using System.Windows.Input;
using Common;
using Common.Helpers;
using Hscm.UI.ViewModels.Ffxiv;

namespace Hscm.UI.ViewModels.MainWindow
{
    public partial class MainWindowViewModel : ObservableViewModel
    {


        private string title;
        private string filterText;
        private string messageText;

        private bool isFilterEnabled;
        private bool isFilterApplied;

        private bool showMessage;

        private bool loggedIn;

        private bool isInfoVisible;


        private MidiSequence selectedSequence;

        private string tempoTooltipText;
        private int infoControlColumn;
        private int playlistColumnSpan;

        private bool gameRunning;
        private bool hasItems;
        private bool showSongSettings;
        private bool tracksVisible;
        private bool ffxivConnected;
        private int ffxivControlColumn;
        private int settingsColumn;
        private int logControlColumn;

        public MainWindowViewModel(
            SettingsViewModel settingsViewModel, 
            PlaylistViewModel playlistViewModel, 
            TracksViewModel tracksViewModel,
            FfxivViewModel mbViewModel,
            InfoViewModel infoViewModel,
            LogViewModel logViewModel) : base()
        {
            this.Settings = settingsViewModel;
            this.Playlist = playlistViewModel;
            this.Tracks = tracksViewModel;
            this.Ffxiv = mbViewModel;
            this.Info = infoViewModel;
            this.Log = logViewModel;
            this.PlayerToolbar = new MainWindowPlayerToolbarViewModel();

            this.selectedSequence = new MidiSequence();

            this.PopulateSettings();

            RaiseSettingsPropertyChangedEvents();

            this.RaisePropertyChangedEvents();
        }

        #region properties
        public MainWindowPlayerToolbarViewModel PlayerToolbar { get; private set; }

        public PlaylistViewModel Playlist { get; private set; }

        public TracksViewModel Tracks { get; private set; }

        public FfxivViewModel Ffxiv { get; private set; }

        public SettingsViewModel Settings { get; private set; }

        public InfoViewModel Info { get; private set; }

        public LogViewModel Log { get; private set; }

        public bool UpdateTempo { get; set; }


        public bool FfxivConnected
        {
            get { return ffxivConnected; }
            set
            {
                ffxivConnected = value;
                RaisePropertyChanged();
            }
        }
        public bool ShowFfxiv
        {
            get { return Common.Settings.AppSettings.ShowFfxiv; }
            set
            {
                Common.Settings.AppSettings.ShowFfxiv = value;
                RaisePropertyChanged();
            }
        }

        public bool PluginFound
        {
            get { return pluginFound; }
            set
            {
                pluginFound = value;
                RaisePropertyChanged();
            }
        }

        public bool TracksVisible
        {
            get { return tracksVisible; }
            set { tracksVisible = value; RaisePropertyChanged(); }
        }

        public bool HasItems
        {
            get { return hasItems; }
            set { hasItems = value; RaisePropertyChanged(); }
        }

        public int PlaylistColumnSpan
        {
            get { return playlistColumnSpan; }
            set { playlistColumnSpan = value; RaisePropertyChanged(); }
        }

        public string TempoTooltipText
        {
            get { return tempoTooltipText; }
            set { tempoTooltipText = value; RaisePropertyChanged(); }
        }

        public string MessageText
        {
            get { return messageText; }
            set { messageText = value; RaisePropertyChanged(); }
        }

        public bool IsInfoVisible
        {
            get { return isInfoVisible; }
            set { isInfoVisible = value; RaisePropertyChanged(); }
        }

        public bool GameRunning
        {
            get { return gameRunning; }
            set { gameRunning = value; RaisePropertyChanged(); }
        }


        public bool ShowAdvancedLayout
        {
            get { return Common.Settings.AppSettings.ShowAdvancedLayout; }
            set { Common.Settings.AppSettings.ShowAdvancedLayout = value; RaisePropertyChanged(); }
        }

        public bool SettingsVisible
        {
            get { return Common.Settings.AppSettings.SettingsVisible; }
            set
            {
                Common.Settings.AppSettings.SettingsVisible = value;
                RaisePropertyChanged();
            }
        }

        public bool IsMessageVisible
        {
            get { return showMessage; }
            set { showMessage = value; RaisePropertyChanged(); }
        
        }
        public int SettingsColumn
        {
            get { return settingsColumn; }
            set { settingsColumn = value; RaisePropertyChanged(); }

        }

        public int FfxivControlColumn
        {
            get { return ffxivControlColumn; }
            set { ffxivControlColumn = value; RaisePropertyChanged(); }

        }

        public int InfoControlColumn
        {
            get { return infoControlColumn; }
            set { infoControlColumn = value; RaisePropertyChanged(); }

        }
        public int LogControlColumn
        {
            get { return logControlColumn; }
            set { logControlColumn = value; RaisePropertyChanged(); }
        }

        public bool ShowSongSettings
        {
            get { return showSongSettings; }
            set
            {
                showSongSettings = value;
                RaisePropertyChanged();
            }
        }


        public bool LoggedIn
        {
            get { return loggedIn; }
            set
            {
                loggedIn = value;
                RaisePropertyChanged();
            }
        }


        public string FilterText
        {
            get => filterText;
            set
            {
                filterText = value;
                RaisePropertyChanged();
            }

        }

        public bool IsFilterEnabled
        {
            get => isFilterEnabled;
            set
            {
                isFilterEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool IsFilterApplied
        {
            get => isFilterApplied;
            set
            {
                isFilterApplied = value;
                RaisePropertyChanged();
            }
        }


        public MidiSequence SelectedSequence
        {
            get { return selectedSequence; }
            set
            {
                selectedSequence = value;
                RaisePropertyChanged();
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                RaisePropertyChanged();
            }
        }



        public int OctaveOffset
        {
            get { return selectedSequence == null ? 0 : selectedSequence.OctaveOffset; }
            set
            {
                if (selectedSequence != null)
                    selectedSequence.OctaveOffset = value;

                RaisePropertyChanged();
            }
        }

        public int KeyOffset
        {
            get { return selectedSequence == null ? 0 : selectedSequence.KeyOffset; }
            set
            {
                if (selectedSequence != null)
                    selectedSequence.KeyOffset = value;

                RaisePropertyChanged();
            }
        }

        public bool ShowInfo
        {
            get { return Common.Settings.AppSettings.ShowInfo; }
            set
            {
                Common.Settings.AppSettings.ShowInfo = value;

                RaisePropertyChanged();
            }
        }

        public bool ShowLog
        {
            get { return Common.Settings.AppSettings.ShowLog; }
            set
            {
                Common.Settings.AppSettings.ShowLog = value;


                RaisePropertyChanged();
            }
        }

        public bool ShowTracks
        {
            get { return Common.Settings.AppSettings.ShowTracks; }
            set
            {
                Common.Settings.AppSettings.ShowTracks = value;
                RaisePropertyChanged();
            }
        }

        public bool IgnoreOffsetChanges { get; set; }
        #endregion


        #region commands



        public RelayCommand ToggleSettingsCommand { get { return new RelayCommand(ExecuteToggleSettingsCommand); } }

        public RelayCommand ToggleFfxivCommand { get { return new RelayCommand(ExecuteToggleFfxivCommand); } }


        public RelayCommand ToggleInfoCommand { get { return new RelayCommand(ExecuteToggleInfoCommand); } }

        public RelayCommand ToggleLogCommand { get { return new RelayCommand(ExecuteToggleLogCommand); } }

        public RelayCommand ToggleTracksCommand { get { return new RelayCommand(ExecuteToggleTracksCommand); } }

        public RelayCommand ToggleSongSettingsCommand { get { return new RelayCommand(ExecuteToggleSongSettingsCommand); } }

        public RelayCommand ToggleLayoutCommand { get { return new RelayCommand(ExecuteToggleLayoutCommand); } }

        public RelayCommand ReloadSequenceCommand { get { return new RelayCommand(ExecuteReloadSequenceCommand); } }


        private void ExecuteReloadSequenceCommand()
        {
            var notification = new ReloadSequenceNotification();

            Messenger.Default.Send(notification);
        }

        private void ExecuteToggleLayoutCommand()
        {
            SettingsVisible = ShowAdvancedLayout;
            TracksVisible = ShowAdvancedLayout;
            ShowInfo = ShowAdvancedLayout;
            ShowLog = ShowAdvancedLayout;
            ShowFfxiv = ShowAdvancedLayout;

            ExecuteToggleSettingsCommand();
            ExecuteToggleFfxivCommand();
            ExecuteToggleInfoCommand();
            ExecuteToggleLogCommand();

            var changedMsg = new LayoutChangedNotification() { Type = UI.MainWindow.LayoutType.Advanced };
            Messenger.Default.Send(changedMsg);

            var saveMsg = new SaveSettingsNotification() { SaveAppSettings = true };
            Messenger.Default.Send(saveMsg);
        }

        private void ExecuteToggleSettingsCommand()
        {
            if (SettingsVisible)
            {
                if (ShowFfxiv)
                {
                    SettingsColumn = 0;
                    FfxivControlColumn = 1;
                }
                else
                    SettingsColumn = 0;
            }
            else
                    FfxivControlColumn = 0;

            var saveMsg = new SaveSettingsNotification() { SaveAppSettings = true };
            Messenger.Default.Send(saveMsg);
        }

        private void ExecuteToggleFfxivCommand()
        {

            if (SettingsVisible)
            {
                if (ShowFfxiv)
                {
                    SettingsColumn = 0;
                    FfxivControlColumn = 1;
                }
                else
                    SettingsColumn = 0;
            }
            else
                FfxivControlColumn = 0;

            var saveMsg = new SaveSettingsNotification() { SaveAppSettings = true };
            Messenger.Default.Send(saveMsg);
        }


        private void ExecuteToggleInfoCommand()
        {
            if (ShowInfo)
            {
                if (ShowLog)
                {
                    InfoControlColumn = 0;
                    LogControlColumn = 1;
                }
                else
                    InfoControlColumn = 0;
            }
            else
                LogControlColumn = 0;

            var saveMsg = new SaveSettingsNotification() { SaveAppSettings = true };
            Messenger.Default.Send(saveMsg);
        }

        private void ExecuteToggleLogCommand()
        {
            if (ShowInfo)
            {
                if (ShowLog)
                {
                    InfoControlColumn = 0;
                    LogControlColumn = 1;
                }
                else
                    InfoControlColumn = 0;
            }
            else
                LogControlColumn = 0;

            var saveMsg = new SaveSettingsNotification() { SaveAppSettings = true };
            Messenger.Default.Send(saveMsg);
        }

        private void ExecuteToggleSongSettingsCommand()
        {
            var saveMsg = new SaveSettingsNotification() { SaveAppSettings = true };
            Messenger.Default.Send(saveMsg);
        }

        private void ExecuteToggleTracksCommand()
        {
            if (ShowTracks)
                PlaylistColumnSpan = 1;
            else
                PlaylistColumnSpan = 2;

            TracksVisible = ShowTracks;

            var saveMsg = new SaveSettingsNotification() { SaveAppSettings = true };
            Messenger.Default.Send(saveMsg);
        }

        public void Initialize()
        {
            ExecuteToggleSettingsCommand();
            ExecuteToggleFfxivCommand();
            ExecuteToggleInfoCommand();
            ExecuteToggleLogCommand();
        }
        #endregion

        #region public methods
        public void ShowMessage(string messageText)
        {
            IsMessageVisible = true;
            MessageText = messageText;
        }

        public void HideMessage()
        {
            IsMessageVisible = false;
        }

        public void ShowMidiNameInTitle(string midiName)
        {
            this.Title = $"HarpSEchord - {midiName}";
        }
        #endregion


        private void RaisePropertyChangedEvents()
        {
            this.Title = "HarpSEchord";

            this.FilterText = "Enter filter text...";

            this.SeekValue = 0;
            this.SequenceLength = 0;

            this.ShowSeekSlider = true;

            this.TimeElapsed = "00:00:00";
            this.TimeLeft = "00:00:00";

            this.TempoTooltipText = "Tempo 100%";


            InfoControlColumn = 0;
            LogControlColumn = 1;

            SettingsColumn = 0;
            FfxivControlColumn = 1;

            ShowSongSettings = false;

            //this.KeyOffset = 0;
            //this.OctaveOffset = 0;
        }

    }
}
        