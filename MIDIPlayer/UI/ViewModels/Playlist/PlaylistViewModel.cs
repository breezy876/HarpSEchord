using Hscm.UI.Notifications;
using Hscm.UI.Notifications.Playlist;
using Hscm;
using Hscm.UI;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Specialized;
using Hscm.UI.Notifications.Tracks;
using Common;
using Common.Music;
using Common.Models.Playlist;
using Common.Helpers;
using Hscm.UI.ViewModels.MainWindow;
using System.Windows.Data;
using System.Windows.Forms;
using Hscm.UI.Services;

namespace Hscm.UI
{
    public class PlaylistViewModel : ObservableViewModel<Playlist>
    {
        //private bool isVisible;

        private string header;
        private string repeatModeImage;
        private string repeatModeToolTip;
        private string lastToRemove;

        private bool isFilterVisible;
        private bool isFilterApplied;

        private bool isEmpty;
        private bool hasItems;
        private double gridHeight;

        private bool cleared;
        private bool showGroupingButtons;
        private bool rowsSelected;


        public PlaylistViewModel(Playlist playlist) : base(playlist)
        {
            this.Menu = new PlaylistMenuViewModel();
            this.Items = new ObservableCollection<PlaylistItemViewModel>();
            this.Items.CollectionChanged += this.FilesChanged;

            IsGroupingEnabled = Common.Settings.AppSettings.PlaylistSettings.IsGroupingEnabled;

            SettingsVisible = false;

            GridHeight = 200;

            RepeatMode = (int)Common.Models.Playlist.RepeatMode.Single;

        }


        #region properties
        public ObservableCollection<PlaylistItemViewModel> Items { get; private set; }

        public PlaylistMenuViewModel Menu { get; private set; }

        public int Total => this.Items.Count;

        public int RepeatMode
        {
            get { return Common.Settings.AppSettings.PlaylistSettings.RepeatMode;  }
            set
            {
                Common.Settings.AppSettings.PlaylistSettings.RepeatMode = value;
                UpdatePlaylistRepeatModeToolTip();
                UpdatePlaylistRepeatModeImage();
                RaisePropertyChanged();
            }
        }

        public string RepeatModeImage
        {
            get { return repeatModeImage; }
            set
            {
                repeatModeImage = value;
                RaisePropertyChanged();
            }
        }
        public string RepeatModeToolTip
        {
            get { return repeatModeToolTip; }
            set
            {
                repeatModeToolTip = value;
                RaisePropertyChanged();
            }
        }

        public double GridHeight
        {
            get { return gridHeight; }
            set
            {
                gridHeight = value;
                RaisePropertyChanged();
            }
        }

