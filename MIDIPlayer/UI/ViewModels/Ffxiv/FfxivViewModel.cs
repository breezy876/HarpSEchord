
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Hscm.Models.Ffxiv;
using Hscm.UI.Notifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI.ViewModels.Ffxiv
{
    public class FfxivViewModel : ObservableViewModel
    {
        private string headerText;

        public ObservableCollection<FfxivClientViewModel> Clients { get; private set; }

        public FfxivViewModel() : base()
        {
            Clients = new ObservableCollection<FfxivClientViewModel>();
            HeaderText = GetHeaderText();
        }

        public string HeaderText
        {
            get
            {
                return headerText;
            }
            set
            {
                headerText = value;
                RaisePropertyChanged();
            }
        }

        public string CurrentPluginPath
        {
            get
            {
                return Common.Settings.AppSettings.HSCPluginConfigPath;
            }
            set
            {
                Common.Settings.AppSettings.HSCPluginConfigPath = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand ChoosePluginPathCommand { get { return new RelayCommand(ExecuteChoosePluginPathCommand); } }

        private void ExecuteChoosePluginPathCommand()
        {
            var msg = new ChoosePluginPathNotification();
            Messenger.Default.Send(msg);
        }

        #region public methods
        public void Clear()
        {
            this.Clients.Clear();
        }

        public void AddClient(FfxivClient client)
        {
            var clientVm = new FfxivClientViewModel(client);

            if (!this.Clients.Any(p => p.CharacterName == client.CharacterName))

                this.Clients.Add(clientVm);

            this.RefreshClients();

            UpdateHeaderText();
        }

        public void RemoveClient(string charName)
        {
            var clientVm = this.Clients.FirstOrDefault(p => p.CharacterName == charName);

            if (clientVm == null)
                return;

            this.Clients.Remove(clientVm);

            this.RefreshClients();

            UpdateHeaderText();
        }

        public void ConnectClient(string charName)
        {
            var clientVm = this.Clients.FirstOrDefault(p => p.CharacterName == charName);

            if (clientVm == null)
                return;

            clientVm.IsConnected = true;

            UpdateHeaderText();
        }

        public void DisconnectClient(string charName)
        {
            var clientVm = this.Clients.FirstOrDefault(p => p.CharacterName == charName);

            if (clientVm == null)
                return;

            clientVm.IsConnected = false;

            UpdateHeaderText();
        }
        #endregion

        #region private methods

        private void RefreshClients()
        {
            var ordered = this.Clients.OrderBy(p => p.Index).ToList();

            this.Clients.Clear();

            foreach (var client in ordered)
                this.Clients.Add(client);

        }

        private void UpdateHeaderText()
        {
            HeaderText = GetHeaderText();
        }

        private string GetHeaderText()
        {
            int total = Clients.Where(c => c.IsConnected).Count();
            string totalText = total == 1 ? "connected" : $"{total} connected";
            return total == 0 ? "FFXIV" : $"FFXIV - {totalText}";
        }
        #endregion

    }
}
