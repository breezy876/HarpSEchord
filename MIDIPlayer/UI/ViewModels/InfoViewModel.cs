using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI.ViewModels
{
    public class InfoViewModel : ObservableViewModel
    {

        private string noteRangeText;
        private string bpmText;
        private string divisionText;
        private string trackCountText;
        private string noteCountText;
        private string eventCountText;
        private string statusText;

        private bool spinnerVisible;
        private string lengthText;

        public InfoViewModel() : base()
        {
            SpinnerVisible = false;
        }

        public bool SpinnerVisible
        {
            get
            {
                return spinnerVisible;
            }
            set
            {
                spinnerVisible = value;
                RaisePropertyChanged();
            }
        }

        public string StatusText
        {
            get
            {
                return statusText;
            }
            set
            {
                statusText = value;
                RaisePropertyChanged();
            }
        }

        public string LengthText
        {
            get
            {
                return lengthText;
            }
            set
            {
                lengthText = value;
                RaisePropertyChanged();
            }
        }

        public string NoteRangeText
        {
            get
            {
                return noteRangeText;
            }
            set
            {
                noteRangeText = value;
                RaisePropertyChanged();
            }
        }

        public string BpmText
        {
            get
            {
                return bpmText;
            }
            set
            {
                bpmText = value;
                RaisePropertyChanged();
            }
        }

        public string DivisionText
        {
            get
            {
                return divisionText;
            }
            set
            {
                divisionText = value;
                RaisePropertyChanged();
            }
        }

        public string TrackCountText
        {
            get
            {
                return trackCountText;
            }
            set
            {
                trackCountText = value;
                RaisePropertyChanged();
            }
        }

        public string NoteCountText
        {
            get
            {
                return noteCountText;
            }
            set
            {
                noteCountText = value;
                RaisePropertyChanged();
            }
        }

        public string EventCountText
        {
            get
            {
                return eventCountText;
            }
            set
            {
                eventCountText = value;
                RaisePropertyChanged();
            }
        }


        public void Clear()
        {
            NoteRangeText = null;

        }

    }
}
