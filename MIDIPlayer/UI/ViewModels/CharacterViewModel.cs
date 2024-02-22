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

namespace Hscm.UI.ViewModels
{
    public class CharacterViewModel : ObservableViewModel<FFXIVCharacter>
    {

        public CharacterViewModel(FFXIVCharacter model) : base(model)
        {
            
        }

        
        public bool IsSelected
        {
            get
            {
                return Model.IsSelected;
            }
            set
            {
                Model.IsSelected = value;
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

        public RelayCommand<object> CharacterToggledCommand { get { return new RelayCommand<object>(ExecuteCharacterToggledCommand); } }


        private void ExecuteCharacterToggledCommand(object charName)
        {
                var msg = new CharacterToggledNotification() { CharacterName = (string)charName, IsSelected = IsSelected };
                Messenger.Default.Send(msg);
        }

    }
}
