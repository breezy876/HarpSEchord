using Common;
using Common.Helpers;
using Common.Midi;
using Common.Models.Playlist;
using Common.Models.Settings;
using Common.Music;
using Common.Playlist;
using Hscm.Helpers;
using Hscm.UI.Controls;
using Hscm.UI.Notifications;
using Hscm.UI.Notifications.Playlist;
using Hscm.UI.Notifications.Window;
using Hscm.UI.ViewModels.MainWindow;
using Hscm.UI.ViewModels.Settings;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Path = System.IO.Path;
using Hscm.UI.Services;
using Hscm.UI.Notifications.Tracks;

namespace Hscm.UI
{
    /// <summary>
    /// Interaction logic for PlaylistControl.xaml
    /// </summary>
    public partial class PlaylistControl : StackPanel
    {

        private enum PlaylistSortType { No, Title }

        private const string PlaylistDefaultFileName = "playlist.pl";

        PlaylistViewModel viewModel;

        private MidiProcessor midiProcessor;

        private string filterText;

        private int totalProcessed;
        private int totalItems;
        private int totalFiltered;

        private bool filterApplied;

        private ListCollectionView itemsView;

        private ObservableCollection<GroupDescription> groups;

        private PlaylistSortType sortType;

        private int totalNew;

        private PlaylistWatcher watcher;

        public PlaylistControl()
        {
            InitializeComponent();
        }

        public event EventHandler FileAddStarted;

        public event EventHandler<FileAddCompleteEventArgs> FileAddComplete;


        #region public methods
        public async Task Initialize(PlaylistViewModel viewModel, PlaylistSettingsViewModel settingsViewModel, MidiProcessor midiProcessor)
        {
            this.viewModel = viewModel;
            this.DataContext = this.viewModel;
            this.midiProcessor = midiProcessor;

            Messenger.Default.Register<ChangePlaylistSongNotification>(this, ChangePlaylistSongNotificationReceived);
            Messenger.Default.Register<AddPlaylistFilesNotification>(this, AddPlaylistFilesNotificationReceived);
            Messenger.Default.Register<AddPlaylistDirectoryNotification>(this, AddPlaylistDirectoryNotificationReceived);
            Messenger.Default.Register<SavePlaylistAsNotification>(this, SavePlaylistAsNotificationReceived);
            Messenger.Default.Register<SavePlaylistNotification>(this, SavePlaylistNotificationReceived);
            Messenger.Default.Register<OpenPlaylistNotification>(this, OpenPlaylistNotificationReceived);
            Messenger.Default.Register<ChangePlaylistSongNotification>(this, ChangePlaylistSongNotificationReceived);
            Messenger.Default.Register<RefreshPlaylistNotification>(this, RefreshPlaylistNotificationReceived);
            Messenger.Default.Register<PlaylistDropNotification>(this, PlaylistDropNotificationReceived);
            Messenger.Default.Register<ClearPlaylistFilterNotification>(this, ClearPlaylistFilterNotificationReceived);
            Messenger.Default.Register<ExpandGroupsNotification>(this, ExpandGroupsNotificationReceived);
            Messenger.Default.Register<CollapseGroupsNotification>(this, CollapseGroupsNotificationReceived);
            Messenger.Default.Register<ItemsChangedNotification>(this, ItemsChangedReceived);
            Messenger.Default.Register<ToggleGroupsNotification>(this, ToggleGroupsNotificationReceived);
            Messenger.Default.Register<NewPlaylistNotification>(this, NewPlaylistNotificationReceived);

            playlist.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(productsDataGrid_PreviewMouseLeftButtonDown);
            playlist.Drop += new DragEventHandler(productsDataGrid_Drop);

            this.playlistSettingsControl.Initialize(settingsViewModel);

            if (Common.Settings.AppSettings.PlaylistSettings.LoadPrevPlaylist)
                await OpenPrevPlaylist();

            if (string.IsNullOrEmpty(Common.Settings.AppSettings.PrevPlaylistFileName))
                Common.Settings.AppSettings.PrevPlaylistFileName = GetDefaultPlaylistFilePath();

            filterText = string.Empty;

            itemsView = (ListCollectionView)CollectionViewSource.GetDefaultView(playlist.ItemsSource);

            groups = new ObservableCollection<GroupDescription>(itemsView.GroupDescriptions.ToList());

            sortType = PlaylistSortType.Title;

            ToggleGroups();


        }

