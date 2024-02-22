
using Common;
using Common.Models.Playlist;
using Common.Music;
using Hscm.UI.Notifications;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hscm.UI
{
    public class PlaylistItemViewModel : ObservableViewModel<PlaylistItem>
    {

        private bool isSelected;

        private string titleHeader;

        private string imagePath;
        private bool expanded;
        private string tooltipText;

        public PlaylistItemViewModel(PlaylistItem item) : base(item)
        {
            IsFiltered = false;
        }

        ~PlaylistItemViewModel()
        {

        }

        public MidiSequence Sequence
        {
            get { 
                return Model.Sequence; 
            }
            set
            {
                Model.Sequence = value;
            }
        }

        public void UpdateProperties()
        {
            RaisePropertyChanged(nameof(this.Title));
            RaisePropertyChanged(nameof(this.Duration));
            RaisePropertyChanged(nameof(this.FolderName));
            RaisePropertyChanged(nameof(this.FileName));
            RaisePropertyChanged(nameof(this.FolderPath));
            RaisePropertyChanged(nameof(this.Size));
            RaisePropertyChanged(nameof(this.Index));
        }


        public bool IsFiltered { 
            get => Model.IsFiltered; 
            set => Model.IsFiltered = value; }

        public string Title => Model.Sequence.Info.Title;

        public string Duration
        {
            get { return Model.Sequence.Duration.ToString(); }
        }

        public string ToolTipText => Model.Sequence.InfoText();

        public string Size
        {
            get { return Model.Sequence.Info.FileSize.ToByteSize(); }
        }

        public string FilePath
        {
            get { return Model.Sequence.Info.FilePath; }
        }

        public string FolderPath
        {
            get { return Path.GetDirectoryName(Model.Sequence.Info.FilePath); }
        }

        public string FolderName
        {
            get { return new DirectoryInfo(Path.GetDirectoryName(Model.Sequence.Info.FilePath)).Name; }
        }

        public string FileName
        {
            get { return Path.GetFileName(Model.Sequence.Info.FilePath); }
        }


        public bool IsExpanded
        {
            get { return expanded; }
            set
            {
                expanded = value;
                RaisePropertyChanged();
            }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set { 
                this.isSelected = value; 
                RaisePropertyChanged(); 
            }
        }

        public bool IsPlaying
        {
            get { return Model.IsPlaying; }
            set
            {
                Model.IsPlaying = value;
                RaisePropertyChanged();
            }
        }

        public int Index { 
            get => Model.Index;
            set { Model.Index = value; UpdateImage(); RaisePropertyChanged(); RaisePropertyChanged("No"); } 
        }

        public int No => Index + 1;

        public string TitleHeader
        {
            get { return titleHeader; }
            set
            {
                titleHeader = value;
                RaisePropertyChanged();
            }
        }

        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                imagePath = value;
                RaisePropertyChanged();
            }
        }

  
        private void UpdateImage()
        {
            int index = Index + 1;

            if (Index == 0)
            {
                ImagePath = "../../Images/icons8-musical-notes-48-2.png";
                return;
            }

            if (index % 5 == 0)
            {
                ImagePath = "../../Images/icons8-musical-notes-48-5.png";
                return;
            }

            if (index % 4 == 0)
            {
                ImagePath = "../../Images/icons8-musical-notes-48-4.png";
                return;
            }

            if (index % 3 == 0)
            {
                ImagePath = "../../Images/icons8-musical-notes-48-3.png";
                return;
            }

            ImagePath = "../../Images/icons8-musical-notes-48-2.png";
        }


        public RelayCommand<object> GroupExcludeCommand { get { return new RelayCommand<object>(ExecuteGroupExcludeCommand); } }

        public RelayCommand<object> GroupIncludeCommand { get { return new RelayCommand<object>(ExecuteGroupIncludeCommand); } }

        public RelayCommand<object> GroupRemoveCommand { get { return new RelayCommand<object>(ExecuteGroupRemoveCommand); } }

        private void ExecuteGroupExcludeCommand(object args)
        {
            var routedEventArgs = args as RoutedEventArgs;
        }


        private void ExecuteGroupIncludeCommand(object args)
        {
            var routedEventArgs = args as RoutedEventArgs;
        }

        private void ExecuteGroupRemoveCommand(object args)
        {
            var routedEventArgs = args as RoutedEventArgs;
        }

    }
}
