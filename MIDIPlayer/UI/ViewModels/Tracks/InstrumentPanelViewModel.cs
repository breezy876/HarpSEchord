using Common;
using Common.Helpers;
using Common.Music;
using Hscm.UI.Notifications;
using Hscm.UI.Notifications.Playlist;
using Hscm.UI.Notifications.Tracks;
using Hscm.UI.ViewModels.Settings;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using static Common.FFXIV.Enums;

namespace Hscm.UI.ViewModels
{
    public class InstrumentPanelViewModel : ViewModelBase
    {
        private string chosenInstrument;
        private int trackIndex;

        public InstrumentPanelViewModel()
        {

        }
        public int TrackIndex
        {
            get
            {
                return trackIndex;
            }
            set
            {
                trackIndex = value;
                RaisePropertyChanged();
            }
        }

        public string ChosenInstrument
        {
            get
            {
                return chosenInstrument;
            }
            set
            {
                chosenInstrument = value;
                RaisePropertyChanged();
            }
        }

        #region commands


        public RelayCommand<object> InstrumentSelectedCommand { get { return new RelayCommand<object>(ExecuteInstrumentSelectedCommand); } }

        public RelayCommand<object> InstrumentMouseEnterCommand { get { return new RelayCommand<object>(ExecuteInstrumentMouseEnterCommand); } }

        private void ExecuteInstrumentSelectedCommand(object obj)
        {
            var args = (MouseEventArgs)obj;

            Image srcImg = (Image)args.Source;

            string name = srcImg.Name.Replace("_", " ");

            var msg = new TrackInstrumentChosenNotification() { TrackIndex = TrackIndex, Instrument = name };
            Messenger.Default.Send(msg);
        }


        private void ExecuteInstrumentMouseEnterCommand(object obj)
        {
            var args = (MouseEventArgs)obj;

            Image srcImg = (Image)args.Source;

            string name = srcImg.Name.Replace("_", " ");

            ChosenInstrument = name;
  
        }

        #endregion


    }
}
