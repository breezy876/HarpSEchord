using Common;
using Common.Helpers;
using Common.Models.Ensemble;
using Common.Music;
using Hscm.UI.Notifications;
using Hscm.UI.Notifications.Playlist;
using Hscm.UI.Notifications.Tracks;
using Hscm.UI.ViewModels.Ensemble;
using Hscm.UI.ViewModels.Settings;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using static Common.FFXIV.Enums;
using Common.Models.FFXIV;
using Hscm.UI.Services;

namespace Hscm.UI.ViewModels
{
    public class TracksViewModel : ViewModelBase
    {
        private bool showMenuItems;
        private bool isVisible;

        private string caption;

        private string header;

        private bool showEnsembleOptions;
        private string ensembleHeader;

        private Dictionary<string, int> ensembleMembers;
        private Dictionary<string, int> assigned;
        private Dictionary<int, string> trackMemberCache;

        private bool spinnerVisible;
        private bool loggedIn;
        private bool showInstrumentsPopup;
        private bool anySelected;
        private bool showClone;
        private bool splitEnabled;
        private bool canSplit;
        private bool showRemove;
        private bool showRemoveAll;
        private bool showTrim;
        private bool showResetAll;
        private bool showReset;
        private bool showUnmuteAll;
        private bool showMuteAll;
        private bool showUnmute;
        private bool showMute;

        public TracksViewModel()
        {
            this.Tracks = new ObservableCollection<TrackViewModel>();
            this.Tracks.CollectionChanged += this.TracksChanged;

            this.ensembleMembers = new Dictionary<string, int>();
            this.assigned = new Dictionary<string, int>();

            trackMemberCache = new Dictionary<int, string>();
        }

        public ObservableCollection<TrackViewModel> Tracks { get; }

        public bool ShowInstrumentsPopup
        {
            get { return this.showInstrumentsPopup; }
            set
            {
                this.showInstrumentsPopup = value;
                RaisePropertyChanged();
            }
        }
        public bool ShowTrim
        {
            get
            {
                return showTrim;
            }
            set
            {
                showTrim = value;
                RaisePropertyChanged();
            }
        }

        public bool CanSplit
        {
            get
            {
                return canSplit;
            }
            set
            {
                canSplit = value;
                RaisePropertyChanged();
            }
        }

        public bool SpinnerVisible
        {
            get
            {
                return spinnerVisible;
            }
            set
            {
                spinnerVisible = value;
                RaisePropertyChanged();
            }
        }

        public string EnsembleHeader
        {
            get
            {
                return ensembleHeader;
            }
            set
            {
                ensembleHeader = value;
                RaisePropertyChanged();
            }
        }

        internal void HandleCharacterToggled(string characterName, bool isSelected)
        {
            if (isSelected)
                return;

            foreach (var track in Tracks)
            {
                if (track.SelectedMember.Equals(characterName))
                    track.ClearSelectedEnsembleMember();
            }
        }

        public bool LoggedIn
        {
            get
            {
                return loggedIn;
            }
            set
            {
                loggedIn = value;

                ShowEnsembleOptions = value;

                RaisePropertyChanged();
            }
        }

