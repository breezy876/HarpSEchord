using Hscm.UI.Notifications.Window;
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
using System.Windows.Shapes;

namespace Hscm.UI
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>

    public struct ProgressInfo
    {
        public ProgressInfo(string text, int percent)
        {
            LoadingText = text;
            PercentComplete = percent;
        }

        public string LoadingText { get; set; }
        public int PercentComplete { get; set; }
    }

    public partial class ProgressWindow : Window
    {

        private ProgressViewModel viewModel;

        private IProgress<ProgressInfo> progress;

        public ProgressWindow()
        {
            InitializeComponent();

            this.viewModel = new ProgressViewModel();
            this.DataContext = this.viewModel;

            progress = new Progress<ProgressInfo>(info =>
            {
                this.viewModel.LoadingText = info.LoadingText;
                this.viewModel.PercentText = info.PercentComplete + "%";
                this.viewModel.PercentComplete = info.PercentComplete;
            });

            Messenger.Default.Register<ReportProgressNotification>(this, ReportProgressNotificationReceived);
        }


        private void ReportProgressNotificationReceived(ReportProgressNotification obj)
        {
            progress.Report(obj.ProgressInfo);
        }
    }
}
