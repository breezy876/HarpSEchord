using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hscm.UI.Notifications;
using Hscm.UI.Notifications.Window;
using Hscm.UI.Notifications.Tracks;
using Hscm.UI.Notifications.Settings;
using Hscm.UI.Notifications.Playlist;
using System.Windows.Controls;
using Common;
using Hscm.UI.Notifications.Settings.Pipe;
using Hscm.UI.ViewModels.MainWindow;
using Common.Models.Ensemble;
using Common.Helpers;
using System.Windows.Media;
using System.Windows.Threading;
using Common.Messaging.Player;
using System.Windows;
using Common.Messaging;
using Common.Messaging.Settings;
using GalaSoft.MvvmLight.Messaging;
using System.Diagnostics;
using System.Windows.Input;
using Common.Music;
using Common.Interop;
using static Common.Interop.WindowHelpers;
using System.Runtime.InteropServices;
using Common.Midi;
using System.Threading;
using Microsoft.WindowsAPICodePack.Dialogs;
using Hscm.IPC;

namespace Hscm.UI
{
    public partial class MainWindow
    {
        private async void ChoosePluginPathNotificationReceived(ChoosePluginPathNotification obj)
        {
            var openFolderDialog = new CommonOpenFileDialog();
            openFolderDialog.Title = "Choose HSC plugin config path";
            openFolderDialog.IsFolderPicker = true;

            openFolderDialog.InitialDirectory = Common.Settings.AppSettings.PluginConfigPath;
            openFolderDialog.DefaultDirectory = Common.Settings.AppSettings.PluginConfigPath;

            var result = openFolderDialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                FfxivControl.UpdatePluginPath(openFolderDialog.FileName);
                viewModel.PluginFound = true;
                settingsViewModel.PluginFound = true;
                await SaveSettings(true, false);
            }
        }

        private void ConnectFfxivClientNotificationReceived(ConnectFfxivClientNotification obj)
        {
            if (obj.Connect)
                ConnectIpcClient(obj.CharacterName);
            else
                DisconnectIpcClient(obj.CharacterName);
        }


        private void ToggleInstrumentsNotificationReceived(ToggleInstrumentsNotification obj)
        {
            ClientManager.SwitchInstruments();
        }

        private void CloseInstrumentsNotificationReceived(CloseInstrumentsNotification obj)
        {
            ClientManager.CloseInstruments();
        }

        private void GameWindowFoundNotificationReceived(GameWindowFoundNotification obj)
        {

            if (viewModel.SelectedSequence == null)
                return;

            tracksControl.HandleGameWindowFound(obj.Info.CharacterName, obj.Info.Index);
        }

        private void GameWindowExitedNotificationReceived(GameWindowLoggedOutOrExitedNotification obj)
        {
            if (viewModel.SelectedSequence == null)
                return;

            tracksControl.HandleGameWindowExited(obj.Info.CharacterName);
        }


    }
}
