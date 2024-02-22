
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
    public class GeneralTabViewModel : ObservableViewModel
    {
        public GeneralTabViewModel() : base()
        {

        }


        public bool Topmost
        {
            get { return Common.Settings.AppSettings.GeneralSettings.Topmost; }
            set
            {
                Common.Settings.AppSettings.GeneralSettings.Topmost = value;
                RaisePropertyChanged();
            }
        }

        public bool TestMode
        {
            get { return Common.Settings.AppSettings.GeneralSettings.TestMode; }
            set
            {
                Common.Settings.AppSettings.GeneralSettings.TestMode = value;
                RaisePropertyChanged();
            }
        }

        public bool UseMidiCache
        {
            get { return Common.Settings.AppSettings.GeneralSettings.UseMidiCache; }
            set
            {
                Common.Settings.AppSettings.GeneralSettings.UseMidiCache = value;
                RaisePropertyChanged();
            }
        }
        public bool EnableTranspose
        {
            get { return Common.Settings.AppSettings.GeneralSettings.EnableTranspose; }
            set
            {
                Common.Settings.AppSettings.GeneralSettings.EnableTranspose = value;
                RaisePropertyChanged();
            }
        }

        public bool EnableTrim
        {
            get { return Common.Settings.AppSettings.GeneralSettings.EnableTrim; }
            set
            {
                Common.Settings.AppSettings.GeneralSettings.EnableTrim = value;
                RaisePropertyChanged();
            }
        }

        public bool EnableTrimFromTracks
        {
            get { return Common.Settings.AppSettings.GeneralSettings.EnableTrimFromTracks; }
            set
            {
                Common.Settings.AppSettings.GeneralSettings.EnableTrimFromTracks = value;
                RaisePropertyChanged();
            }
        }

        public bool InputModeDirect
        {
            get { return Common.Settings.AppSettings.GeneralSettings.InputMode == 0; }
            set
            {
                if (value)
                    Common.Settings.AppSettings.GeneralSettings.InputMode = 0;
                RaisePropertyChanged();
            }
        }

        public bool InputModeKey
        {
            get { return Common.Settings.AppSettings.GeneralSettings.InputMode == 1; }
            set
            {
                if (value)
                    Common.Settings.AppSettings.GeneralSettings.InputMode = 1;
                RaisePropertyChanged();
            }
        }

        public int KeyDelta
        {
            get { return Common.Settings.AppSettings.GeneralSettings.KeyDelta; }
            set
            {
                Common.Settings.AppSettings.GeneralSettings.KeyDelta = value;
                RaisePropertyChanged();
            }
        }


    }
}