        public bool ShowRemove
        {
            get { return this.showRemove; }
            set
            {
                this.showRemove = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowMute
        {
            get { return this.showMute; }
            set
            {
                this.showMute = value;
                RaisePropertyChanged();
            }
        }
        public bool ShowUnmute
        {
            get { return this.showUnmute; }
            set
            {
                this.showUnmute = value;
                RaisePropertyChanged();
            }
        }


        public bool ShowMuteAll
        {
            get { return this.showMuteAll; }
            set
            {
                this.showMuteAll = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowUnmuteAll
        {
            get { return this.showUnmuteAll; }
            set
            {
                this.showUnmuteAll = value;
                RaisePropertyChanged();
            }
        }


        public bool ShowReset
        {
            get { return this.showReset; }
            set
            {
                this.showReset = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowResetAll
        {
            get { return this.showResetAll; }
            set
            {
                this.showResetAll = value;
                RaisePropertyChanged();
            }
        }


        public bool ShowRemoveAll
        {
            get { return this.showRemoveAll; }
            set
            {
                this.showRemoveAll = value;
                RaisePropertyChanged();
            }
        }


        public bool SplitEnabled
        {
            get
            {
                return splitEnabled;
            }
            set
            {
                splitEnabled = value;
                RaisePropertyChanged();
            }
        }

        internal void UpdateCharactersDropDown()
        {
            if (!loggedIn)
                return;

            foreach (string charName in this.ensembleMembers.Keys)
                if (this.Tracks.All(t => !string.IsNullOrEmpty(t.SelectedMember) && !t.SelectedMember.Equals(charName)))
                    UpdateCharacters(charName, false);
                else
                    UpdateCharacters(charName, true);
        }

        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowSettings
        {
            get
            {
                return Common.Settings.AppSettings.TrackSettings.ShowSettings;
            }
            set
            {
                Common.Settings.AppSettings.TrackSettings.ShowSettings = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowEnsembleOptions
        {
            get
            {
                return showEnsembleOptions;
            }
            set
            {
                showEnsembleOptions = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowMenuItems
        {
            get
            {
                return showMenuItems;
            }
            set
            {
                showMenuItems = value;
                RaisePropertyChanged();
            }
        }

        public bool AnySelected
        {
            get
            {
                return anySelected;
            }
            set
            {
                anySelected = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowClone
        {
            get
            {
                return showClone;
            }
            set
            {
                showClone = value;
                RaisePropertyChanged();
            }
        }

        public string Caption
        {
            get
            {
                return caption;
            }

            set
            {
                caption = value;
                RaisePropertyChanged();
            }
        }

        public string Header
        {
            get { return header; }
            set
            {
                header = value;
                RaisePropertyChanged();
            }
        }


        #region commands
        public RelayCommand SplitPercussionCommand { get { return new RelayCommand(ExecuteSplitPercussionCommand); } }

        public RelayCommand DuplicateCommand { get { return new RelayCommand(ExecuteDuplicateCommand); } }

        public RelayCommand MuteCommand { get { return new RelayCommand(ExecuteMuteCommand); } }


        public RelayCommand MuteAllCommand { get { return new RelayCommand(ExecuteMuteAllCommand); } }

        public RelayCommand UnmuteCommand { get { return new RelayCommand(ExecuteUnmuteCommand); } }


        public RelayCommand UnmuteAllCommand { get { return new RelayCommand(ExecuteUnmuteAllCommand); } }

        public RelayCommand ResetCommand { get { return new RelayCommand(ExecuteResetCommand); } }

        public RelayCommand ResetAllCommand { get { return new RelayCommand(ExecuteResetAllCommand); } }

        public RelayCommand RemoveCommand { get { return new RelayCommand(ExecuteRemoveCommand); } }

        public RelayCommand RemoveAllCommand { get { return new RelayCommand(ExecuteRemoveAllCommand); } }

        public RelayCommand TrimCommand { get { return new RelayCommand(ExecuteTrimCommand); } }

        public RelayCommand ToggleSettingsCommand { get { return new RelayCommand(ExecuteToggleSettingsCommand); } }

        public RelayCommand AutoPopulateCommand { get { return new RelayCommand(ExecuteAutoPopulateCommand); } }

        public RelayCommand<object> ShowCharactersPopupCommand { get { return new RelayCommand<object>(ExecuteShowCharactersPopupCommand); } }

        public RelayCommand ToggleInstrumentsCommand { get { return new RelayCommand(ExecuteToggleInstrumentsCommand); } }

        public RelayCommand CloseInstrumentsCommand { get { return new RelayCommand(ExecuteCloseInstrumentsCommand); } }

        private void ExecuteToggleInstrumentsCommand()
        {
            var msg = new ToggleInstrumentsNotification();
            Messenger.Default.Send(msg);
        }

        private void ExecuteCloseInstrumentsCommand()
        {
            var msg = new CloseInstrumentsNotification();
            Messenger.Default.Send(msg);
        }


        private void ExecuteShowCharactersPopupCommand(object obj)
        {
            var args = (MouseButtonEventArgs)obj;


            var msg = new ShowCharactersPopupNotification() { Parent = args.Source };
            Messenger.Default.Send(msg);
        }

        private void ExecuteToggleSettingsCommand()
        {
            var msg = new SaveSettingsNotification() { SaveAppSettings = true };
            Messenger.Default.Send(msg);
        }

        private void ExecuteTrimCommand()
        {
            var msg = new TrimTracksNotification();
            Messenger.Default.Send(msg);
        }

        public void ShowPopupForTrack(int trackIndex)
        {
            ShowInstrumentsPopup = true;
        }

        private void ExecuteRemoveCommand()
        {
            var trackIndexes = this.GetSelectedItems().Select(t => t.Index);

            var msg = new RemoveTracksNotification() { TrackIndexes = trackIndexes };
            Messenger.Default.Send(msg);
        }

        private void ExecuteRemoveAllCommand()
        {
            var msg = new RemoveAllTracksNotification();
            Messenger.Default.Send(msg);
        }

        private void ExecuteSplitPercussionCommand()
        {
            var msg = new SplitPercussionNotification() { Split = SplitEnabled };
            Messenger.Default.Send(msg);
        }

        private struct TrackInstrumentGrouping
        {
            public Instrument instrument { get; set; }
            public TrackViewModel track { get; set; }
        }

        private void ExecuteAutoPopulateCommand()
        {
            Autopopulate(false);
        }

        private void ExecuteDuplicateCommand()
        {
            var trackIndexes = this.GetSelectedItems().Select(t => t.Index);

            var msg = new DuplicateTracksNotification() { TrackIndexes = trackIndexes };
            Messenger.Default.Send(msg);
        }

        private void ExecuteMuteCommand()
        {
            foreach (var track in this.Tracks.Where(t => t.IsSelected))
                track.Muted = true;

            var msg = new TrackChangedNotification();
            Messenger.Default.Send(msg);
        }

        private void ExecuteUnmuteCommand()
        {
            foreach (var track in this.Tracks.Where(t => t.IsSelected))
                track.Muted = false;

            var msg = new TrackChangedNotification();
            Messenger.Default.Send(msg);
        }

        private void ExecuteMuteAllCommand()
        {
            foreach (var track in this.Tracks)
                track.Muted = true;

            var msg = new TrackChangedNotification();
            Messenger.Default.Send(msg);
        }

        private void ExecuteUnmuteAllCommand()
        {
            foreach (var track in this.Tracks)
                track.Muted = false;

            var msg = new TrackChangedNotification();
            Messenger.Default.Send(msg);
        }

        private void ExecuteResetCommand()
        {
            foreach (var track in this.Tracks.Where(t => t.IsSelected))
                track.Reset();

            var msg = new TrackChangedNotification();
            Messenger.Default.Send(msg);
        }

        private void ExecuteResetAllCommand()
        {
            ResetAll();

            var msg = new TrackChangedNotification();
            Messenger.Default.Send(msg);
        }
        #endregion


        #region public methods

        //private void PopulateFromInstrumentMap(InstrumentMap insMap, bool )
        //{

        //}


        public void Autopopulate(bool ignoreSave = false)
        {
            try
            {
                //generate from instrument map

                if (Common.Settings.AppSettings.TrackSettings.PopulateFromPlaylist)
                    return;

                InstrumentMap insMap = PerformanceHelpers.LoadInstrumentMap();

                string[] insList = PerformanceHelpers.LoadInstrumentMapFromList();

                IEnumerable<IGrouping<Instrument, TrackInstrumentGrouping>> trackGroups;

                //try loading from instrument map
                if (insMap != null)
                    trackGroups = GetTrackGroupsFromInstrumentMap(insMap);

                //try loading from instrument/track list
                else if (!insList.IsNullOrEmpty())
                    trackGroups = GetTrackGroupsFromInstrumentList(insList);

                else
                {
                    trackGroups = GetTrackGroups(t =>
                    {
                        Instrument ins = Instrument.None;

                        var insFromTitle = PerformanceHelpers.GetInstrumentIDByName(t.Title);
                        if (insFromTitle != Instrument.None)
                            ins = insFromTitle;

                        var insFromName = PerformanceHelpers.GetInstrumentIDByName(t.Instrument);
                        if (insFromName != Instrument.None)
                            ins = insFromName;

                        return new TrackInstrumentGrouping { track = t, instrument = ins };
                    });

                }
                EnumerateTracksAndAssignInstruments(trackGroups);

                if (ignoreSave)
                    return;

                var msg = new SaveSongSettingsNotification() { SaveAppSettings = true };
                Messenger.Default.Send(msg);
            }
            catch (Exception ex)
            {
                LogService.AppendLog("", "Error with autofilling tracks");
            }
        }

        public void ClearMemberCache()
        {
            trackMemberCache.Clear();
        }

        public void UpdateButtons()
        {
            ShowMute = false;
            ShowReset = false;
            ShowClone = false;

            ShowMuteAll = !AllAreMuted();
            ShowUnmuteAll = AnyAreMuted();
            ShowResetAll = AnyHaveChanged();
        }

        public void HandleTrackChanged()
        {
            ShowMuteAll = !AllAreMuted();
            ShowUnmuteAll = AnyAreMuted();
            ShowResetAll = AnyHaveChanged();

            ShowMute = !AnySelectedAreMuted();
            ShowUnmute = AnySelectedAreMuted();
            ShowReset = AnySelected && AnySelectedHaveChanged();
        }

        public void ChangeTrackInstrument(int trackIndex, string instrument)
        {
            this.Tracks[trackIndex].ChangeInstrument(instrument);
        }

        public void HandleTrackEnsembleMemberChanged(string member, int trackIndex)
        {
            if (!member.Equals("None") && !assigned.ContainsKey(member))
                assigned.Add(member, trackIndex);
        }

        public void UpdateEnsembleMembers()
        {
            if (!LoggedIn)
                return;

            var prevMembers = Tracks.Select(t => t.Model.EnsembleMember).ToArray();

            foreach (var track in this.Tracks)
            {
                track.ClearEnsembleMembers();

                foreach (var member in ensembleMembers)
                {
                    track.AddEnsembleMember(member.Key, member.Value);
                }

                track.UpdateMemberDisplay();

                if (track.Model.EnsembleMember.HasValue && track.Model.EnsembleMember != -1 && CharacterExists(track.Model.EnsembleMember.Value))
                    track.UpdateSelectedEnsembleMember();
                else
                {
                    track.ClearSelectedEnsembleMember();
                    track.Model.EnsembleMember = prevMembers[track.Index];
                }
            }

        }

        public void UpdateEnsembleMemberDisplay()
        {

            //if (!LoggedIn)
            //    return;

            var prevMembers = Tracks.Select(t => t.Model.EnsembleMember).ToArray();

            foreach (var track in this.Tracks)
            {
                track.UpdateMemberDisplay();

                if (track.Model.EnsembleMember.HasValue && track.Model.EnsembleMember != -1 && CharacterExists(track.Model.EnsembleMember.Value))
                    track.UpdateSelectedEnsembleMember();
                else
                {
                    track.ClearSelectedEnsembleMember();
                    track.Model.EnsembleMember = prevMembers[track.Index];
                }
            }
        }


        public void AddEnsembleMember(string charName, int index)
        {
            if (ensembleMembers.ContainsKey(charName))
                return;

            foreach (var track in this.Tracks)
            {
                track.AddEnsembleMember(charName, index);

                //if (trackMemberCache.ContainsKey(track.Index) && Common.Settings.AppSettings.EnsembleSettings.RememberPrevious)
                //    track.UpdateSelectedEnsembleMember(trackMemberCache[track.Index]);

                //track.UpdateSelectedEnsembleMember();

                //track.UpdateSelectedEnsembleMember(charName);

                UpdateTrackMemberFromCache(track, charName, true);
            }

            this.ensembleMembers.Add(charName, index);

            //if (ensembleMembers.Count() > 1)
            //    this.ShowEnsembleOptions = true;
        }

        public void RemoveEnsembleMember(string charName, bool ignoreSave = false)
        {

            if (!ensembleMembers.ContainsKey(charName))
                return;

            foreach (var track in this.Tracks)
            {

                if (!string.IsNullOrEmpty(track.SelectedMember) && track.SelectedMember.Equals(charName))
                    UpdateTrackMemberFromCache(track, charName, false);

                track.RemoveEnsembleMember(charName);

                //track.UpdateSelectedEnsembleMember();
            }

            this.ensembleMembers.Remove(charName);
        }

        public void HandlePopulateFromPlaylistChanged()
        {
            foreach (var track in Tracks)
                track.HandlePopulateFromPlaylistChanged();
        }

        public void Update(MidiSequence midiSequence)
        {
            trackMemberCache.Clear();

            Tracks.Clear();

            int index = 0;

            foreach (var track in midiSequence.Tracks)
            {
                var trackVm = new TrackViewModel(track.Value);
         
                if (trackVm.Cloned)
                    UpdateClonedTitle(trackVm);
                this.Tracks.Add(trackVm);
                index++;
            }

            UpdateButtons();
        }


        public void ResetAll()
        {
            foreach (var track in this.Tracks)
                track.Reset(true);
        }

        public void Clear()
        {
            this.Tracks.Clear();
        }

        public bool AnySelectedAreMuted() => this.Tracks.Where(t => t.IsSelected).Any(t => t.Muted);

        public bool AnySelectedHaveChanged() => this.Tracks.Where(t => t.IsSelected).Any(t => t.HasChanged);

        public bool AnyAreMuted() => this.Tracks.Any(t => t.Muted);

        public bool AllAreMuted() => this.Tracks.All(t => t.Muted);

        public bool AnyHaveChanged() => this.Tracks.Any(t => t.HasChanged);

        #endregion


        #region private methods
        private IEnumerable<IGrouping<Instrument, TrackInstrumentGrouping>> GetTrackGroups(Func<TrackViewModel, TrackInstrumentGrouping> selector)
        {
            var trackGroups = this.Tracks
                .Select(t => selector(t))
                .Where(t => t.instrument != Instrument.None)
                .GroupBy(t => t.instrument).ToArray();

            return trackGroups;
        }

        private IEnumerable<IGrouping<Instrument, TrackInstrumentGrouping>> GetTrackGroupsFromInstrumentMap(InstrumentMap insMap)
        {
            var trackGroups = GetTrackGroups(t =>
            {
                Instrument ins = Instrument.None;
                var insFromMidi = PerformanceHelpers.GetInstrumentFromMidi(insMap, t.Instrument);
                var insFromTitle = PerformanceHelpers.GetInstrumentFromMidi(insMap, t.Title);
                if (insFromTitle != Instrument.None)
                    ins = insFromTitle;
                if (insFromMidi != Instrument.None)
                    ins = insFromMidi;
                return new TrackInstrumentGrouping { track = t, instrument = ins };
            });

            return trackGroups;
        }


        private IEnumerable<IGrouping<Instrument, TrackInstrumentGrouping>> GetTrackGroupsFromInstrumentList(string[] insList)
        {
            for (int i = 0; i < insList.Length; i++)
            {
                string ins = insList[i];
                var track = this.Tracks[i];
                track.SelectedInstrument = ins;
            }

            var trackGroups = GetTrackGroups(t =>
            {
                Instrument ins = Instrument.None;

                var insFromName = PerformanceHelpers.GetInstrumentIDByName(t.SelectedInstrument);
                if (insFromName != Instrument.None)
                    ins = insFromName;

                return new TrackInstrumentGrouping { track = t, instrument = ins };
            });

            return trackGroups;
        }


        private void EnumerateTracksAndAssignInstruments(IEnumerable<IGrouping<Instrument, TrackInstrumentGrouping>> trackGroups)
        {
            var members = this.ensembleMembers.Keys.AsEnumerable();

            var memberEnumerator = members.GetEnumerator();

            foreach (var track in Tracks)
            {
                track.SelectedMember = "None";
                track.SelectedInstrument = "None";
            }

            foreach (var trackGrp in trackGroups)
            {
                bool hasNext = memberEnumerator.MoveNext();
                if (!hasNext)
                    break;

                string nextMember = memberEnumerator.Current;

                foreach (var track in trackGrp)
                {
                    track.track.SelectedMember = nextMember;
                    track.track.SelectedInstrument = track.instrument.ToString();
                }
            }
        }

        private bool CharacterExists(int index) => ensembleMembers.Any(m => m.Value == index);

        private void UpdateTrackMemberFromCache(TrackViewModel track, string charName, bool loggedIn)
        {
            if (loggedIn)
            {
                if (trackMemberCache.ContainsKey(track.Index) && trackMemberCache[track.Index].Equals(charName))
                {
                    track.IgnoreSaveSettings = true;
                    track.SelectedMember = charName;
                    trackMemberCache.Remove(track.Index);
                }
                else
                    track.UpdateSelectedEnsembleMember();
            }
            else
            {
                if (track.SelectedMember.Equals(charName) && !trackMemberCache.ContainsKey(track.Index))
                {
                    track.ClearSelectedEnsembleMember(true);
                    trackMemberCache.Add(track.Index, charName);
                }
            }

        }

        private void UpdateClonedTitle(TrackViewModel track)
        {

            var clones = Tracks.Where(t => t.Cloned && t.Model.ParentIndex.HasValue && t.Model.ParentIndex.Value == track.Model.ParentIndex.Value);
            int total = clones.Count() + 1;
            var origTrack = Tracks[track.Model.ParentIndex.Value];

            track.Title = $"{origTrack.Title} #{total} [{track.Model.ParentIndex.Value+1}]";
        }

        private void UpdateCharacters(string charName, bool selected)
        {
            var msg = new UpdateCharactersNotification() {  CharacterName = charName, IsSelected = selected};
            Messenger.Default.Send(msg);
        }

        private string GetMember(int index)
        {
            var member = this.ensembleMembers.FirstOrDefault(m => m.Value == index);
            return member.Key;
        }

        private bool ChildTracksSelected() => GetSelectedItems().Any(i => i.Model.ParentIndex.HasValue);

        private void TracksChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var items = e.NewItems.Cast<TrackViewModel>();

                foreach (var item in items)
                {
                    item.AddPropertyChangedHandler(nameof(item.IsSelected), IsSelected_PropertyChanged);
                }


                this.Header = $"# ({this.Tracks.Count})";

            }

            var selItems = GetSelectedItems();


            var totalSelected = selItems.Count();

            if (totalSelected > 0)
                AnySelected = true;
            else
                AnySelected = false;

            ShowClone = AnySelected && !ChildTracksSelected();

            SplitEnabled = Tracks.Any(t => t.Model.PercussionNote.HasValue && t.Model.ParentIndex.HasValue);

            CanSplit = Tracks.Any(t => t.IsPercussion);
   
            ShowRemoveAll = Tracks.Any(t => !t.Model.PercussionNote.HasValue && t.Model.ParentIndex.HasValue);

            ShowRemove = AnySelected && selItems.Any(i => i.Model.Cloned);

            ShowTrim = SplitEnabled || ShowRemoveAll;

            HandleTrackChanged();
        }

        private void IsSelected_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var selItems = GetSelectedItems();

            var totalSelected = selItems.Count();

            if (totalSelected > 0)
                AnySelected = true;
            else
                AnySelected = false;

            ShowClone = AnySelected && !ChildTracksSelected();
            ShowRemove = AnySelected && selItems.Any(i => i.Model.Cloned);

            HandleTrackChanged();
        }

        private IEnumerable<TrackViewModel> GetSelectedItems()
        {
            var tracks = this.Tracks.Where(f => f.IsSelected);
            return tracks.ToArray();
        }

        #endregion

    }
}