        private async void SavePlaylistNotificationReceived(SavePlaylistNotification obj)
        {
            await SavePlaylistAs(viewModel.FullPath);
        }


        public void PlayCurrent()
        {
            if (!Common.Settings.Playlist.HasItems)
                return;

            var curItem = viewModel.Model.GetCurrent();

            if (curItem == null)
                return;

            viewModel.Change(curItem.Title, true);
        }

        public new void UpdateLayout()
        {
            playlist.UpdateLayout();

            //if (viewModel.Items.Any())
            //playlist.ScrollIntoView(viewModel.Items.First());
        }

        public void Clear()
        {
            Common.Settings.Playlist.Clear();
            Common.Settings.PlaylistSettings.Clear();
            viewModel.Clear();
        }

        public async Task Save()
        {
            await viewModel.Save();
        }

        private async Task Save(Common.Models.Playlist.Playlist playlist, string filePath)
        {
            await viewModel.Save(playlist, filePath);
        }

        public async Task OpenPrevPlaylist()
        {
            var defPlaylistPath = GetDefaultPlaylistFilePath();

            var prevPlaylistPath = Settings.AppSettings.PrevPlaylistFileName;

            var filePath = Path.Combine(AppHelpers.GetAppAbsolutePath(), !string.IsNullOrEmpty(prevPlaylistPath) ? prevPlaylistPath : defPlaylistPath);

            await OpenPlaylist(filePath, Settings.AppSettings.PlaylistSettings.LoadPlaylistSettings);
        }

        public async Task OpenFiles(List<string> filePaths)
        {

            var filePathList = new List<string>();

            this?.FileAddStarted(this, EventArgs.Empty);

            foreach (var filePath in filePaths)
            {
                if (Directory.Exists(filePath))
                    await EnumerateFiles(filePath, filePathList);
                else
                {
                    if (File.Exists(filePath))
                        filePathList.Add(filePath);
                }
            }

            var existingFiles = this.viewModel.Model.Files;

            await ProcessFiles(filePathList.Where(p => !existingFiles.Contains(p)), false);
        }
        #endregion


        #region notifications
        private void ChangePlaylistSongNotificationReceived(ChangePlaylistSongNotification msg)
        {
            if (msg.Sequence == null)
                return;

            this.viewModel.Change(msg.Sequence.Info.Title);

            var row = playlist.Items.Cast<PlaylistItemViewModel>().Where(i => i.IsPlaying);

            this.playlist.ScrollIntoView(row);
        }


        private void ToggleGroupsNotificationReceived(ToggleGroupsNotification obj)
        {
            ToggleGroups();
            SaveSettings();

        }

        private void ItemsChangedReceived(ItemsChangedNotification obj)
        {
            //for when grouping is needed

            Common.Settings.Playlist.Items.Clear();

            foreach (var item in viewModel.Items)
            {
                Common.Settings.Playlist.Items.Add(item.Model);
            }


            //Settings.Playlist.Items = viewModel.Items;
            //playlist.ItemsSource = itemsView;

        }

        private void ExpandGroupsNotificationReceived(ExpandGroupsNotification obj)
        {
            var expanders = Helpers.UIHelpers.GetVisualChildren<Expander>(playlist);
            foreach (var expander in expanders)
            {
                this.playlist.ScrollIntoView(expander, null);
                expander.IsExpanded = true;
            }
        }


