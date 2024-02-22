using Common;
using Common.Helpers;
using Common.Models.Settings;
using Common.Music;
using Hscm.UI.Notifications;
using Hscm.UI.Notifications.Playlist;
using Hscm.UI.Notifications.Settings;
using Hscm.UI.Notifications.Settings.Pipe;
using Hscm.UI.Notifications.Tracks;
using Hscm.UI.Notifications.Window;
using Hscm.UI.ViewModels.MainWindow;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.IO;
using Hscm.UI.Services;
using Common.Models;
using Hscm.IPC;
using System.Windows.Markup;

namespace Hscm.UI
{
    public partial class MainWindow 
    {
        FileWatcher pluginWatcher;
        FileWatcher charConfigWatcher;

        #region game client handlers

        const string CharConfigFileName = "characters.config";

        private void CreateAndStartCharConfigWatcher()
        {
            charConfigWatcher = new FileWatcher();
            charConfigWatcher.Create(Common.Settings.AppSettings.HSCPluginConfigPath);
            charConfigWatcher.Filter = CharConfigFileName;
            charConfigWatcher.Created += ConfigWatcher_FileCreated;
            charConfigWatcher.Deleted += ConfigWatcher_FileDeleted;
            charConfigWatcher.Changed += ConfigWatcher_FileChanged;
            charConfigWatcher.Start();
        }

        private void UpdateCharacters()
        {
            HandleCharactersChanged();
        }

        private void ConfigWatcher_FileCreated(object sender, FileSystemEventArgs e)
        {
            AppendLog("", "plugin FFXIV character config created");
            HandleCharactersChanged();
        }

        private void ConfigWatcher_FileChanged(object sender, FileSystemEventArgs e)
        {
            AppendLog("", "plugin FFXIV character config changed");
            HandleCharactersChanged();
        }

        private void ConfigWatcher_FileDeleted(object sender, FileSystemEventArgs e)
        {
            AppendLog("", "plugin FFXIV character config deleted");
            HandleCharactersChanged();
        }

        private void HandleGameLaunched(GameClientInfo clientInfo)
        {
            //ensembleControl.HandleClientLaunched();
            this.viewModel.GameRunning = true;
        }

        private void UpdateCharIndex(GameClientInfo clientInfo)
        {
            Common.Settings.CharConfig = CharConfigReader.Load();

            if (Common.Settings.CharConfig == null)
                clientInfo.Index = -1;

            clientInfo.Index = Common.Settings.CharConfig.ToDictionary()[clientInfo.CharacterName];
        }


        private void HandleCharactersChanged()
        {
            AppendLog("", "updating FFXIV characters...");

            var charConfig = FileHelpers.Load<CharacterConfig>(Path.Combine(Common.Settings.AppSettings.HSCPluginConfigPath, CharConfigFileName));
            if (charConfig == null || charConfig.Characters.IsNullOrEmpty())
                return;

            int index = 0;
            foreach(var chr in charConfig.Characters)
            {
                if (chr.Value)//add logged in chars
                    HandleLoggedIn(chr.Key, index);
                else//remove logged out chars
                    HandleLoggedOutOrExited(index);
                index++;
            }
        }

        private void HandleLoggedIn(string charName, int index)
        {
            HandleLoggedIn(new GameClientInfo() { CharacterName = charName, Index = index });
        }

        private void HandleLoggedIn(GameClientInfo clientInfo)
        {
            AppendLog("", $"FFXIV character '{clientInfo.CharacterName}' logged in");

            gameClients.AddOrUpdate(clientInfo.Index, clientInfo);

            clientInfo.LoggedIn = true;

            this.viewModel.LoggedIn = true;

            tracksControl.HandleClientLoggedIn();
            FfxivControl.HandleClientLoggedIn(clientInfo.CharacterName, clientInfo.Index);

            ClientManager.Add(clientInfo.CharacterName, clientInfo.Index, true);

            var msg = new GameWindowFoundNotification() { Info = clientInfo };

            Messenger.Default.Send(msg);
        }