        public bool IsEmpty
        {
            get { return isEmpty; }
            set
            {
                isEmpty = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowGroupingButtons
        {
            get { return showGroupingButtons; }
            set
            {
                showGroupingButtons = value;
                RaisePropertyChanged();
            }
        }

        public bool SettingsVisible
        {
            get { return Common.Settings.AppSettings.PlaylistSettings.SettingsVisible; }
            set
            {
                Common.Settings.AppSettings.PlaylistSettings.SettingsVisible = value;
                RaisePropertyChanged();
            }
        }

        public bool IsGroupingEnabled
        {
            get { return Common.Settings.AppSettings.PlaylistSettings.IsGroupingEnabled; }
            set
            {
                Common.Settings.AppSettings.PlaylistSettings.IsGroupingEnabled = value;
                RaisePropertyChanged();
            }
        }


        public bool IsFilterVisible
        {
            get { return isFilterVisible; }
            set
            {
                isFilterVisible = value;
                RaisePropertyChanged();
            }
        }

        internal int GetSelectedIndex() => GetSelectedItems().First().Index;

        public bool IsFilterApplied
        {
            get { return isFilterApplied; }
            set
            {
                isFilterApplied = value;
                RaisePropertyChanged();
            }
        }

        public string Title
        {
            get { return Model.Title; }
            set { Model.Title = value; RaisePropertyChanged(); }
        }

        public string FilePath
        {
            get { return Model.FilePath; }
            set { Model.FilePath = value; RaisePropertyChanged(); }
        }

        public string FullPath => Model.FullPath;

        public bool HasItems
        {
            get
            {
                return hasItems;
            }
            set
            {
                hasItems = value;
                RaisePropertyChanged();
            }
        }

        public string Header
        {
            get { return header; }
            set
            {
                header = value;
                RaisePropertyChanged();
            }
        }

        public bool RowsSelected
        {
            get { return rowsSelected; }
            set
            {
                rowsSelected = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region public methods

        public PlaylistItemViewModel GetCurrent() => this.Items[this.Model.GetCurrent().Index];

        public void Change(string title, bool playing = true)
        {
            if (!this.Items.Any())
                return;

            var index = this.GetIndex(title);

            if (index == -1)
                return;

            this.Model.MoveTo(index);

            if (playing)
            {
                foreach (var item in this.Items)
                    item.IsPlaying = false;

                var targetItem = this.Items[index];
                targetItem.IsPlaying = true;
            }

        }

        public void MoveTo(int index)
        {
            Model.MoveTo(index);
        }

        public void Clear()
        {
            this.Model.Clear();
            this.Items.Clear();
        }

        public void ExpandAll()
        {
            foreach (var item in this.Items)
                item.IsExpanded = true;
        }

        public void CollapseAll()
        {
            foreach (var item in this.Items)
                item.IsExpanded = false;
        }

        public void ClearFilter()
        {
            this.Header = $"# ({this.Items.Count} total)";

            IsFilterApplied = false;
            //Model.ClearFilter();
        }

        public void ApplyFilter()
        {
            var filtered = this.Items.Where(i => i.IsFiltered).ToArray();

            int totalFiltered = filtered.Length;

            this.Header = $"# ({this.Items.Count} total, {totalFiltered} matches)";

            IsFilterApplied = true;

            //Model.Filter(filtered.Select(i => i.Title));
        }

        public void Update(PlaylistItemViewModel viewModel)
        {
            this.Items[viewModel.Index] = viewModel;
        }

        public PlaylistItemViewModel GetByTitle(string title)
        {
            return this.Items.FirstOrDefault(i => i.Sequence.Info.Title == title);
        }

        public PlaylistItemViewModel GetByIndex(int index)
        {
            return this.Items.FirstOrDefault(i => i.Index == index);
        }

        public void Update(MidiSequence midiSequence)
        {
            int index = GetIndex(midiSequence.Info.Title);

            if (index == -1)
                return;

            this.Items[index].Sequence = midiSequence;
        }
        public void Add(MidiSequence sequence, int index, bool loading = false)
        {
            if (!ContainsFile(sequence.Info.FilePath))
            {
                var itemViewModel = new PlaylistItemViewModel(new PlaylistItem() { Sequence = sequence, Index= index });
 
                this.Items.Add(itemViewModel);
            }
        }

        public void Remove()
        {
            var files = this.GetSelectedItems();
            lastToRemove = files.Last().Sequence.Info.Title;

            foreach (var file in files)
            {
                this.Items.RemoveAt(file.Index);
            }

            ToastService.DisplayMessage($"Removed {files.Count()} MIDIs from playlist.", MessageType.Success);
        }

        public void RemoveAll()
        {
            cleared = true;
            this.Items.Clear();
        }

        public Playlist GetPluginPlaylistModel()
        {
            return _GetPlaylistModel(this.Title, null, false);
        }

        public Playlist GetPlaylistModel()
        {
            return _GetPlaylistModel(this.Title);
        }

        public Playlist GetPlaylistModel(string title, string settingsFile = null)
        {
            return _GetPlaylistModel(title, settingsFile);
        }


        public bool ContainsFile(string filePath)
        {
            return this.Items.Any(f => f.FilePath == filePath);
        }

        public async Task Save()
        {
            if (!Settings.AppSettings.PlaylistSettings.SavePlaylist)
                return;

            string path = Path.Combine(AppHelpers.GetAppAbsolutePath(), FullPath);
            var model = GetPlaylistModel();

            await Save(model, path);
        }

        public async Task Save(Playlist playlist, string filePath)
        {
            if (!Settings.AppSettings.PlaylistSettings.SavePlaylist)
                return;

            //await Common.Settings.SaveCharConfig();

            await _Save(playlist, filePath);

            await _SaveToPluginPath(filePath);
   
        }


        public async Task SaveToPluginPath()
        {
            if (!Settings.AppSettings.PlaylistSettings.SavePlaylist)
                return;

            await _SaveToPluginPath(FullPath);


        }

        public async Task SaveSettingsToPluginPath()
        {
            if (!Settings.AppSettings.PlaylistSettings.SavePlaylistSettings)
                return;

            await _SaveSettingsToPluginPath(Path.Combine(AppHelpers.GetAppAbsolutePath(), Common.Settings.Playlist.SettingsFile));
        }

        public async Task SavePlaylistSettings(string title = null, string filePath = null)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    if (string.IsNullOrEmpty(Common.Settings.Playlist.SettingsFile))
                        filePath = System.IO.Path.Combine(Common.Settings.Playlist.FilePath, $"{Common.Settings.Playlist.Title}.settings.json");
                    else
                        filePath = Common.Settings.Playlist.SettingsFile;
                }

                if (Items.IsNullOrEmpty())
                    return;

                if (!title.IsNullOrEmpty())
                {
                    var song = this.GetByTitle(title);

                    if (song != null)
                        Common.Settings.PlaylistSettings.Settings.AddOrUpdate(title, song.Sequence);
                }
                else
                {

                    foreach (var song in this.Items)

                        Common.Settings.PlaylistSettings.Settings.AddOrUpdate(song.Sequence.Info.Title, song.Sequence);
                }


                //foreach (var setting in Common.Settings.PlaylistSettings.Settings)
                //    setting.Value.FixTracks();


                ////Remove settings not already in playlist 


                await Common.Settings.SavePlaylistSettings(filePath);

                await SaveSettingsToPluginPath();
            }
            catch (Exception ex)
            {
                //AppendLog("", $"Error: unable to save playlist settings '{filePath}'.");
            }
        }
        #endregion

        #region private methods
        private void UpdatePlaylistRepeatModeToolTip()
        {
            var tooltips = new string[]
            {
                "Repeat mode: single",
                "Repeat mode: single repeat",
                "Repeat mode: list ordered",
                "Repeat mode: list repeat",
                "Repeat mode: shuffle"
            };

            RepeatModeToolTip = tooltips[RepeatMode];
        }


        private void UpdatePlaylistRepeatModeImage()
        {
            var images = new string[]
            {
                "icons8-up-left-64.png",
                "icons8-single-repeat-48.png",
                "icons8-list-64.png",
                "icons8-repeat-48.png",
                "icons8-shuffle-48.png"
            };

            RepeatModeImage = $"../Images/{images[RepeatMode]}";
        }

        private Playlist _GetPlaylistModel(string title, string settingsFile = null, bool relative = true)
        {

            var model = this.CreateModel(title);

            model.SettingsFile = String.IsNullOrEmpty(settingsFile) ? Common.Settings.Playlist.SettingsFile : settingsFile;

            if (relative)
                model.PrepareForSave();

            return model;
        }

        private int GetIndex(string title)
        {
            var item = this.Items.FirstOrDefault(i => i.Sequence.Info.Title.Equals(title));
            return item == null ? -1 : item.Index;

        }
        private List<PlaylistItemViewModel> GetItems()
        {

            if (this.Items.Any())
                return this.Items.ToList();

            return new List<PlaylistItemViewModel>();
        }

        private bool IsRowSelected(object args)
        {
            var routedEventArgs = args as RoutedEventArgs;
            var dataGrid = routedEventArgs.Source as System.Windows.Controls.DataGrid;
            DataGridRow row = ItemsControl.ContainerFromElement(dataGrid, routedEventArgs.OriginalSource as DependencyObject) as DataGridRow;
            return row != null;
        }

        private PlaylistItemViewModel GetSelectedItem(object args)
        {
            PlaylistItemViewModel selectedItem = this.GetFirstSelectedItem();

            if (selectedItem == null)
            {
                var routedEventArgs = args as RoutedEventArgs;
                var dataGrid = routedEventArgs.Source as System.Windows.Controls.DataGrid;
                selectedItem = (PlaylistItemViewModel)dataGrid.SelectedItem;
            }
            return selectedItem;
        }

        public PlaylistItemViewModel GetFirstSelectedItem()
        {
            var file = this.Items.FirstOrDefault(f => f.IsSelected);
            return file;
        }

        public void UpdateIndexes()
        {
            int index = 0;

            foreach (var item in this.Items)
            {
                item.Index = index;
                index++;
            }
        }

        public void HandleGroupsToggled()
        {
            ShowGroupingButtons = IsGroupingEnabled && HasItems;
        }

        private bool ContainsIndex(int index)
        {
            return this.Items.Any(i => i.Index == index);
        }

        private void MoveItem(int srcIndex, int destIndex)
        {
            this.Items.Move(srcIndex, destIndex);
        }

        private IEnumerable<PlaylistItemViewModel> GetSelectedItems()
        {
            var files = this.Items.Where(f => f.IsSelected);
            return files.ToArray();
        }

        private Playlist CreateModel(string title)
        {
            return new Playlist(title, this.GetItems().Select(i => new PlaylistItem() { Sequence = i.Sequence }).ToList());
        }

        private void SaveSettings()
        {
            var itemsChangedMsg = new SaveSettingsNotification() { SaveAppSettings = true };
            Messenger.Default.Send(itemsChangedMsg);
        }

        private async Task _SaveToPluginPath(string path)
        {
            if (string.IsNullOrEmpty(Settings.AppSettings.HSCPluginConfigPath))
            {
                AppendLog("", "Error: saving playlist to plugin config path failed. No path has been chosen.");
                ToastService.DisplayMessage("Error when saving playlist to plugin config path. No path has been chosen.", MessageType.Error);
                return;
            }

            string fullPath = Path.Combine(Settings.AppSettings.HSCPluginConfigPath, Path.GetFileName(path));
            var model = GetPluginPlaylistModel();

            foreach (string f in Directory.EnumerateFiles(Settings.AppSettings.HSCPluginConfigPath, "*.pl"))
            {
                File.Delete(f);
            }

            await _Save(model, fullPath);
        }

        private async Task _SaveSettingsToPluginPath(string path)
        {
            if (string.IsNullOrEmpty(Settings.AppSettings.HSCPluginConfigPath))
            {
                AppendLog("", "Error: saving playlist to plugin config path failed. No path has been chosen.");
                ToastService.DisplayMessage("Error when saving playlist to plugin config path. No path has been chosen.", MessageType.Error);
                return;
            }

            string fullPath = Path.Combine(Settings.AppSettings.HSCPluginConfigPath, Path.GetFileName(path));
            foreach (string f in Directory.EnumerateFiles(Settings.AppSettings.HSCPluginConfigPath, "*.settings.json"))
            {
                File.Delete(f);
            }

            await Common.Settings.SavePlaylistSettings(fullPath);
        }

        private async Task _Save(Playlist playlist, string filePath)
        {
            await Task.Run(() => FileHelpers.Save(playlist, filePath));
        }
        #endregion

        #region commands
        public RelayCommand NewCommand { get { return new RelayCommand(ExecuteNewCommand); } }

        public RelayCommand CopyCommand { get { return new RelayCommand(ExecuteCopyCommand); } }

        public RelayCommand ToggleGroupingCommand { get { return new RelayCommand(ExecuteToggleGroupingCommand); } }

        public RelayCommand CollapseAllCommand { get { return new RelayCommand(ExecuteCollapseAllCommand); } }

        public RelayCommand ExpandAllCommand { get { return new RelayCommand(ExecuteExpandAllCommand); } }


        public RelayCommand SaveSongSettingsCommand { get { return new RelayCommand(ExecuteSaveSongSettingsCommand); } }

        public RelayCommand LoadCurrentSongSettingsCommand { get { return new RelayCommand(ExecuteLoadCurrentSongSettingsCommand); } }

        public RelayCommand LoadSongSettingsCommand { get { return new RelayCommand(ExecuteLoadSongSettingsCommand); } }

        public RelayCommand SavePlaylistSettingsCommand { get { return new RelayCommand(ExecuteSavePlaylistSettingsCommand); } }

        public RelayCommand OpenPlaylistSettingsCommand { get { return new RelayCommand(ExecuteLoadPlaylistSettingsCommand); } }

        public RelayCommand AddFilesCommand { get { return new RelayCommand(ExecuteAddFilesCommand); } }

        public RelayCommand AddDirectoryCommand { get { return new RelayCommand(ExecuteAddDirectoryCommand); } }

        public RelayCommand RemoveCommand { get { return new RelayCommand(ExecuteRemoveCommand); } }

        public RelayCommand RemoveAllCommand { get { return new RelayCommand(ExecuteRemoveAllCommand); } }

        public RelayCommand SaveCommand { get { return new RelayCommand(ExecuteSaveCommand); } }

        public RelayCommand SaveAsCommand { get { return new RelayCommand(ExecuteSaveAsCommand); } }

        public RelayCommand OpenCommand { get { return new RelayCommand(ExecuteOpenCommand); } }

        public RelayCommand<object> SelectedRowsChangedCommand { get { return new RelayCommand<object>(ExecuteSelectedRowsChangedCommand); } }

        public RelayCommand RefreshCommand { get { return new RelayCommand(ExecuteRefreshCommand); } }

        public RelayCommand ShowFilterCommand { get { return new RelayCommand(ExecuteShowFilterCommand); } }

        public RelayCommand ClearFilterCommand { get { return new RelayCommand(ExecuteClearFilterCommand); } }

        public RelayCommand ToggleSettingsCommand { get { return new RelayCommand(ExecuteToggleSettingsCommand); } }
        public RelayCommand ToggleRepeatModeCommand { get { return new RelayCommand(ExecuteToggleRepeatModeCommand); } }

        private void ExecuteNewCommand()
        {
            var notification = new NewPlaylistNotification();
            Messenger.Default.Send(notification);
        }

        private void ExecuteCopyCommand()
        {
            var notification = new CopyPlaylistNotification();
            Messenger.Default.Send(notification);
        }

        private void ExecuteToggleGroupingCommand()
        {
            var notification = new ToggleGroupsNotification();
            Messenger.Default.Send(notification);
        }

        private void ExecuteCollapseAllCommand()
        {

            var notification = new CollapseGroupsNotification();
            Messenger.Default.Send(notification);
        }

        private void ExecuteExpandAllCommand()
        {

            var notification = new ExpandGroupsNotification();
            Messenger.Default.Send(notification);
        }


        private void ExecuteClearFilterCommand()
        {
            this.IsFilterVisible = false;

            var notification = new ClearPlaylistFilterNotification();
            Messenger.Default.Send(notification);
        }

        private void ExecuteShowFilterCommand()
        {
            this.IsFilterVisible = true;
        }

        private void ExecuteSaveSongSettingsCommand()
        {
            var notification = new ShowSaveSongSettingsDialogNotification();
            Messenger.Default.Send(notification);
        }

        private void ExecuteLoadSongSettingsCommand()
        {
            var notification = new LoadSongSettingsNotification();
            Messenger.Default.Send(notification);
        }


        private void ExecuteLoadCurrentSongSettingsCommand()
        {
            var notification = new LoadCurrentSongSettingsNotification();
            Messenger.Default.Send(notification);
        }

        private void ExecuteLoadPlaylistSettingsCommand()
        {
            var notification = new OpenPlaylistSettingsNotification();
            Messenger.Default.Send(notification);
        }

        private void ExecuteSavePlaylistSettingsCommand()
        {
            var notification = new SavePlaylistSettingsNotification();
            Messenger.Default.Send(notification);
        }

        //private void ExecuteDropCommand(object args)
        //{
        //    var e = args as DragEventArgs;

        //    if (e.Data.GetDataPresent(DataFormats.FileDrop))
        //    {
        //        //Note that you can have more than one file.
        //        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

        //        var notification = new PlaylistDropNotification() { FileNames = files };

        //        Messenger.Default.Send(notification);
        //    }
        //}

        private void ExecuteSelectedRowsChangedCommand(object args)
        {
            var e = (SelectionChangedEventArgs)args;

            foreach (var item in e.AddedItems)
            {
                var playlistItem = item as PlaylistItemViewModel;
                playlistItem.IsSelected = true;
            }

            foreach (var item in e.RemovedItems)
            {
                var playlistItem = item as PlaylistItemViewModel;
                playlistItem.IsSelected = false;
            }

            var totalSelected = this.GetSelectedItems().Count();

            if (totalSelected > 0)
            {
                this.Menu.ShowRemove = true;
                this.Menu.ShowPlay = true;
                this.Menu.ShowTracks = true;
            }
            else
            {
                this.Menu.ShowRemove = false;
                this.Menu.ShowPlay = false;
                this.Menu.ShowTracks = false;
            }

            RowsSelected = totalSelected > 0;

            if (totalSelected == 1)
            {
                var selectedItem = this.GetFirstSelectedItem();

                var notification = new PlaylistSequenceSelectedNotification() { Sequence = selectedItem.Sequence };
                Messenger.Default.Send(notification);
            }
        }

        private void ExecuteRefreshCommand()
        {
            var notification = new RefreshPlaylistNotification();

            Messenger.Default.Send(notification);
        }

        private void ExecuteOpenCommand()
        {
            var notification = new OpenPlaylistNotification();
            Messenger.Default.Send(notification);
        }

        private void ExecuteSaveCommand()
        {
            var notification = new SavePlaylistNotification();
            Messenger.Default.Send(notification);
        }

        private void ExecuteSaveAsCommand()
        {
            var notification = new SavePlaylistAsNotification();
            Messenger.Default.Send(notification);
        }

        private void ExecuteRemoveCommand()
        {
            this.Remove();
        }

        private void ExecuteRemoveAllCommand()
        {
            this.RemoveAll();
        }

        private void ExecuteAddDirectoryCommand()
        {
            var notification = new AddPlaylistDirectoryNotification();
            Messenger.Default.Send(notification);
        }

        private void ExecuteAddFilesCommand()
        {
            var notification = new AddPlaylistFilesNotification();
            Messenger.Default.Send(notification);
        }

        private void ExecuteToggleSettingsCommand()
        {
            var notification = new SaveSettingsNotification() { SaveAppSettings = true };
            Messenger.Default.Send(notification);
        }

        private void ExecuteToggleRepeatModeCommand()
        {
            int repeatType = (int)RepeatMode;
            repeatType++;
            repeatType %= 5;
            RepeatMode = repeatType;

            var notification = new SaveSettingsNotification() { SaveAppSettings = true };
            Messenger.Default.Send(notification);
        }

        #endregion

        #region event handlers
        private async void FilesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var item = e.NewItems.Cast<PlaylistItemViewModel>().First();

                //var settings = Common.Settings.PlaylistSettings.Settings.ContainsKey(item.Sequence.Info.Title) ? 
                //    Common.Settings.PlaylistSettings.Settings[item.Sequence.Info.Title] : null;

                //item.Index =  settings == null ? e.NewStartingIndex : settings.Index;

                item.Index = e.NewStartingIndex;
                this.Model.Add(new PlaylistItem() { Sequence = item.Sequence, Index = item.Index });

                UpdateIndexes();

                this.Header = $"# ({this.Items.Count} total)";



            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                int total = GetSelectedItems().Count();

                var items = e.OldItems.Cast<PlaylistItemViewModel>().ToArray();
                this.Model.Remove(items.Select(i => new PlaylistItem() { Sequence = i.Sequence, Index = e.OldStartingIndex }));

                UpdateIndexes();

                if (!items.First().Title.Equals(lastToRemove))
                    return;

                lastToRemove = "";

                await Save();

            }
            else if (e.Action == NotifyCollectionChangedAction.Reset && cleared)
            {
                Model.Clear();
                await Save();



                cleared = false;
            }