        private void CollapseGroupsNotificationReceived(CollapseGroupsNotification obj)
        {


            var expanders = Helpers.UIHelpers.GetVisualChildren<Expander>(playlist);
            foreach (var expander in expanders)
            {
                this.playlist.ScrollIntoView(expander, null);
                expander.IsExpanded = false;
            }

        }

        private void ClearPlaylistFilterNotificationReceived(ClearPlaylistFilterNotification msg)
        {
            ClearFilter();
        }

        private async void PlaylistDropNotificationReceived(PlaylistDropNotification msg)
        {
            if (Path.GetExtension(msg.FileNames.First()).Equals(".pl"))
            {
                await OpenPlaylist(msg.FileNames.First(), true);
                return;
            }

            var dirs = msg.FileNames.Where(f => FileHelpers.IsDirectory(f));
            var files = msg.FileNames.Where(f => !FileHelpers.IsDirectory(f));

            foreach (var dir in dirs)
            {
                await OpenDirectory(dir);
            }

            await OpenFiles(files.ToList());
        }

        private async void RefreshPlaylistNotificationReceived(RefreshPlaylistNotification msg)
        {
            if (!string.IsNullOrEmpty(Settings.AppSettings.PrevPlaylistPath))
                await this.OpenDirectory(Paths.MidiFilePath);
        }


        private async void NewPlaylistNotificationReceived(NewPlaylistNotification obj)
        {
            var saveFileDialog = new CommonSaveFileDialog();
            saveFileDialog.Title = "Save Playlist";
            saveFileDialog.Filters.Add(new CommonFileDialogFilter("Playlist files", "*.pl"));
            saveFileDialog.DefaultExtension = "pl";
            saveFileDialog.DefaultFileName = $"playlist{++totalNew}";
            saveFileDialog.InitialDirectory = Common.Settings.AppSettings.PrevPlaylistPath.ToAbsolutePath();
            saveFileDialog.DefaultDirectory = Common.Settings.AppSettings.PrevPlaylistPath.ToAbsolutePath();

            var result = saveFileDialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                Clear();

                await SavePlaylistAs(saveFileDialog.FileName);
            }
        }

