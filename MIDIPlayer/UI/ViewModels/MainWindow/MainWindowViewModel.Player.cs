using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Hscm.UI.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hscm.UI.ViewModels.MainWindow
{
    public partial class MainWindowViewModel
    {
        private long seekValue;
        private long sequenceLength;

        private string timeLeft;
        private string timeElapsed;
        private string progress;

        private bool seekSliderEnabled;
        private bool showSeekSlider;
        private bool showTimer;
        private string tempoText;

        public string Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                RaisePropertyChanged();
            }
        }

        public string TempoText
        {
            get { return tempoText; }
            set
            {
                tempoText = value;
                RaisePropertyChanged();
            }
        }

        public double Tempo
        {
            get { return selectedSequence == null ? 100 : selectedSequence.Tempo; }
            set
            {
                if (selectedSequence == null)
                    return;

                selectedSequence.Tempo = value;
                RaisePropertyChanged();
            }
        }

        public long SequenceLength
        {
            get { return sequenceLength; }
            set
            {
                sequenceLength = value;
                RaisePropertyChanged();
            }
        }

        public long SeekValue
        {
            get { return seekValue; }
            set
            {
                seekValue = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowSeekSlider
        {
            get { return showSeekSlider; }
            set
            {
                showSeekSlider = value;
                RaisePropertyChanged();
            }
        }
        public bool IsSeekSliderEnabled
        {
            get { return seekSliderEnabled; }
            set
            {
                seekSliderEnabled = value;
                RaisePropertyChanged();
            }
        }

        public string TimeElapsed
        {
            get { return timeElapsed; }
            set
            {
                timeElapsed = value;
                RaisePropertyChanged();
            }
        }

        public string TimeLeft
        {
            get { return timeLeft; }
            set
            {
                timeLeft = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowTimer
        {
            get { return showTimer; }
            set
            {
                showTimer = value;
                RaisePropertyChanged();
            }
        }

        public void UpdateInfo(long position, TimeSpan ts)
        {

        }

        #region commands
        public RelayCommand<object> SeekChangedCommand { get { return new RelayCommand<object>(ExecuteSeekChangedCommand); } }

        public RelayCommand<object> SeekMouseWheelCommand { get { return new RelayCommand<object>(ExecuteSeekMouseWheelCommand); } }

        public RelayCommand<object> SeekMouseEnterCommand { get { return new RelayCommand<object>(ExecuteSeekMouseEnterCommand); } }

        public RelayCommand<object> SeekMouseLeaveCommand { get { return new RelayCommand<object>(ExecuteSeekMouseLeaveCommand); } }
        public RelayCommand<object> SeekMouseLeftButtonDownCommand { get { return new RelayCommand<object>(ExecuteSeekMouseLeftButtonDownCommand); } }


        public RelayCommand<object> TempoChangedCommand { get { return new RelayCommand<object>(ExecuteTempoChangedCommand); } }

        public RelayCommand<object> TempoUpdatedCommand { get { return new RelayCommand<object>(ExecuteTempoUpdatedCommand); } }

        public RelayCommand<object> TempoMouseWheelCommand { get { return new RelayCommand<object>(ExecuteTempoMouseWheelCommand); } }

        public RelayCommand<object> TempoMouseEnterCommand { get { return new RelayCommand<object>(ExecuteTempoMouseEnterCommand); } }

        public RelayCommand<object> TempoKeyDownCommand { get { return new RelayCommand<object>(ExecuteTempoKeyDownCommand); } }

        public RelayCommand<object> TempoKeyUpCommand { get { return new RelayCommand<object>(ExecuteTempoKeyUpCommand); } }



        private void ExecuteTempoMouseEnterCommand(object obj)
        {
            var msg = new TempoMouseEnterNotification();
            Messenger.Default.Send(msg);
        }


        private void ExecuteTempoKeyDownCommand(object e)
        {
            var args = e as KeyEventArgs;

            //if (!Common.Settings.AppSettings.GeneralSettings.UseHotkeys)
            //    return;

            //var shiftHotkey = Common.Settings.AppSettings.HotKeys[HotKeyType.MouseWheelShift];
            //var revertHotkey = Common.Settings.AppSettings.HotKeys[HotKeyType.RevertTempo];
            //var resetHotkey = Common.Settings.AppSettings.HotKeys[HotKeyType.ResetTempo];

            //if (args.Key == Key.LeftShift)
            //{
            //    var msg = new TempoShiftNotification() { IsDown = true };
            //    Messenger.Default.Send(msg);
            //}

            //if (args.Key == revertHotkey.Value)
            //{
            //    var msg = new TempoRevertNotification() { IsDown = true };
            //    Messenger.Default.Send(msg);
            //}

            //if (args.Key == resetHotkey.Value)
            //{
            //    var msg = new TempoResetNotification();
            //    Messenger.Default.Send(msg);
            //}

        }

        private void ExecuteTempoKeyUpCommand(object e)
        {
            //var args = e as KeyEventArgs;

            ////if (!Common.Settings.AppSettings.GeneralSettings.UseHotkeys)
            ////    return;

            //var shiftHotkey = Common.Settings.AppSettings.HotKeys[HotKeyType.MouseWheelShift];
            //var revertHotkey = Common.Settings.AppSettings.HotKeys[HotKeyType.RevertTempo];

            //if (args.Key == shiftHotkey.Value)
            //{
            //    var msg = new TempoShiftNotification() { IsDown = false };
            //    Messenger.Default.Send(msg);
            //}

            //if (args.Key == revertHotkey.Value)
            //{
            //    var msg = new TempoRevertNotification() { IsDown = false };
            //    Messenger.Default.Send(msg);
            //}
        }

        private void ExecuteSeekMouseLeftButtonDownCommand(object e)
        {
            var msg = new SeekMouseLeftButtonDownNotification();
            Messenger.Default.Send(msg);
        }

        private void ExecuteTempoMouseWheelCommand(object e)
        {
            var args = e as MouseWheelEventArgs;

            var msg = new TempoMouseWheelNotification() { Delta = args.Delta };
            Messenger.Default.Send(msg);
        }

        private void ExecuteSeekMouseEnterCommand(object e)
        {

            var msg = new SeekMouseEnterNotification();
            Messenger.Default.Send(msg);
        }

        private void ExecuteSeekMouseLeaveCommand(object e)
        {

            var msg = new SeekMouseLeaveNotification();
            Messenger.Default.Send(msg);
        }


        private void ExecuteSeekMouseWheelCommand(object e)
        {
            var args = e as MouseWheelEventArgs;

            var msg = new SeekMouseWheelNotification() { Delta = args.Delta };
            Messenger.Default.Send(msg);
        }

        private void ExecuteSeekChangedCommand(object args)
        {
            if (ignoreSaveChanges)
            {
                this.ignoreSaveChanges = false;
                return;
            }

            var e = (RoutedPropertyChangedEventArgs<double>)args;

            var changeMsg = new SeekChangedNotification() { Value = e.NewValue };
            Messenger.Default.Send(changeMsg);
        }

        private void ExecuteTempoChangedCommand(object args)
        {
            if (ignoreSaveChanges)
            {
                this.ignoreSaveChanges = false;
                return;
            }

            var e = (RoutedPropertyChangedEventArgs<double>)args;

            var changeMsg = new SequenceTempoChangedNotification() { Tempo = e.NewValue };
            Messenger.Default.Send(changeMsg);
        }

        private void ExecuteTempoUpdatedCommand(object args)
        {
            var notification = new SequenceTempoUpdatedNotification();
            Messenger.Default.Send(notification);
        }


        internal void UpdateProgress(long position, long duration)
        {
            float prog = ((float)((float)position / (float)duration) * (float)100.0);
            Progress = $"{(int)prog}%";
        }
        #endregion 
    }
}
