
namespace Hscm.UI
{
    public class PlaylistMenuViewModel : ObservableViewModel
    {
        private bool showPlay;
        private bool showAddFiles;
        private bool showAddDirectory;

        private bool showOpen;
        private bool showSave;
        private bool showSaveAs;

        private bool showRemove;
        private bool showRemoveAll;

        private bool showTracks;

        private bool showRefresh;

        public PlaylistMenuViewModel()
        {
            this.ShowPlay = false;
            this.ShowAddFiles = true;
            this.ShowAddDirectory = true;
            this.ShowOpen = true;
            this.ShowSave = false;
            this.ShowSaveAs = false;
            this.ShowRemove = false;
            this.ShowRemoveAll = false;
            this.ShowTracks = false;
            this.ShowRefresh = true;
        }

        public bool ShowRefresh { get => showRefresh; set { showRefresh = value; RaisePropertyChanged(); } }
        public bool ShowTracks { get => showTracks; set { showTracks = value; RaisePropertyChanged(); } }
        public bool ShowPlay { get => showPlay; set { showPlay = value; RaisePropertyChanged(); } }
        public bool ShowAddFiles { get => showAddFiles; set { showAddFiles = value; RaisePropertyChanged(); } }
        public bool ShowAddDirectory { get => showAddDirectory; set { showAddDirectory = value; RaisePropertyChanged(); } }
        public bool ShowOpen { get => showOpen; set { showOpen = value; RaisePropertyChanged(); } }
        public bool ShowSave { get => showSave; set { showSave = value; RaisePropertyChanged(); } }
        public bool ShowSaveAs { get => showSaveAs; set { showSaveAs = value; RaisePropertyChanged(); } }
        public bool ShowRemove { get => showRemove; set { showRemove = value; RaisePropertyChanged(); } }
        public bool ShowRemoveAll { get => showRemoveAll; set { showRemoveAll = value; RaisePropertyChanged(); } }
    }
}