        private void HandleLoggedOutOrExited(int index)
        {
            if (!gameClients.ContainsKey(index))
                return;

            var clientInfo = gameClients[index];

            AppendLog("", $"FFXIV character '{clientInfo.CharacterName}' logged out or exited");

            clientInfo.LoggedIn = false;

            ClientManager.Remove(index);

            FfxivControl.HandleClientLoggedOutOrExited(clientInfo.CharacterName);
            if (!gameClients.Values.Any(c => c.LoggedIn))
                tracksControl.HandleAllClientsLoggedOut();

            var msg = new GameWindowLoggedOutOrExitedNotification() { Info = clientInfo };

            Messenger.Default.Send(msg);
        }


        //private void HandleGameWindowExited(IntPtr windowHandle)
        //{

        //    if (gameClients.IsNullOrEmpty() || !gameClients.Any(c => c.Value.WindowHandle == windowHandle))
        //        return;

        //    var clientInfo = gameClients.First(c => c.Value.WindowHandle == windowHandle).Value;

        //    ClientManager.Remove(clientInfo.Index);

        //    FfxivControl.HandleClientLoggedOutOrExited(clientInfo.CharacterName);

        //    var msg = new GameWindowLoggedOutOrExitedNotification() { Info = clientInfo };

        //    Messenger.Default.Send(msg);

        //    this.gameClients.Remove(clientInfo.Id);

        //    if (gameClients.IsNullOrEmpty())
        //    {

        //        this.viewModel.GameRunning = false;
        //        //ensembleControl.HandleAllClientsExited();
        //        tracksControl.HandleAllClientsExited();
        //    }

        //}

        #endregion


        private void IpcClient_Connected(object sender, int index)
        {
            var clientInfo = gameClients.FirstOrDefault(c => c.Value.Index == index);
            FfxivControl.HandleClientConnected(clientInfo.Value.CharacterName);
            ToastService.DisplayMessage($"Client ['{clientInfo.Value.CharacterName}'] connected.", MessageType.Success, true, 0);
        }

        private void IpcClient_Disconnected(object sender, int index)
        {
            var clientInfo = gameClients.FirstOrDefault(c => c.Value.Index == index);
            FfxivControl.HandleClientDisconnected(clientInfo.Value.CharacterName);
            ToastService.DisplayMessage($"Client ['{clientInfo.Value.CharacterName}'] disconnected.", MessageType.Info, true, 0);
        }

        private void DisconnectIpcClient(string charName)
        {
            var clientInfo = gameClients.FirstOrDefault(c => c.Value.CharacterName == charName);
            ClientManager.SendDisconnect(clientInfo.Value.Index);
            ClientManager.Disconnect(clientInfo.Value.Index);
        }

        private void ConnectIpcClient(string charName)
        {
            var clientInfo = gameClients.FirstOrDefault(c => c.Value.CharacterName == charName);
            ClientManager.SendConnect(clientInfo.Value.Index);
            ClientManager.Connect(clientInfo.Value.Index);
        }

        private void CreateAndStartPluginWatcher()
        {
            //pluginWatcher = new FileWatcher();
            //pluginWatcher.Create(Common.Settings.AppSettings.PluginConfigPath);
            //pluginWatcher.Created += PluginWatcher_FileCreated;
            //pluginWatcher.Deleted += PluginWatcher_FileDeleted;
            //pluginWatcher.Start();
        }

        private void ScanPluginPath()
        {
            AppendLog("", $"Checking plugin config path...");

            if (!string.IsNullOrEmpty(Common.Settings.AppSettings.HSCPluginConfigPath) && Common.Settings.AppSettings.HSCPluginConfigPath.Contains("HSC"))
            {
                viewModel.PluginFound = true;
                settingsViewModel.PluginFound = true;
                //LaunchWorkerService();
                AppendLog("", $"Plugin config path found.");
                return;
            }

            if (IsPluginFound())
                PluginFound();
            else
                PluginNotFound();
        }

        private bool IsPluginFound()
        {
            var dirs = Directory.GetDirectories(Common.Settings.AppSettings.PluginConfigPath);
            return (!dirs.IsNullOrEmpty() && dirs.Any(d => d.Contains("HSC")));
        }

