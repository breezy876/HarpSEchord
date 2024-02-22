using Common.Models.Ensemble;
using Hscm.UI.Notifications.Tracks;
using Hscm.UI.ViewModels;
using Hscm.UI.ViewModels.Settings;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hscm.UI
{
    /// <summary>
    /// Interaction logic for InstrumentPanel.xaml
    /// </summary>
    public partial class CharactersPanel : UserControl
    {
        public CharactersPanel()
        {
            InitializeComponent();


        }

        private CharactersPanelViewModel viewModel;


        public void Initialize(CharactersPanelViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.DataContext = this.viewModel;


        }


        public void HandleGameWindowExited(string charName)
        {

            var chara = new FFXIVCharacter()
            {
                CharacterName = charName
            };

            this.Dispatcher.Invoke(() =>
            {
                this.viewModel.RemoveCharacter(charName);
            });
        }

        public void HandleGameWindowFound(string charName, int index)
        {

            var chara = new FFXIVCharacter()
            {
                CharacterName = charName,
                Index = index
            };

            this.Dispatcher.Invoke(() =>
            {
                this.viewModel.AddCharacter(chara);
            });
        }

        internal void SelectCharacter(string member)
        {
            viewModel.SelectCharacter(member);
        }

        internal void DeselectCharacter(string member)
        {
            viewModel.DeselectCharacter(member);
        }
    }
}