            if (this.Total > 0)
            {
                HasItems = true;
                this.Menu.ShowSave = true;
                this.Menu.ShowSaveAs = true;
                this.Menu.ShowRemoveAll = true;
            }
            else
            {
                HasItems = false;
                this.Menu.ShowSave = false;
                this.Menu.ShowSaveAs = false;
                this.Menu.ShowRemove = false;
                this.Menu.ShowRemoveAll = false;
                this.Menu.ShowPlay = false;

                this.Header = "#";

                var emptyMsg = new PlaylistEmptyNotification();
                Messenger.Default.Send(emptyMsg);
            }

            ShowGroupingButtons = IsGroupingEnabled && HasItems;

            var itemsChangedMsg = new ItemsChangedNotification();
            Messenger.Default.Send(itemsChangedMsg);


        }

        public async void RemoveFolder(string folderName)
        {
            var items = Items.Where(i => i.FolderName == folderName).ToArray();

            foreach (var item in items)
                Items.Remove(item);

            await Save();

            ToastService.DisplayMessage($"Removed {items.Count()} MIDIs from playlist.", MessageType.Success);
        }

        private  void AppendLog(string processName, string text)
        {
            var msg = new AddLogNotification() { ServiceName = processName, Text = text };

            Messenger.Default.Send(msg);
        }
        #endregion

    }
}
