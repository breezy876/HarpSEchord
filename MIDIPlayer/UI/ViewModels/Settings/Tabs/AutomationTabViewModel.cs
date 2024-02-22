
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
    public class AutomationTabViewModel : ObservableViewModel
    {
        public AutomationTabViewModel() : base()
        {

        }

        public bool EnableInstrumentSwitching
        {
            get { return Common.Settings.AppSettings.AutomationSettings.EnableInstrumentSwitching; }
            set
            {
                Common.Settings.AppSettings.AutomationSettings.EnableInstrumentSwitching = value;
                RaisePropertyChanged();
            }
        }

        public bool AcceptReadyChecks
        {
            get { return Common.Settings.AppSettings.AutomationSettings.AcceptReadyChecks; }
            set
            {
                Common.Settings.AppSettings.AutomationSettings.AcceptReadyChecks = value;
                RaisePropertyChanged();
            }
        }
        public bool PlayOnStart
        {
            get { return Common.Settings.AppSettings.AutomationSettings.PlayOnStart; }
            set
            {
                Common.Settings.AppSettings.AutomationSettings.PlayOnStart = value;
                RaisePropertyChanged();
            }
        }

        public bool CloseOnFinish
        {
            get { return Common.Settings.AppSettings.AutomationSettings.CloseOnFinish; }
            set
            {
                Common.Settings.AppSettings.AutomationSettings.CloseOnFinish = value;
                RaisePropertyChanged();
            }
        }

        public bool StopOnClose
        {
            get { return Common.Settings.AppSettings.AutomationSettings.StopOnClose; }
            set
            {
                Common.Settings.AppSettings.AutomationSettings.StopOnClose = value;
                RaisePropertyChanged();
            }
        }

        public bool SendReadyCheckOnEquip
        {
            get { return Common.Settings.AppSettings.AutomationSettings.SendReadyCheckOnEquip; }
            set
            {
                Common.Settings.AppSettings.AutomationSettings.SendReadyCheckOnEquip = value;
                RaisePropertyChanged();
            }
        }

        public bool GuitarModeInstruments
        {
            get { return Common.Settings.AppSettings.AutomationSettings.GuitarMode == 2; }
            set
            {
                if (value)
                    Common.Settings.AppSettings.AutomationSettings.GuitarMode = 2;
                RaisePropertyChanged();
            }
        }

        public bool GuitarModeMIDI
        {
            get { return Common.Settings.AppSettings.AutomationSettings.GuitarMode == 1; }
            set
            {
                if (value)
                    Common.Settings.AppSettings.AutomationSettings.GuitarMode = 1;
                RaisePropertyChanged();
            }
        }

        public bool GuitarModeOff
        {
            get { return Common.Settings.AppSettings.AutomationSettings.GuitarMode == 0; }
            set
            {
                if (value)
                    Common.Settings.AppSettings.AutomationSettings.GuitarMode = 0;
                RaisePropertyChanged();
            }
        }




    }
}
