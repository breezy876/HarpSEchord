
using Hscm.UI.Notifications;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hscm.UI.ViewModels.Settings
{
    public class TracksTabViewModel : ObservableViewModel
    {
        public TracksTabViewModel() : base()
        {

        }

        public bool PopulateFromPlaylist
        {
            get { return Common.Settings.AppSettings.TrackSettings.PopulateFromPlaylist; }
            set
            {
                Common.Settings.AppSettings.TrackSettings.PopulateFromPlaylist = value;
                RaisePropertyChanged();
            }
        }

        public bool PopulateFromMidi
        {
            get { return Common.Settings.AppSettings.TrackSettings.PopulateFromMidi; }
            set
            {
                Common.Settings.AppSettings.TrackSettings.PopulateFromMidi = value;
                RaisePropertyChanged();
            }
        }

        public bool TransposeFromTitle
        {
            get { return Common.Settings.AppSettings.TrackSettings.TransposeFromTitle; }
            set
            {
                Common.Settings.AppSettings.TrackSettings.TransposeFromTitle = value;
                RaisePropertyChanged();
            }
        }

        public bool TransposeInstruments
        {
            get { return Common.Settings.AppSettings.TrackSettings.TransposeInstruments; }
            set
            {
                Common.Settings.AppSettings.TrackSettings.TransposeInstruments = value;
                RaisePropertyChanged();
            }
        }

        public bool TransposeDrums
        {
            get { return Common.Settings.AppSettings.TrackSettings.TransposeDrums; }
            set
            {
                Common.Settings.AppSettings.TrackSettings.TransposeDrums = value;
                RaisePropertyChanged();
            }
        }

        public bool EnableChooseDrums
        {
            get { return Common.Settings.AppSettings.TrackSettings.EnableChooseDrums; }
            set
            {
                Common.Settings.AppSettings.TrackSettings.EnableChooseDrums = value;
                RaisePropertyChanged();
            }
        }



    }
}
