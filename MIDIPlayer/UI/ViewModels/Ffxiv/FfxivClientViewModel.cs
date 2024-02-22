using Common.Models.Ensemble;
using Common.Music;
using Hscm.UI.Notifications;
using Hscm.UI.Notifications.Tracks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Hscm.Models.Ffxiv;

namespace Hscm.UI.ViewModels.Ffxiv
{
    public class FfxivClientViewModel : ObservableViewModel<FfxivClient>
    {

        public FfxivClientViewModel(FfxivClient model) : base(model)
        {
            
        }

        
        public bool IsConnected
        {
            get
            {
                return Model.IsConnected;
            }
            set
            {
                Model.IsConnected = value;
                RaisePropertyChanged();
            }
        }

        public int Index
        {
            get
            {
                return Model.Index;
            }
            set
            {
                Model.Index = value;
                RaisePropertyChanged();
            }
        }

        public string CharacterName
        {
            get
            {
                return Model.CharacterName;
            }
            set
            {
                Model.CharacterName = value;
                RaisePropertyChanged();
            }
        }



        public RelayCommand ConnectCommand { get { return new RelayCommand(ExecuteConnectCommand); } }

        public RelayCommand DisconnectCommand { get { return new RelayCommand(ExecuteDisconnectCommand); } }

        private void ExecuteConnectCommand()
        {
                var msg = new ConnectFfxivClientNotification() { CharacterName = CharacterName, Connect = true };
                Messenger.Default.Send(msg);
        }

        private void ExecuteDisconnectCommand()
        {
            var msg = new ConnectFfxivClientNotification() { CharacterName = CharacterName };
            Messenger.Default.Send(msg);
        }
    }
}
