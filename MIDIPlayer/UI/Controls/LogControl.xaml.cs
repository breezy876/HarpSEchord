using Hscm.UI.Notifications;
using Hscm.UI.Notifications.Playlist;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hscm.UI
{
    /// <summary>
    /// Interaction logic for InfoControl.xaml
    /// </summary>
    public partial class LogControl : StackPanel
    {
        private LogViewModel viewModel;

        public LogControl()
        {
            InitializeComponent();
        }

        public void Initialize(LogViewModel viewModel)
        {
            this.viewModel = viewModel;

            this.DataContext = viewModel;

            Messenger.Default.Register<PlaylistEmptyNotification>(this, PlaylistEmptyNotificationReceived);
            Messenger.Default.Register<AddLogNotification>(this, AddLogNotificationReceived);
        }


        private void PlaylistEmptyNotificationReceived(PlaylistEmptyNotification msg)
        {
            //Dispatcher.Invoke(() =>
            //{
            //    this.viewModel.Clear();
            //});
        }

        private void AddLogNotificationReceived(AddLogNotification obj)
        {
            Dispatcher.Invoke(() =>
            {
                AppendLog(obj.ServiceName, obj.Text);
            });
        }

        public void AppendLog(string serviceName, string text)
        {
            //if (!Common.Settings.AppSettings.GeneralSettings.EnableLogging)
            //    return;

            Dispatcher.Invoke(() =>
        {
            this.viewModel.AppendLog(serviceName, text);
            this.outputText.ScrollToEnd();
        });
        }

    }
}
