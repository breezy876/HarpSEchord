using Common;
using Common.Models.Ensemble;
using Common.Music;
using GalaSoft.MvvmLight.Messaging;
using Hscm.UI.Notifications.Playlist;
using Hscm.UI.Notifications.Tracks;
using Hscm.UI.ViewModels;
using Hscm.UI.ViewModels.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace Hscm.UI
{

    public partial class TracksControl : StackPanel
    {

        private TracksViewModel viewModel;
        MainWindow parent;
        System.Windows.Controls.Primitives.Popup popup;
        private CharactersPanelViewModel charPanelViewModel;
        private CharactersPanel charPanel;
        private bool popupFocused;

        public TracksControl()
        {
            InitializeComponent();
        }

        public void Initialize(TracksViewModel viewModel, MainWindow parent)
        {
            this.viewModel = viewModel;

            InitializeComponent();

            this.parent = parent;

            //Messenger.Default.Register<SequenceProcessedNotification>(this, SequenceProcessedNotificationReceived);
            Messenger.Default.Register<KeyOffsetChangedNotification>(this, KeyOffsetChangedNotificationReceived);
            Messenger.Default.Register<OctaveOffsetChangedNotification>(this, OctaveOffsetChangedNotificationReceived);
            Messenger.Default.Register<TrackChangedNotification>(this, TrackChangedNotificationReceived);
            Messenger.Default.Register<ShowInstrumentsPopupNotification>(this, ShowInstrumentsPopupNotificationReceived);
            Messenger.Default.Register<TrackInstrumentChosenNotification>(this, TrackInstrumentChosenNotificationReceived);
            Messenger.Default.Register<TrackEnsembleMemberChangedNotification>(this, TrackEnsembleMemberChangedNotificationReceived);
            Messenger.Default.Register<ShowCharactersPopupNotification>(this, ShowCharactersPopupNotificationReceived);
            Messenger.Default.Register<CharacterToggledNotification>(this, CharacterToggledNotificationReceived);
            Messenger.Default.Register<UpdateCharactersNotification>(this, UpdateCharactersNotificationReceived);

            this.DataContext = viewModel;


            charPanelViewModel = new CharactersPanelViewModel();
            charPanel = new CharactersPanel();
            charPanel.Initialize(charPanelViewModel);


            popup = new System.Windows.Controls.Primitives.Popup();
            popup.StaysOpen = true;
            popup.AllowsTransparency = true;
            

            this.Children.Add(popup);

            parent.LocationChanged += delegate (object sender, EventArgs args)
            {
                var offset = popup.HorizontalOffset;
                popup.HorizontalOffset = offset + 1;
                popup.HorizontalOffset = offset;
            };

            if (Common.Settings.AppSettings.GeneralSettings.TestMode)
            {
                if (!viewModel.LoggedIn)
                    viewModel.LoggedIn = true;
            }
        }

        private void UpdateCharactersNotificationReceived(UpdateCharactersNotification obj)
        {
            if (obj.IsSelected)
                charPanel.SelectCharacter(obj.CharacterName);
            else
                charPanel.DeselectCharacter(obj.CharacterName);
        }

        private void CharacterToggledNotificationReceived(CharacterToggledNotification obj)
        {
            viewModel.HandleCharacterToggled(obj.CharacterName, obj.IsSelected);
        }

        public IEnumerable<int> OctaveOffsets => this.viewModel.Tracks.Select(t => t.OctaveOffset);

        public IEnumerable<int> KeyOffsets => this.viewModel.Tracks.Select(t => t.KeyOffset);


        public void ShowSpinner()
        {
            this.viewModel.SpinnerVisible = true;
        }

        public void HideSpinner()
        {
            this.viewModel.SpinnerVisible = false;
        }

        public void HandleGameWindowExited(string charName)
        {
            RemoveEnsembleMember(charName, true);
            viewModel.UpdateEnsembleMemberDisplay();
            UpdateCharactersDropdown();
        }

        public void HandleGameWindowFound(string charName, int index)
        {
            AddEnsembleMember(charName, index);
            viewModel.UpdateEnsembleMemberDisplay();
            UpdateCharactersDropdown();
        }

        public void AddEnsembleMember(string charName, int index)
        {
            Dispatcher.Invoke(() =>
            {
                this.viewModel.AddEnsembleMember(charName, index);
                this.charPanelViewModel.AddCharacter(new FFXIVCharacter() { CharacterName = charName, Index = index });
            });
        }

        public void RemoveEnsembleMember(string charName, bool ignoreSave = false)
        {
            Dispatcher.Invoke(() =>
            {
                this.viewModel.RemoveEnsembleMember(charName, ignoreSave);
                this.charPanelViewModel.RemoveCharacter(charName);
            });
        }

        public void ChangeAllOctaveOffsets(int offset)
        {
            foreach (var track in this.viewModel.Tracks)
            {
                UpdateOffsetIfRequired(track, offset, true);
            }
        }

        public void ChangeAllKeyOffsets(int offset)
        {
            foreach (var track in this.viewModel.Tracks)
            {
                UpdateOffsetIfRequired(track, offset, false);
            }
        }

        public void Show()
        {
            this.viewModel.IsVisible = true;
        }

        public void Hide()
        {
            this.viewModel.IsVisible = false;
        }


        public void InitializeTracks(MidiSequence sequence)
        {
            viewModel.Caption = sequence.Info.Title + " - Tracks";

            viewModel.ClearMemberCache();
            UpdateTracks(sequence);
        }

        public void UpdateTracks(MidiSequence sequence)
        {
            viewModel.Update(sequence);

            AddCharsFromConfigInTestMode();

            UpdateEnsembleMembers();

            UpdateCharactersDropdown();

            if (!Common.Settings.AppSettings.TrackSettings.PopulateFromPlaylist)
            {
                viewModel.Autopopulate(true);
                sequence.Autofilled = true;
            }
            else
                sequence.Autofilled = false;
        }
        void AddCharsFromConfigInTestMode()
        {
            if (Common.Settings.AppSettings.GeneralSettings.TestMode)
            {
                var charConfig = CharConfigReader.Load();

                foreach (var chara in charConfig.ToDictionary())
                {
                    AddEnsembleMember(chara.Key, chara.Value);
                }
            }
        }

            
        internal void UpdateCharactersDropdown()
        {
            viewModel.UpdateCharactersDropDown();
        }

        public void HandleAllClientsExited()
        {
            viewModel.LoggedIn = false;
        }

        public void HandleAllClientsLoggedOut()
        {
            viewModel.LoggedIn = false;
        }

        public void HandleClientLoggedIn()
        {
            viewModel.LoggedIn = true;
        }

        #region notifications

        private void TrackChangedNotificationReceived(TrackChangedNotification msg)
        {
            viewModel.HandleTrackChanged();

            SaveSongSettings();
        }

        private void KeyOffsetChangedNotificationReceived(KeyOffsetChangedNotification msg)
        {
            viewModel.HandleTrackChanged();

            ChangeAllKeyOffsets(msg.Offset);

            SaveSongSettings();
        }

        private void OctaveOffsetChangedNotificationReceived(OctaveOffsetChangedNotification msg)
        {
            viewModel.HandleTrackChanged();

            ChangeAllOctaveOffsets(msg.Offset);

            SaveSongSettings();

        }
        private void TrackEnsembleMemberChangedNotificationReceived(TrackEnsembleMemberChangedNotification obj)
        {
            viewModel.HandleTrackChanged();

            viewModel.HandleTrackEnsembleMemberChanged(obj.Member, obj.TrackIndex);

            UpdateCharactersDropdown();

            SaveSongSettings();
        }

        private void TrackInstrumentChosenNotificationReceived(TrackInstrumentChosenNotification obj)
        {
            ChangeTrackInstrument(obj.TrackIndex, obj.Instrument.Replace(" ", ""));
        }

        private void ChangeTrackInstrument(int trackIndex, string instrument)
        {
            this.viewModel.ChangeTrackInstrument(trackIndex, instrument);
            popup.IsOpen = false;
        }

        private void ShowInstrumentsPopupNotificationReceived(ShowInstrumentsPopupNotification obj)
        {
            var control = new InstrumentPanel();
            control.Initialize(new InstrumentPanelViewModel() { TrackIndex = obj.TrackIndex });

            ShowPopupAt((UIElement)obj.Control, control, System.Windows.Controls.Primitives.PlacementMode.Left);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (!popupFocused)
                popup.IsOpen = false;
                //popup.StaysOpen = false;
            });
        }

        private void ShowCharactersPopupNotificationReceived(ShowCharactersPopupNotification obj)
        {
            ShowPopupAt((UIElement)obj.Parent, charPanel, System.Windows.Controls.Primitives.PlacementMode.Left);
        }
        #endregion


        #region private methods
        private void ShowPopupAt(UIElement parent, Control child, System.Windows.Controls.Primitives.PlacementMode placement)
        {
            //set the target to current user control
            popup.PlacementTarget = parent;
            //to the center of that current user control
            popup.Placement = placement;
            popup.Child = child;

            popup.IsOpen = true;

            popup.GotFocus -= Popup_GotFocus;
            popup.LostFocus -= Popup_LostFocus;
            popup.MouseEnter -= Popup_MouseEnter;
            popup.MouseLeave -= Popup_MouseLeave;

            popup.GotFocus += Popup_GotFocus;
            popup.LostFocus += Popup_LostFocus;
            popup.MouseEnter += Popup_MouseEnter;
            popup.MouseLeave += Popup_MouseLeave;

            var timer = new Timer();
            timer.Interval = 2000;
            timer.AutoReset = false;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Popup_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Released)
                popup.IsOpen = false;
            popupFocused = false;

        }

        private void Popup_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            popupFocused = true;
        }

        private void Popup_LostFocus(object sender, RoutedEventArgs e)
        {
            popupFocused = false;
        }

        private void Popup_GotFocus(object sender, RoutedEventArgs e)
        {
            popupFocused = true;
        }

        private void UpdateOffsetIfRequired(TrackViewModel track, int offset, bool isOctave = false)
        {
            track.IgnoreSaveSettings = true;

            if (!isOctave)
            {
                if (offset != track.KeyOffset)
                    track.KeyOffset = offset;
            }
            else
            {
                if (offset != track.OctaveOffset)
                    track.OctaveOffset = offset;
            }

            track.IgnoreSaveSettings = false;
        }


        public void UpdateEnsembleMembers()
        {

            Dispatcher.Invoke(() =>
            {
                this.viewModel.UpdateEnsembleMembers();
            });
        }

        private void SaveSongSettings()
        {
            var notification = new SaveSongSettingsNotification();
            Messenger.Default.Send(notification);
        }
        #endregion

    }
}