        private string ChoosePluginPath(string path)
        {
            var openFolderDialog = new CommonOpenFileDialog();
            openFolderDialog.Title = "Choose plugin config path";
            openFolderDialog.IsFolderPicker = true;

            openFolderDialog.InitialDirectory = path;
            openFolderDialog.DefaultDirectory = path;

            var result = openFolderDialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
                return openFolderDialog.FileName;

            return null;
        }


        private string GetMostRecentPluginDirectory()
        {
            string mostRecent = Common.Settings.AppSettings.PluginConfigPath.GetMostRecentDirectory("HSC");

            var pluginDirs = Directory.GetDirectories(mostRecent);

            if (pluginDirs.IsNullOrEmpty())
                return mostRecent;

            return mostRecent.GetMostRecentDirectory();
        }

        private void PluginFound()
        {

            if (string.IsNullOrEmpty(Common.Settings.AppSettings.HSCPluginConfigPath))
            {
                ToastService.DisplayMessage($"Plugin config path found.", MessageType.Success, true);

                var pluginDirs = Common.Settings.AppSettings.PluginConfigPath.GetDirectories("HSC");

                if (pluginDirs.Count() > 1)
                {
                    string path = ChoosePluginPath(Common.Settings.AppSettings.PluginConfigPath);

                    if (string.IsNullOrEmpty(path))
                    {
                        string mostRecent = GetMostRecentPluginDirectory();
                        Common.Settings.AppSettings.HSCPluginConfigPath = mostRecent;
                        ToastService.DisplayMessage($"No plugin config path chosen.\n\rPlugin playlists and settings will be saved to '{mostRecent}'.");
                    }
                    else
                    {
                        Common.Settings.AppSettings.HSCPluginConfigPath = path;
                        ToastService.DisplayMessage($"Plugin playlists and settings will be saved to '{path}'.");
                    }

                    viewModel.PluginFound = true;
                    settingsViewModel.PluginFound = true;
                    FfxivControl.UpdatePluginPath(Common.Settings.AppSettings.HSCPluginConfigPath);

                    SaveSettings(true, false);

                }

                else
                {
                    var pluginPath = Directory.GetDirectories(Common.Settings.AppSettings.PluginConfigPath).FirstOrDefault(d => d.Contains("HSC"));
                    Common.Settings.AppSettings.HSCPluginConfigPath = pluginPath;
                    var verDirs = Directory.GetDirectories(pluginPath);

                    if (verDirs.Count() > 1)
                    {
                        string path = ChoosePluginPath(pluginPath);

                        if (string.IsNullOrEmpty(path))
                        {
                            string mostRecent = pluginPath.GetMostRecentDirectory();
                            Common.Settings.AppSettings.HSCPluginConfigPath = mostRecent;
                            ToastService.DisplayMessage($"No plugin config path chosen.\n\rPlugin playlists and settings will be saved to '{mostRecent}'.");
                        }
                        else
                        {
                            Common.Settings.AppSettings.HSCPluginConfigPath = path;
                            ToastService.DisplayMessage($"Plugin playlists and settings will be saved to '{path}'.");
                        }
                    }
                    else
                    {
                        if (verDirs.Count() != 0)
                            Common.Settings.AppSettings.HSCPluginConfigPath = verDirs.First();
                    }
            
                    viewModel.PluginFound = true;
                    settingsViewModel.PluginFound = true;
                    FfxivControl.UpdatePluginPath(Common.Settings.AppSettings.HSCPluginConfigPath);

                    SaveSettings(true, false);

                }
            }
            else
            {
                viewModel.PluginFound = true;
                settingsViewModel.PluginFound = true;
            }
        }

        private void PluginNotFound()
        {
            viewModel.PluginFound = false;
            settingsViewModel.PluginFound = false;
            ToastService.DisplayMessage("HSC plugin config path was not found.", MessageType.Error, true);
        }

        //private void PluginWatcher_FileCreated(object sender, FileSystemEventArgs e)
        //{
        //    if (!e.FullPath.Contains("MidiBard")) return;

        //    if (IsMidiBardPluginFound())
        //        MidiBardPluginFound();
        //}

        //private void PluginWatcher_FileDeleted(object sender, FileSystemEventArgs e)
        //{
        //    if (!e.FullPath.Contains("MidiBard")) return;

        //    if (!IsMidiBardPluginFound())
        //        MidiBardPluginNotFound();
        //}
    }
}