        private async void OpenPlaylistNotificationReceived(OpenPlaylistNotification msg)
        {
            //if (Common.Settings.Playlist.Items.Count >= 100)
            //{
            //    AppendLog("", "Error: basic version can only load a maximum of 100 MIDIs");
            //    return;
            //}

            var openFileDialog = new CommonOpenFileDialog();
            openFileDialog.Title = "Open Playlist";
            openFileDialog.Filters.Add(new CommonFileDialogFilter("Playlist files", "*.pl"));
            openFileDialog.DefaultExtension = "pl";
            openFileDialog.InitialDirectory = Common.Settings.AppSettings.PrevPlaylistPath.ToAbsolutePath();
            openFileDialog.DefaultDirectory = Common.Settings.AppSettings.PrevPlaylistPath.ToAbsolutePath();

            var result = openFileDialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                Common.Settings.AppSettings.PrevPlaylistPath = Common.Helpers.AppHelpers.GetAppRelativePath(Path.GetDirectoryName(openFileDialog.FileName));
                await this.OpenPlaylist(openFileDialog.FileName, true);

                await viewModel.SaveToPluginPath();
                await viewModel.SaveSettingsToPluginPath();

                var notification = new UpdateSelectedSequenceNotification();
                Messenger.Default.Send(notification);

            }
        }

        private async void SavePlaylistAsNotificationReceived(SavePlaylistAsNotification msg)
        {
            var saveFileDialog = new CommonSaveFileDialog();
            saveFileDialog.Title = "Save Playlist";
            saveFileDialog.Filters.Add(new CommonFileDialogFilter("Playlist files", "*.pl"));
            saveFileDialog.DefaultExtension = "pl";
            saveFileDialog.DefaultFileName = this.viewModel.Title;
            saveFileDialog.InitialDirectory = Common.Settings.AppSettings.PrevPlaylistPath.ToAbsolutePath();
            saveFileDialog.DefaultDirectory = Common.Settings.AppSettings.PrevPlaylistPath.ToAbsolutePath();

            var result = saveFileDialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)

                await SavePlaylistAs(saveFileDialog.FileName);
        }

        private async void AddPlaylistFilesNotificationReceived(AddPlaylistFilesNotification msg)
        {
            var openFileDialog = new CommonOpenFileDialog();
            openFileDialog.Title = "Select MIDI Files";
            openFileDialog.Filters.Add(new CommonFileDialogFilter("MIDI files", "*.midi; *.mid"));
            openFileDialog.Multiselect = true;
            openFileDialog.DefaultExtension = "mid";
            openFileDialog.InitialDirectory = Common.Settings.AppSettings.PrevMidiPath.ToAbsolutePath();
            openFileDialog.DefaultDirectory = Common.Settings.AppSettings.PrevMidiPath.ToAbsolutePath();

            var result = openFileDialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                if (openFileDialog.FileNames.IsNullOrEmpty())
                    return;

                Common.Settings.AppSettings.PrevMidiPath = AppHelpers.GetAppRelativePath(Path.GetDirectoryName(openFileDialog.FileNames.First()));

                await OpenFiles(openFileDialog.FileNames.ToList());
            }
        }

        private async void AddPlaylistDirectoryNotificationReceived(AddPlaylistDirectoryNotification msg)
        {
            var openFolderDialog = new CommonOpenFileDialog();
            openFolderDialog.Title = "Select Folder";
            openFolderDialog.IsFolderPicker = true;

            openFolderDialog.InitialDirectory = Common.Settings.AppSettings.PrevMidiPath.ToAbsolutePath();
            openFolderDialog.DefaultDirectory = Common.Settings.AppSettings.PrevMidiPath.ToAbsolutePath();

            var result = openFolderDialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                Common.Settings.AppSettings.PrevMidiPath = AppHelpers.GetAppRelativePath(openFolderDialog.FileName);

                await OpenDirectory(openFolderDialog.FileName);
            }
        }
        #endregion



        #region private methods

        private void ToggleGroups()
        {
            if (this.viewModel.IsGroupingEnabled)
            {
                itemsView.GroupDescriptions.Clear();

                foreach (var group in groups)
                    itemsView.GroupDescriptions.Add(group);

                itemsView.Refresh();
                //this.playlist.ItemsSource = itemsView;
            }
            else
            {
                itemsView.GroupDescriptions.Clear();
                itemsView.Refresh();
            }

            viewModel.HandleGroupsToggled();
        }

        #region filter
        private void ClearFilter()
        {
            foreach (var item in this.viewModel.Items)
            {
                item.IsFiltered = false;
            }

            ApplyFilter("", false);

            this.viewModel.ClearFilter();

            //Common.Settings.Playlist.ClearFilter();
        }

        private void ApplyFilter(string text, bool filterApplied = true)
        {

            this.filterText = text;

            totalItems = this.viewModel.Items.Count;
            totalProcessed = 0;
            totalFiltered = 0;

            this.filterApplied = filterApplied;

            this.RefreshFilter();
        }

        private void RefreshFilter()
        {
            var source = CollectionViewSource.GetDefaultView(this.playlist.ItemsSource);

            if (source != null)
                source.Refresh();
        }
        #endregion

        private string GetDefaultPlaylistFilePath()
        {
            return Path.Combine($"Playlists", PlaylistDefaultFileName);
        }

        private async Task SavePlaylist(string filePath, string settingsFile)
        {
            try
            {
                string title = Path.GetFileNameWithoutExtension(filePath);

                viewModel.Title = title;

                var model = viewModel.GetPlaylistModel(title, settingsFile);

                await Save(model, filePath);

                ToastService.DisplayMessage($"Playlist saved.", MessageType.Success);
            }
            catch
            {

            }
        }

        private async Task LoadPlaylistSettings(Common.Models.Playlist.Playlist playlist)
        {


            if (string.IsNullOrEmpty(playlist.SettingsFile))
                return;

            string settingsFile = Path.Combine(AppHelpers.GetAppAbsolutePath(), playlist.SettingsFile);

            var playlistSettings = await Task.Run(() => FileHelpers.Load<SongSettings>(settingsFile));

            if (playlistSettings != null)
                Common.Settings.PlaylistSettings = playlistSettings;
        }

        public async Task OpenPlaylist(string playlistFilePath, bool loadSettings = true)
        {

            Common.Settings.Playlist.Title = Path.GetFileNameWithoutExtension(playlistFilePath);

            if (!File.Exists(playlistFilePath))
                return;

            Settings.AppSettings.PrevPlaylistFileName = AppHelpers.GetAppRelativePath(playlistFilePath);

            this.viewModel.FilePath = System.IO.Path.GetDirectoryName(playlistFilePath);

            this.FileAddStarted(this, EventArgs.Empty);

            var playlist = await Task.Run(() => FileHelpers.Load<Common.Models.Playlist.Playlist>(playlistFilePath));

            if (playlist == null)
                return;

            Clear();
            Common.Settings.Playlist = playlist;

            if (loadSettings)
                await LoadPlaylistSettings(playlist);

            await ProcessFiles(Common.Settings.Playlist.Files, true);

            viewModel.Title = playlist.Title;

            SaveSettings();
        }

        private void SaveSettings()
        {
            var itemsChangedMsg = new SaveSettingsNotification() { SaveAppSettings = true };
            Messenger.Default.Send(itemsChangedMsg);
        }

        private async Task SavePlaylistAs(string fileName)
        {

            string settingsFile = AppHelpers.GetAppRelativePath(fileName.Replace(".pl", ".settings.json"));

            Common.Settings.AppSettings.PrevPlaylistPath = Common.Helpers.AppHelpers.GetAppRelativePath(Path.GetDirectoryName(fileName));

            Common.Settings.AppSettings.PrevPlaylistFileName = Common.Helpers.AppHelpers.GetAppRelativePath(fileName);

            Common.Settings.Playlist.SettingsFile = settingsFile;
            //var itemsChangedMsg = new ChangeSettingsFileNotification() { FilePath = AppHelpers.GetAppRelativePath(settingsFile) };
            //Messenger.Default.Send(itemsChangedMsg);

            await SavePlaylistSettings(null, settingsFile);

            await SavePlaylist(fileName, settingsFile);

            ToastService.DisplayMessage($"Saved playlist to '{fileName}'.", MessageType.Success);
            SaveSettings();

        }

        private async Task SavePlaylistSettings(string title = null, string filePath = null)
        {
            await viewModel.SavePlaylistSettings(title, filePath);
        }

        private async Task OpenDirectory(string path)
        {
            var filePaths = new List<string>();

            this?.FileAddStarted(this, EventArgs.Empty);

            await EnumerateFiles(path, filePaths);

            await ProcessFiles(filePaths, false);

            Settings.AppSettings.PrevPlaylistPath = Common.Helpers.AppHelpers.GetAppRelativePath(path);
        }

        private async Task<MidiSequence> LoadMidiFileWorker(ProgressReporter reporter, string filePath)
        {
            var appPath = AppHelpers.GetAppAbsolutePath();

            var task = await this.midiProcessor.LoadMidiFile(Path.Combine(appPath, filePath), false, true);

            //reporter.Update();

            //var msg = new ReportProgressNotification() { ProgressInfo = new ProgressInfo("Loading MIDIs...", reporter.PercentComplete) };
            //Messenger.Default.Send(msg);

            return task.sequence;
        }


        private void AddToPlaylist(MidiSequence[] sequences, bool loadingPlaylist = false)
        {
            //Parallel.ForEach(sequences, seq =>
            //{
            //    if (seq != null)
            //        this.viewModel.Add(seq);
            //});

            //var seqWithSettings = sequences.Where(seq => Common.Settings.PlaylistSettings.Settings.ContainsKey(seq.Info.Title);

            int index = 0;

            foreach (var seq in sequences)
            {
                //seq.Index = i;
                //i++;

                if (Common.Settings.PlaylistSettings.Settings.ContainsKey(seq.Info.Title))
                {
                    var playlistSeq = Common.Settings.PlaylistSettings.Settings[seq.Info.Title];

                    //sequences = sequences.OrderBy(seq => settings.Settings[seq.Info.Title].Index).ToArray();

                    if (playlistSeq != null)//add from playlist settings
                    {
                        var newSeq = new MidiSequence(seq);
                        if (!playlistSeq.Tracks.IsNullOrEmpty())
                            newSeq.Tracks = playlistSeq.Tracks;
                        if (!this.viewModel.Items.Any(i => i.Title.ToLower().Equals(newSeq.Info.Title.ToLower())))
                            this.viewModel.Add(newSeq, index, false);
                    }

                }
                //else
                if (!this.viewModel.Items.Any(i => i.Title.ToLower().Equals(seq.Info.Title.ToLower())))
                    this.viewModel.Add(seq, index, false); //no playlist settings
                index++;
            }
        }

        private string CreateDirectoryIfNotExists(string filePath)
        {;
            var newPath = GetRelativeMidiFilePath(filePath);

            Debug.WriteLine($"Creating directory '{newPath}'");

            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);

            return newPath;
        }

        private string GetRelativeMidiFilePath(string filePath)
        {
            var paths = filePath.Split(new[] { '\\' });
            string newPath = $"{Paths.MidiFilePath}\\{string.Join("\\", paths.Skip(1).ToArray())}";

            return newPath;
        }

        private async Task<string[]> ProcessMidiFiles(IEnumerable<string> filePaths)
        {
            var tasks = new List<Task<string>>();

            foreach (var filePath in filePaths)
            {
                var task = ProcessMidiFile(filePath);
                tasks.Add(task);
            }

            return await Task.WhenAll(tasks);
        }

        private async Task<string> ProcessMidiFile(string filePath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (FileNotInMidiDirectory(filePath))
                    {
                        Debug.WriteLine($"File '{filePath}' does not exist in MIDI directory. Copying.");

                        string fileName = Path.GetFileName(filePath);
                        string newPath = CreateDirectoryIfNotExists(Path.GetDirectoryName(filePath));
                        string fullPath = Path.Combine(newPath, fileName);

                        if (!File.Exists(fullPath))
                        {
                            Debug.WriteLine($"Copying '{filePath}' to '{fullPath}'");
                            File.Copy(filePath, fullPath);
                        }
                        return fullPath;
                    }
                    return filePath;
                }
                catch (Exception ex)
                {
                    return null;
                }
            });
        }

        private bool FileNotInMidiDirectory(string filePath) => !filePath.StartsWith(Path.Combine(AppHelpers.GetAppAbsolutePath(), Paths.MidiFilePath));


        //private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
        //{
        //    if (!IsFileReady(e.FullPath)) return; //first notification the file is arriving

        //    //The file has completed arrived, so lets process it
        //    DoWorkOnFile(e.FullPath);
        //}
        //#endregion

        private async Task<MidiSequence[]> LoadMidiFiles(IEnumerable<string> filePaths)
        {
            var tasks = new List<Task<MidiSequence>>();


            var existingFilePaths = filePaths.Where(p => File.Exists(p));

            var reporter = new ProgressReporter(filePaths.Count());

            foreach (var filePath in existingFilePaths)
            {

                //if file does not exist, copy over
                //if (!File.Exists(filePath))
                //    File.Copy(filePath, Paths.MidiFilePath);

                var task = LoadMidiFileWorker(reporter, filePath);

                tasks.Add(task);
            }

            return await Task.WhenAll(tasks);
        }

        private async Task ProcessFiles(IEnumerable<string> filePaths, bool loadingPlaylist = false)
        {

            var showMsg = new ShowProgressNotification();
            var hideMsg = new HideProgressNotification();

            //Messenger.Default.Send(showMsg);
            var settings = Common.Settings.PlaylistSettings;

            if (!loadingPlaylist)
                filePaths = (await ProcessMidiFiles(filePaths)).Where(f => !string.IsNullOrEmpty(f));

            var sequences = await LoadMidiFiles(filePaths);

            //Messenger.Default.Send(hideMsg);

            AddToPlaylist(sequences, loadingPlaylist);

            var settingsFile = Common.Settings.Playlist.SettingsFile;

            //Common.Settings.Playlist = this.viewModel.Model;

            var notification = new ChangeSettingsFileNotification() { FilePath = settingsFile };
            Messenger.Default.Send(notification);

            //var updateMsg = new UpdateSelectedSequenceNotification();
            //Messenger.Default.Send(updateMsg);

            if (!loadingPlaylist)
            ToastService.DisplayMessage($"Added {sequences.Count()} MIDIs to playlist.", MessageType.Success);

            this?.FileAddComplete(this, new FileAddCompleteEventArgs() { LoadingPlaylist = loadingPlaylist });

            //UpdateLengths(sequences);

        }

        private void UpdateLengths(IEnumerable<MidiSequence> sequences)
        {
            //Parallel.ForEach(Common.Settings.Playlist.Items, item =>
            //{
            //    this.viewModel.UpdateDuration(item.Title, item.Sequence.Duration);

            //    //var msg = new UpdatePlaylistItemDurationNotification() { Title = item.Title, Duration = item.Sequence.Duration };
            //    //Messenger.Default.Send(msg);
            //});
        }

        private async Task EnumerateFiles(string path, List<string> filePaths)
        {
            await Task.Run(() => {

                LoadFiles(path, filePaths);

                var dirs = Directory.GetDirectories(path);

                foreach (string dir in dirs)
                {
                    TraverseDirectory(dir, filePaths);
                }
            });
        }

        private void TraverseDirectory(string path, List<string> filePaths)
        {
            LoadFiles(path, filePaths);

            var dirs = Directory.GetDirectories(path);

            foreach (string dir in dirs)
            {
                TraverseDirectory(dir, filePaths);
            }
        }

        private void LoadFiles(string dir, List<string> filePathList)
        {
            var filePaths = Directory.GetFiles(dir).Where(f => this.IsValidPlaylistFile(f)).ToList();

            var existingFiles = this.viewModel.Model.Files;

            foreach (var filePath in filePaths.Where(p => !existingFiles.Contains(p)))
            {
                filePathList.Add(filePath);
            }
        }

        private bool IsValidPlaylistFile(string fileName)
        {
            return System.IO.Path.GetExtension(fileName).ToLower() == ".mid" ||
                System.IO.Path.GetExtension(fileName).ToLower() == ".midi";
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        #region events
        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            if (!filterApplied || string.IsNullOrEmpty(filterText))
                return;

            int index = (e.Item as PlaylistItemViewModel).Index;

            var playlistItem = this.viewModel.GetByIndex(index);

            if (playlistItem == null)
                return;

            //if (playlistItemVm.FolderPath.ToLower().Contains(filterText.ToLower()) || playlistItemVm.Title.ToLower().Contains(filterText.ToLower()))
            if (playlistItem.Title.ToLower().Contains(filterText.ToLower()))
            {
                e.Accepted = true;
                playlistItem.IsFiltered = true;
                totalFiltered++;
            }
            else
            {
                e.Accepted = false;
                playlistItem.IsFiltered = false;
            }

            totalProcessed++;

            if (totalProcessed == totalItems)
                this.viewModel.ApplyFilter();

        }

        private void filterText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(filter.Text))
                this.ClearFilter();

            if (filter.Text != "Enter filter text...")
                ApplyFilter(filter.Text, true);
        }

        private void filterText_GotFocus(object sender, RoutedEventArgs e)
        {
            if (filter.Text == "Enter filter text...")
                filter.Text = "";
        }

        private void filterText_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (filter.Text == "Enter filter text...")
                filter.Text = "";
        }
        #endregion


        private void playlist_Sorting(object sender, DataGridSortingEventArgs e)
        {
            //var colName = e.Column.Header.ToString();

            //switch(colName)
            //{
            //    case "#":
            //        sortType = PlaylistSortType.No;
            //        break;

            //    case "Title":
            //        sortType = PlaylistSortType.Title;
            //        break;
            //}

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var expander = Helpers.UIHelpers.GetAnscestorByType(e.Source as Control, typeof(Expander));
        }



        public delegate Point GetPosition(IInputElement element);

        int rowIndex = -1;

        async void productsDataGrid_Drop(object sender, DragEventArgs e)
        {
            if (viewModel.IsGroupingEnabled)
                return;

            DoingDragDrop = true;

            if (rowIndex < 0)
                return;
            int index = this.GetCurrentRowIndex(e.GetPosition);
            if (index < 0)
                return;
            if (index == rowIndex)
                return;
            if (index == playlist.Items.Count - 1)
            {
                MessageBox.Show("This row-index cannot be drop");
                return;
            }
            var changedItem = viewModel.Items[rowIndex];
            viewModel.Items.RemoveAt(rowIndex);
            viewModel.Items.Insert(index, changedItem);

           await Save();
        }

        public bool DoingDragDrop { get; private set; }

        void productsDataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (viewModel.IsGroupingEnabled)
                    return;

                DoingDragDrop = false;

                rowIndex = GetCurrentRowIndex(e.GetPosition);
                if (rowIndex < 0)
                    return;
                playlist.SelectedIndex = rowIndex;
                var selectedItem = viewModel.Items[rowIndex] as PlaylistItemViewModel;
                if (selectedItem == null)
                    return;
                DragDropEffects dragdropeffects = DragDropEffects.Move;
                if (DragDrop.DoDragDrop(playlist, selectedItem, dragdropeffects)
                                    != DragDropEffects.None)
                {
                    playlist.SelectedItem = selectedItem;
                }
            }
            catch { }
        }

        private bool GetMouseTargetRow(Visual theTarget, GetPosition position)
        {
            try
            {
                Rect rect = VisualTreeHelper.GetDescendantBounds(theTarget);
                Point point = position((IInputElement)theTarget);
                return rect.Contains(point);
            }
            catch { return false; }
        }

        private DataGridRow GetRowItem(int index)
        {
            if (playlist.ItemContainerGenerator.Status
                    != GeneratorStatus.ContainersGenerated)
                return null;
            return playlist.ItemContainerGenerator.ContainerFromIndex(index)
                                                            as DataGridRow;
        }

        private int GetCurrentRowIndex(GetPosition pos)
        {
            int curIndex = -1;
            for (int i = 0; i < playlist.Items.Count; i++)
            {
                DataGridRow itm = GetRowItem(i);
                if (GetMouseTargetRow(itm, pos))
                {
                    curIndex = i;
                    break;
                }
            }
            return curIndex;
        }
        #endregion

        private void GroupItemRemoveMenu_Click(object sender, RoutedEventArgs e)
        {

            var parent = UIHelpers.GetTemplatedAnscestorByType(sender as FrameworkElement, typeof(GroupItem)) as GroupItem;
            var col = parent.Content as CollectionViewGroup;
            string folderName = (col.Items.First() as PlaylistItemViewModel).FolderName;

            viewModel.RemoveFolder(folderName);
        }
    }
}
