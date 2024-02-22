using Hscm.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI
{
    public class MainWindowPlayerToolbarViewModel : ObservableViewModel
    {

        private bool showPlayButton;
        private bool showPauseButton;

        private bool showLoopAllButton;
        private bool showLoopOneButton;

        private bool isVisible;
        private bool stopButtonEnabled;

        private bool isDropDownsEnabled;

        private bool showToggleTracksButton;
        private bool showToggleSongSettingsButton;

        private bool showReloadButton;

        public MainWindowPlayerToolbarViewModel()
        {
            this.ShowPlayButton = true;
            this.ShowPauseButton = false;
            this.ShowLoopOneButton = true;
            this.ShowLoopAllButton = false;
            this.ShowToggleSongSettingsButton = false;
            this.IsVisible = false;
        }
        public bool IsDropDownsEnabled
        {
            get { return isDropDownsEnabled; }
            set
            {
                isDropDownsEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowToggleSongSettingsButton { get => showToggleSongSettingsButton; set { showToggleSongSettingsButton = value; RaisePropertyChanged(); } }
        public bool ShowToggleTracksButton { get => showToggleTracksButton; set { showToggleTracksButton = value; RaisePropertyChanged(); } }

        public bool ShowLoopAllButton { get => showLoopAllButton; set { showLoopAllButton = value; RaisePropertyChanged(); } }
        public bool ShowLoopOneButton { get => showLoopOneButton; set { showLoopOneButton = value; RaisePropertyChanged(); } }

        public bool ShowPlayButton { get => showPlayButton; set { showPlayButton = value; RaisePropertyChanged(); } }
        public bool ShowPauseButton { get => showPauseButton; set { showPauseButton = value; RaisePropertyChanged(); } }

        public bool IsVisible { get => isVisible; set { isVisible = value; RaisePropertyChanged(); } }

        public bool StopButtonEnabled { get => stopButtonEnabled; set { stopButtonEnabled = value; RaisePropertyChanged(); } }

        public bool ShowReloadButton { get => showReloadButton; set { showReloadButton = value; RaisePropertyChanged(); } }
    }
}
