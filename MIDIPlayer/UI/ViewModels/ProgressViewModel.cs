using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI.ViewModels
{
    public class ProgressViewModel : ObservableViewModel
    {

        private string loadingText;
        private string progressPercentText;

        private int percentComplete;


        public ProgressViewModel() : base()
        {

        }

        public int PercentComplete
        {
            get
            {
                return percentComplete;
            }
            set
            {
               percentComplete = value;
                RaisePropertyChanged();
            }
        }

        public string LoadingText
        {
            get
            {
                return loadingText;
            }
            set
            {
                loadingText = value;
                RaisePropertyChanged();
            }
        }

        public string PercentText
        {
            get
            {
                return progressPercentText;
            }
            set
            {
                progressPercentText = value;
                RaisePropertyChanged();
            }
        }
    }
}
