using Common;
using Common.Helpers;
using Common.Models.Ensemble;
using Common.Music;
using Hscm.UI.Notifications;
using Hscm.UI.Notifications.Playlist;
using Hscm.UI.Notifications.Tracks;
using Hscm.UI.ViewModels.Ensemble;
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
    public class CharactersPanelViewModel : ViewModelBase
    {

        public ObservableCollection<CharacterViewModel> Characters { get; private set; }

        public CharactersPanelViewModel()
        {
            Characters = new ObservableCollection<CharacterViewModel>();
        }

        #region public methods
        public void Clear()
        {
            this.Characters.Clear();
        }

        public void AddCharacter(FFXIVCharacter chara)
        {
            var CharacterVm = new CharacterViewModel(chara);

            if (!this.Characters.Any(p => p.CharacterName == chara.CharacterName))

                this.Characters.Add(CharacterVm);

            this.RefreshCharacters();
        }

            public void RemoveCharacter(string charName)
        {
            var charVm = this.Characters.FirstOrDefault(p => p.CharacterName == charName);

            if (charVm == null)
                return;

            this.Characters.Remove(charVm);

            this.RefreshCharacters();
        }
        public void SelectCharacter(string charName)
        {
            var chara = this.Characters.FirstOrDefault(p => p.CharacterName == charName);

            if (chara == null || chara.IsSelected)
                return;

            chara.IsSelected = true;
        }

        public void DeselectCharacter(string charName)
        {
            var chara = this.Characters.FirstOrDefault(p => p.CharacterName == charName);

            if (chara == null || !chara.IsSelected)
                return;

            chara.IsSelected = false;
        }
        #endregion

        #region private methods

        private void RefreshCharacters()
        {
            var ordered = this.Characters.OrderBy(p => p.Index).ToList();

            this.Characters.Clear();

            foreach (var Character in ordered)
                this.Characters.Add(Character);

        }

        #endregion
    }
}
