using Common;
using Common.Helpers;
using Common.Messaging.Settings;
using Common.Music;
using Hscm.UI.Notifications;
using Hscm.UI.Notifications.Playlist;
using Hscm.UI.Notifications.Settings;
using Hscm.UI.Notifications.Tracks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace Hscm.UI.ViewModels.Settings
{
    public class TrackViewModel : ObservableViewModel<Track>
    {
        private string autofilledMember;
        private string selectedMember;
        private string selectedInstrumentImage;
        private string insName;
        private bool showInstrument;
        private bool loggedIn;
        private bool isPercussion;
        private bool populateFromPlaylist;
        private bool showEnsembleOptions;
        private bool isSelected;

        private Dictionary<string, int> ensembleMembers;

        private bool reduceMaxNoteItemsEnabled;
        private bool chordSettingsEnabled;
        private string tooltipText;
        private bool showMemberLabel;
        private bool showMembers;

        public TrackViewModel(Track model) : base(model)
        {
            this.Initialize();
        }


        #region properties
        public ObservableCollection<string> EnsembleMembers { get; private set; }

        public ObservableCollection<int> ReduceMaxNoteItems { get; set; }

        public bool IgnoreTranspose { get; set; }

        public int No => Index + 1;

        public bool IgnoreSaveSettings { get; set; }


        public int Index
        {
            get { return Model.Index; }
        }

        public string ToolTipText
        {
            get { return this.tooltipText; }
            set
            {
                this.tooltipText = value;
                RaisePropertyChanged();
            }
        }

        public bool ChordSettingsEnabled
        {
            get { return this.chordSettingsEnabled; }
            set
            {
                this.chordSettingsEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowMemberLabel
        {
            get { return this.showMemberLabel; }
            set
            {
                this.showMemberLabel = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowMembers
        {
            get { return this.showMembers; }
            set
            {
                this.showMembers = value;
                RaisePropertyChanged();
            }
        }

        public bool IsPercussion
        {
            get { return isPercussion; }
            set
            {
                isPercussion = value;
                RaisePropertyChanged();
            }
        }

        public bool Enabled
        {
            get { return Model.Enabled; }
            set
            {
                Model.Enabled = value;
                RaisePropertyChanged();
            }
        }

        public bool IsSplit
        {
            get { return Model.IsSplit; }
            set
            {
                Model.IsSplit = value;
                RaisePropertyChanged();
            }
        }

        public bool Cloned
        {
            get { return Model.Cloned ; }
            set
            {
                Model.Cloned = value;
                RaisePropertyChanged();
            }
        }

        public bool ReduceMaxNotesEnabled
        {
            get { return this.reduceMaxNoteItemsEnabled; }
            set
            {
                this.reduceMaxNoteItemsEnabled = value;
                RaisePropertyChanged();
            }
        }


        public bool HoldLongNotes
        {
            get
            {
                return Model.HoldLongNotes;
            }
            set
            {
                Model.HoldLongNotes = value;
                RaisePropertyChanged();
            }
        }


        public bool HighestOnly
        {
            get { return Model.HighestOnly; }
            set
            {
                if (value)
                    PlayAll = false;

                Model.HighestOnly = value;

    
                this.ReduceMaxNotesEnabled = !value;
                RaisePropertyChanged();
            }
        }

        public bool PlayAll
        {
            get { return Model.PlayAll; }
            set
            {
                if (value)
                    HighestOnly = false;

                Model.PlayAll = value;

                this.ReduceMaxNotesEnabled = !value;
                RaisePropertyChanged();
            }
        }

        public bool ShowEnsembleOptions
        {
            get { return showEnsembleOptions; }
            set
            {
                this.showEnsembleOptions = value;
                RaisePropertyChanged();
            }
        }


        public bool ShowSettings => Enabled;

        public bool LoggedIn
        {
            get { return loggedIn; }
            set
            {
                this.loggedIn = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowInstrument
        {
            get { return showInstrument; }
            set
            {
                this.showInstrument = value;
                RaisePropertyChanged();
            }
        }


        public int ReduceMaxNotes
        {
            get
            {
                return Model.ReduceMaxNotes;
            }
            set
            {
                Model.ReduceMaxNotes = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedInstrument
        {
            get
            { 
                return Common.Settings.AppSettings.TrackSettings.PopulateFromPlaylist ? Model.EnsembleInstrument : Model.AutofilledInstrument;
            }
            set
            {
                if (Common.Settings.AppSettings.TrackSettings.PopulateFromPlaylist)
                    Model.EnsembleInstrument = value;
                else
                    Model.AutofilledInstrument = value;

                SelectedInstrumentImage = GetInstrumentImage();

                SelectedInstrumentName = value;

                RaisePropertyChanged();
            }
        }

        public string SelectedInstrumentName
        {
            get
            {
                return insName;
            }
            set
            {
                insName = PerformanceHelpers.GetInstrumentDisplayName(value);

                RaisePropertyChanged();
            }
        }

        public string SelectedInstrumentImage
        {
            get
            {
                return selectedInstrumentImage;
            }
            set
            {
                selectedInstrumentImage = value;

                RaisePropertyChanged();
            }
        }

        public bool HasChanged => HasTrackChanged();



        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                RaisePropertyChanged();
            }
        }

        public bool PopulateFromPlaylist
        {
            get
            {
                return populateFromPlaylist;
            }
            set
            {
                populateFromPlaylist = value;
                RaisePropertyChanged();
            }
        }

        public bool Muted
        {
            get
            {
                return Model.Muted;
            }
            set
            {
                Model.Muted = value;
                RaisePropertyChanged();
            }
        }

        public string Instrument
        {
            get
            {
                return Model.Instrument;
            }
            set
            {
                Model.Instrument = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedMember
        {
            get
            {
                //return this.ensembleMembers.IsNullOrEmpty() || !ensembleMembers.ContainsKey(Model.EnsembleMember.Value)
                //    || !Model.EnsembleMember.HasValue || Model.EnsembleMember.Value == -1 ? "None" : this.ensembleMembers[Model.EnsembleMember.Value];

                return Common.Settings.AppSettings.TrackSettings.PopulateFromPlaylist ? selectedMember : autofilledMember;
            }
            set
            {
                if (Common.Settings.AppSettings.TrackSettings.PopulateFromPlaylist)
                    selectedMember = value;
                else
                    autofilledMember = value;

                if (Common.Settings.AppSettings.TrackSettings.PopulateFromPlaylist)
                    Model.EnsembleMember = GetEnsembleMember(value);
                else
                    Model.AutofilledMember = GetEnsembleMember(value);

                ShowInstrument = Enabled && (Common.Settings.AppSettings.TrackSettings.PopulateFromPlaylist  ? Model.EnsembleMember : Model.AutofilledMember ) != -1; 

                RaisePropertyChanged();
            }
        }

        public int TimeOffset
        {
            get { return Model.TimeOffset; }
            set
            {
                Model.TimeOffset = value;

                RaisePropertyChanged();
            }
        }

        public int KeyOffset
        {
            get { return Model.KeyOffset; }
            set
            {
                Model.KeyOffset = value;

                RaisePropertyChanged();
            }
        }

        public int OctaveOffset
        {
            get
            {
                return Model.OctaveOffset;
            }
            set
            {
                Model.OctaveOffset = value;

                RaisePropertyChanged();
            }
        }

        public string Title
        {
            get
            {
                return Model.Title;
            }
            set
            {
                Model.Title = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region public methods
        public void HandlePopulateFromPlaylistChanged()
        {
            PopulateFromPlaylist = Common.Settings.AppSettings.TrackSettings.PopulateFromPlaylist;
            ShowEnsembleOptions = Enabled && PopulateFromPlaylist;
        }

        public void Update(Track model)
        {
            Model = model;
            Initialize();
        }

        public void ChangeInstrument(string instrument)
        {
            this.SelectedInstrument = instrument;

            if (IgnoreSaveSettings)
                return;

            ApplyTrackSettingsChange();

        }

        public void Reset(bool all = false)
        {
            PlayAll = true;
            HighestOnly = false;
            HoldLongNotes = true;
            OctaveOffset = 0;
            KeyOffset = 0;
            TimeOffset = 0;
            Muted = false;
            ReduceMaxNotes = 2;

            SelectedMember = "None";
            SelectedInstrument = "Harp";

            ReduceMaxNotesEnabled = false;

            if (all) 
                return;

            var changedMsg = new TrackChangedNotification();
            Messenger.Default.Send(changedMsg);
        }

        public void ClearEnsembleMembers()
        {
            this.EnsembleMembers.Clear();

            this.EnsembleMembers.Add("None");

            this.ensembleMembers.Clear();

            RaisePropertyChanged(nameof(this.EnsembleMembers));
        }

        public void ResetEnsembleMembers()
        {

            var memberDic =
               this.ensembleMembers.Where(m => m.Key != "None" && EnsembleMembers.Contains(m.Key)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            this.ensembleMembers = new Dictionary<string, int>() { {  "None", -1 } };

            foreach (var member in memberDic.OrderBy(m => m.Value))
                this.ensembleMembers.Add(member.Key, member.Value);

            this.EnsembleMembers.Clear();

            foreach (var member in ensembleMembers.Select(m => m.Key))
                EnsembleMembers.Add(member);

        }

        public void AddEnsembleMember(string charName, int index)
        {
            this.IgnoreSaveSettings = true;

            this.EnsembleMembers.Add(charName);
            this.ensembleMembers.Add(charName, index);

            this.ResetEnsembleMembers();

            //if (Model.PrevSelectedMember.HasValue)
            //{
            //    var member = GetEnsembleMember(Model.PrevSelectedMember.Value);

            //    if (!member.Equals(charName))
            //        return;

            //    this.SelectedMember = member;
            //    Model.PrevSelectedMember = null;
            //}

            RaisePropertyChanged(nameof(this.EnsembleMembers));
            //RaisePropertyChanged(nameof(this.SelectedMember));

            this.IgnoreSaveSettings = false;
        }

        public void RemoveEnsembleMember(string charName)
        {
            this.IgnoreSaveSettings = true;

            this.EnsembleMembers.Remove(charName);

            this.ResetEnsembleMembers();

            RaisePropertyChanged(nameof(this.EnsembleMembers));
            RaisePropertyChanged(nameof(this.SelectedMember));

            this.IgnoreSaveSettings = false;
        }

        public void ClearSelectedEnsembleMember(bool ignoreSave = false)
        {
            if (ignoreSave)
                IgnoreSaveSettings = true;

            this.SelectedMember = "None";

            IgnoreSaveSettings = false;
        }

        public void UpdateSelectedEnsembleMember(string charName = null)
        {
            this.IgnoreSaveSettings = true;

            if (charName.IsNullOrEmpty())
                this.SelectedMember = GetEnsembleMember();
            else
                this.SelectedMember = charName;
            
            this.IgnoreSaveSettings = false;

            //var changedMsg = new TrackChangedNotification();
            //Messenger.Default.Send(changedMsg);
        }

        #endregion

        #region commands
        public RelayCommand<object> HoldLongNotesCheckedCommand { get { return new RelayCommand<object>(ExecuteHoldLongNotesCheckedCommand); } }


        public RelayCommand<object> HoldLongNotesUncheckedCommand { get { return new RelayCommand<object>(ExecuteHoldLongNotesUncheckedCommand); } }


        public RelayCommand<object> PlayAllUncheckedCommand { get { return new RelayCommand<object>(ExecutePlayAllUncheckedCommand); } }


        public RelayCommand<object> PlayAllCheckedCommand { get { return new RelayCommand<object>(ExecutePlayAllCheckedCommand); } }


        public RelayCommand<object> HighestOnlyUncheckedCommand { get { return new RelayCommand<object>(ExecuteHighestOnlyUncheckedCommand); } }


        public RelayCommand<object> HighestOnlyCheckedCommand { get { return new RelayCommand<object>(ExecuteHighestOnlyCheckedCommand); } }

        public RelayCommand<object> ReduceMaxNoteItemsChangedCommand { get { return new RelayCommand<object>(ExecuteReduceMaxNoteItemsChangedCommand); } }

        public RelayCommand<object> TimeOffsetChangedCommand { get { return new RelayCommand<object>(ExecuteTimeOffsetChangedCommand); } }

        public RelayCommand<object> KeyOffsetChangedCommand { get { return new RelayCommand<object>(ExecuteKeyOffsetChangedCommand); } }

        public RelayCommand<object> OctaveOffsetChangedCommand { get { return new RelayCommand<object>(ExecuteOctaveOffsetChangedCommand); } }

        public RelayCommand<object> EnsembleMemberChangedCommand { get { return new RelayCommand<object>(ExecuteEnsembleMemberChangedCommand); } }

        public RelayCommand<object> InstrumentSelectedCommand { get { return new RelayCommand<object>(ExecuteInstrumentSelectedCommand); } }

        public RelayCommand<object> ClearEnsembleMemberCommand { get { return new RelayCommand<object>(ExecuteClearEnsembleMemberCommand); } }


        public RelayCommand<object> MaxNotesChangedCommand { get { return new RelayCommand<object>(ExecuteMaxNotesChangedCommand); } }


        private void ExecuteMaxNotesChangedCommand(object args)
        {

            var e = (RoutedPropertyChangedEventArgs<decimal>)args;

            ReduceMaxNotes = (int)e.NewValue;

            if (IgnoreSaveSettings)
                return;

            ApplyTrackSettingsChange();

        }

        private void ExecuteClearEnsembleMemberCommand(object obj)
        {
            this.SelectedMember = "None";

            ApplyTrackSettingsChange();
        }

        private void ExecuteInstrumentSelectedCommand(object obj)
        {
            var args = (MouseButtonEventArgs)obj;

            var msg = new ShowInstrumentsPopupNotification() { TrackIndex = Index, Control = args.Source };
            Messenger.Default.Send(msg);
        }


        private void ExecuteHoldLongNotesUncheckedCommand(object obj)
        {
            HoldLongNotes = false;

            if (IgnoreSaveSettings)
                return;

            ApplyTrackSettingsChange();
        }

        private void ExecuteHoldLongNotesCheckedCommand(object args)
        {
            HoldLongNotes = true;

            if (IgnoreSaveSettings)
                return;

            ApplyTrackSettingsChange();
        }


        private void ExecutePlayAllUncheckedCommand(object obj)
        {
            PlayAll = false;

            if (IgnoreSaveSettings)
                return;

            ApplyTrackSettingsChange();
        }

        private void ExecutePlayAllCheckedCommand(object args)
        {
            PlayAll = true;

            if (IgnoreSaveSettings)
                return;

            ApplyTrackSettingsChange();
        }

        private void ExecuteHighestOnlyUncheckedCommand(object obj)
        {
            HighestOnly = false;

            if (IgnoreSaveSettings)
                return;

            ApplyTrackSettingsChange();
        }

        private void ExecuteHighestOnlyCheckedCommand(object args)
        {
            HighestOnly = true;

            if (IgnoreSaveSettings)
                return;

            ApplyTrackSettingsChange();
        }

        private void ExecuteReduceMaxNoteItemsChangedCommand(object args)
        {

            SelectionChangedEventArgs e = (SelectionChangedEventArgs)args;

            if (e.AddedItems.Count == 0)
                return;

            this.ReduceMaxNotes = (int)e.AddedItems[0];

            if (IgnoreSaveSettings)
                return;

            ApplyTrackSettingsChange();
        }

        private void ExecuteEnsembleMemberChangedCommand(object args)
        {
            SelectionChangedEventArgs e = (SelectionChangedEventArgs)args;

            if (e.AddedItems.Count == 0)
                return;

            this.SelectedMember = (string)e.AddedItems[0];

            if (IgnoreSaveSettings)
                return;

            var msg = new TrackEnsembleMemberChangedNotification() { Member = SelectedMember, TrackIndex = Index };
            Messenger.Default.Send(msg);
        }

        private void ExecuteTimeOffsetChangedCommand(object args)
        {
            if (IgnoreSaveSettings)
                return;

            var e = (RoutedPropertyChangedEventArgs<decimal>)args;

            ApplyTrackSettingsChange();
        }

        private void ExecuteOctaveOffsetChangedCommand(object args)
        {

            if (IgnoreSaveSettings)
                return;

            var e = (RoutedPropertyChangedEventArgs<decimal>)args;


            ApplyTrackSettingsChange((int)e.NewValue);
        }

        private void ExecuteKeyOffsetChangedCommand(object args)
        {

            if (IgnoreSaveSettings)
                return;

            var e = (RoutedPropertyChangedEventArgs<decimal>)args;

            ApplyTrackSettingsChange((int)e.NewValue);
        }
        #endregion

        #region private methods
        private bool HasTrackChanged()
        {
            return

                !SelectedInstrument.Equals("Harp") ||
                !string.IsNullOrEmpty(SelectedMember) && !SelectedMember.Equals("None") ||
                ReduceMaxNotes != 2 ||
                 !PlayAll ||
                  HighestOnly ||
                   OctaveOffset != 0 ||
                 KeyOffset != 0 ||
                TimeOffset != 0 ||
                !HoldLongNotes ||
                Muted;
        }

        private string GetSelectedInstrument()
        {
            if (!Common.Settings.AppSettings.TrackSettings.PopulateFromPlaylist)
                return Model.AutofilledInstrument;

            return string.IsNullOrEmpty(Model.EnsembleInstrument) || Model.EnsembleInstrument.Equals("None") ? "Harp" : Model.EnsembleInstrument;
        }

        private void Initialize()
        {
            ToolTipText = GetToolTipText();

            IgnoreSaveSettings = true;

            ChordSettingsEnabled = true;

            this.EnsembleMembers = new ObservableCollection<string>() { "None" };
            this.ensembleMembers = new Dictionary<string, int>();

            Cloned = Model.Cloned;
            Enabled = Model.Enabled;

            UpdateMemberDisplay();

            IsSplit = Model.IsSplit;
            IsPercussion = Model.IsPercussion || Model.PercussionNote.HasValue;

            RaisePropertyChanged(nameof(this.ReduceMaxNoteItems));

            ReduceMaxNotesEnabled = true;
            ReduceMaxNotes = Model.ReduceMaxNotes;
            PlayAll = Model.PlayAll;
            HighestOnly = Model.HighestOnly;
            HoldLongNotes = Model.HoldLongNotes;

            OctaveOffset = Model.OctaveOffset;

            KeyOffset = Model.KeyOffset;
            TimeOffset = Model.TimeOffset;

            Muted = Model.Muted;

            IgnoreSaveSettings = false;

        }

        public void UpdateMemberDisplay()
        {
            PopulateFromPlaylist = Common.Settings.AppSettings.TrackSettings.PopulateFromPlaylist;
            ShowMemberLabel = !PopulateFromPlaylist || EnsembleMembers.Where(e => !e.Equals("None")).Count() == 1;
            ShowMembers = PopulateFromPlaylist && EnsembleMembers.Where(e => !e.Equals("None")).Count() > 1;
            ShowEnsembleOptions = Enabled && !EnsembleMembers.IsNullOrEmpty();
            ShowInstrument = Enabled && (PopulateFromPlaylist ? Model.EnsembleMember : Model.AutofilledMember) != -1;
            SelectedInstrument = GetSelectedInstrument();
        }


        string GetInstrumentImage()
        {
            string ins = Common.Settings.AppSettings.TrackSettings.PopulateFromPlaylist ? Model.EnsembleInstrument : Model.AutofilledInstrument;

            if (ins.Equals("None")) 
                ins = "Harp";

            int id = (int)PerformanceHelpers.GetInstrumentFromName(ins);
            return $"../../Images/instruments/{id}.png";
        }

        string GetEnsembleMember()
        {
            if (Model.EnsembleMember == null || Model.EnsembleMember == -1)
                    return "None";

            var member = ensembleMembers.FirstOrDefault(m => m.Value == Model.EnsembleMember.Value);

            return member.Equals(default(KeyValuePair<string, int>)) ? "None" : member.Key;
        }

        private int? GetEnsembleMember(string member)
        {
            if (member.IsNullOrEmpty() || member.Equals("None"))
                return -1;

            if (this.ensembleMembers.IsNullOrEmpty())
                return -1;

            if (!this.ensembleMembers.Any(m => m.Key == member))
                return -1;

            return this.ensembleMembers.First(m => m.Key == member).Value;

        }

        private void ApplyTrackSettingsChange(int offset = 0)
        {
            var changedMsg = new TrackChangedNotification() { Offset = offset };
            Messenger.Default.Send(changedMsg);

        }

        private string GetToolTipText()
        {
            var sb = new StringBuilder();

            string type = Model.IsPercussion ? "Drums" : "Melody";

            sb.AppendLine($"Type: {type}");
            sb.AppendLine(Model.Range);
            sb.AppendLine($"MIDI Instrument: {Model.Instrument}");
            sb.AppendLine($"Total Notes: {Model.TotalNotes}");

            return sb.ToString();

        }
        #endregion


    }
}
