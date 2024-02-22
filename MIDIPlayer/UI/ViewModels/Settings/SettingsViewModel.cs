
using Hscm.UI.Notifications;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hscm.UI.Notifications.Playlist;

namespace Hscm.UI.ViewModels.Settings
{
    public class SettingsViewModel : ObservableViewModel
    {
        private bool sequenceSelected;
        private bool pluginFound;

        public GeneralTabViewModel GeneralTab { get; private set; }
        public TracksTabViewModel TracksTab { get; private set; }

        public AutomationTabViewModel AutomationTab { get; private set; }

        public SettingsViewModel() : base()
        {
            this.GeneralTab = new GeneralTabViewModel();
            this.TracksTab = new TracksTabViewModel();
            AutomationTab = new AutomationTabViewModel();
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

        public bool SequenceSelected
        {
            get { return sequenceSelected; }
            set
            {
                sequenceSelected = true;
                RaisePropertyChanged();
            }
        }


        public RelayCommand SaveAppSettingsCommand { get { return new RelayCommand(ExecuteSaveAppSettingsCommand); } }

        public RelayCommand AutofillOptionChangedCommand { get { return new RelayCommand(ExecuteAutofillOptionChangedCommand); } }

        private void ExecuteSaveAppSettingsCommand()
        {
            var notification = new SaveAppSettingsNotification();
            Messenger.Default.Send(notification);
        }

        private void ExecuteAutofillOptionChangedCommand()
        {
            var notification = new AutofillOptionChangedNotification();
            Messenger.Default.Send(notification);
        }
    }
}
 