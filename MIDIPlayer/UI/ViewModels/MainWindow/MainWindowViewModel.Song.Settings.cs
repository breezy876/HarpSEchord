
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Hscm.UI.ViewModels.MainWindow
{
    public partial class MainWindowViewModel : ObservableViewModel
    {

        private bool reduceMaxNoteItemsEnabled;

        public ObservableCollection<int> ReduceMaxNoteItems { get; set; }

        public ObservableCollection<string> Instruments { get; private set; }


        public string SelectedInstrument
        {
            get
            {
                return selectedSequence.Instrument;
            }
            set
            {
                selectedSequence.Instrument = value;
                RaisePropertyChanged();
            }
        }


        public bool HighestOnly
        {
            get { return SelectedSequence.HighestOnly; }
            set
            {
                this.SelectedSequence.HighestOnly = value;

                if (value)
                    this.PlayAll = false;

                this.ReduceMaxNotesEnabled = !value;
                RaisePropertyChanged();
            }
        }

        public int ReduceMaxNotes
        {
            get { return SelectedSequence.ReduceMaxNotes; }
            set
            {
                SelectedSequence.ReduceMaxNotes = value;
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

        public bool PlayAll
        {
            get { return SelectedSequence.PlayAll; }
            set
            {
                SelectedSequence.PlayAll = value;

                if (value)
                    this.HighestOnly = false;

                this.ReduceMaxNotesEnabled = !value;
                RaisePropertyChanged();
            }
        }


        public bool HoldLongNotes
        {
            get { return SelectedSequence.HoldLongNotes; }
            set
            {
                SelectedSequence.HoldLongNotes = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand<object> HoldLongNotesCommand { get { return new RelayCommand<object>(ExecuteHoldLongNotesCommand); } }

        public RelayCommand SequenceHighestOnlyCommand { get { return new RelayCommand(ExecuteSequenceHighestOnlyCommand); } }

        public RelayCommand PlayAllCommand { get { return new RelayCommand(ExecutePlayAllCommand); } }

        //public RelayCommand<object> ReduceTypeItemsChangedCommand { get { return new RelayCommand<object>(ExecuteReduceTypeItemsChangedCommand); } }

        public RelayCommand SaveSettingsCommand { get { return new RelayCommand(ExecuteSaveSettingsCommand); } }


        public RelayCommand<object> KeyOffsetChangedCommand { get { return new RelayCommand<object>(ExecuteKeyOffsetChangedCommand); } }

        public RelayCommand<object> OctaveOffsetChangedCommand { get { return new RelayCommand<object>(ExecuteOctaveOffsetChangedCommand); } }


        public RelayCommand<object> MaxNotesChangedCommand { get { return new RelayCommand<object>(ExecuteMaxNotesChangedCommand); } }



        private bool ignoreSaveChanges;
        private bool pluginFound;

        public void UpdateSettings(MidiSequence sequence)
        {
            this.ignoreSaveChanges = true;

            ReduceMaxNotes = sequence.ReduceMaxNotes;
            PlayAll = sequence.PlayAll;
            HighestOnly = sequence.HighestOnly;
            Tempo = sequence.Tempo;
            HoldLongNotes = sequence.HoldLongNotes;

            OctaveOffset = sequence.OctaveOffset;

            KeyOffset = sequence.KeyOffset;

            ignoreSaveChanges = false;
        }

 

        public void PopulateSettings()
        {
            this.Tempo = 100;
        }

        private void ExecuteMaxNotesChangedCommand(object args)
        {

            if (ignoreSaveChanges)
            {
                this.ignoreSaveChanges = false;
                return;
            }

            var e = (RoutedPropertyChangedEventArgs<decimal>)args;

            ReduceMaxNotes = (int)e.NewValue;

            var notification = new SequenceChordSettingsChangedNotification();
            Messenger.Default.Send(notification);

        }

        private void ExecutePlayAllCommand()
        {

           
            if (ignoreSaveChanges)
                return;

            var notification = new SequenceChordSettingsChangedNotification();
            Messenger.Default.Send(notification);
        }

        private void ExecuteSequenceHighestOnlyCommand()
        {

            if (ignoreSaveChanges)
                return;

            var notification = new SequenceChordSettingsChangedNotification();
            Messenger.Default.Send(notification);
        }


        private void ExecuteHoldLongNotesCommand(object obj)
        {
            if (ignoreSaveChanges)
                return;


            var notification = new SequenceChordSettingsChangedNotification();
            Messenger.Default.Send(notification);
        }

        private void ExecuteOctaveOffsetChangedCommand(object args)
        {
            //var saveMsg = new SaveSettingsNotification() { SaveSongSettings = true };
            //Messenger.Default.Send(saveMsg);

            if (ignoreSaveChanges)
            {
                this.ignoreSaveChanges = false;
                return;
            }

            var e = (RoutedPropertyChangedEventArgs<decimal>)args;

            var notification = new OctaveOffsetChangedNotification() { Offset = (int)e.NewValue };
            Messenger.Default.Send(notification);
        }

        private void ExecuteKeyOffsetChangedCommand(object args)
        {
            if (ignoreSaveChanges)
            {
                this.ignoreSaveChanges = false;
                return;
            }

            var e = (RoutedPropertyChangedEventArgs<decimal>)args;

            var notification = new KeyOffsetChangedNotification() { Offset = (int)e.NewValue };
            Messenger.Default.Send(notification);
        }

        private void ExecuteSaveSettingsCommand()
        {
            var notification = new SaveSettingsNotification() { SaveAppSettings = false, SaveSongSettings = true };
            Messenger.Default.Send(notification);
        }

        private void RaiseSettingsPropertyChangedEvents()
        {
            RaisePropertyChanged(nameof(this.HighestOnly));
            RaisePropertyChanged(nameof(this.ReduceMaxNotes));
            RaisePropertyChanged(nameof(this.ReduceMaxNoteItems));
            RaisePropertyChanged(nameof(this.ReduceMaxNotesEnabled));
            RaisePropertyChanged(nameof(this.PlayAll));
            RaisePropertyChanged(nameof(this.HoldLongNotes));
            RaisePropertyChanged(nameof(this.Tempo));
            RaisePropertyChanged(nameof(this.Instruments));
        }
    }
}
