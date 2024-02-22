using Hscm.UI.Notifications;
using Hscm.UI.Notifications.Playlist;
using Hscm.UI.ViewModels;
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
using Hscm.UI.ViewModels.Ffxiv;
using Hscm.Models.Ffxiv;
using Microsoft.WindowsAPICodePack.Dialogs;
using Common;

namespace Hscm.UI
{
    /// <summary>
    /// Interaction logic for InfoControl.xaml
    /// </summary>
    public partial class FfxivControl : StackPanel
    {
        private FfxivViewModel viewModel;

        public FfxivControl()
        {
            InitializeComponent();
        }

        public void Initialize(FfxivViewModel viewModel)
        {
            this.viewModel = viewModel;

            this.DataContext = viewModel;


        }


        public void AddCharsFromConfigInTestMode()
        {
            if (Common.Settings.AppSettings.GeneralSettings.TestMode)
            {
                var charConfig = CharConfigReader.Load();

                foreach (var chara in charConfig.ToDictionary())
                {
                    this.viewModel.AddClient(new FfxivClient() { CharacterName = chara.Key, Index = chara.Value });
                }
            }
        }

        public void UpdatePluginPath(string path)
        {
            viewModel.CurrentPluginPath = path;
        }

        public void HandleClientConnected(string charName)
        {
            Dispatcher.Invoke(() =>
            {
                this.viewModel.ConnectClient(charName);
            });
        }

        public void HandleClientDisconnected(string charName)
        {
            Dispatcher.Invoke(() =>
            {
                this.viewModel.DisconnectClient(charName);
            });
        }

        public void HandleClientLoggedIn(string charName, int index)
        {
            Dispatcher.Invoke(() =>
            {
                this.viewModel.AddClient(new FfxivClient() { CharacterName = charName, Index = index });
            });
        }

        public void HandleClientLoggedOutOrExited(string charName)
        {
            Dispatcher.Invoke(() =>
            {
                this.viewModel.RemoveClient(charName);
            });
        }
    }
}
