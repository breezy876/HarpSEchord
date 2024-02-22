using Hscm.UI.Notifications;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI.ViewModels
{
    public class LogViewModel : ObservableViewModel
    {

        private string outputText;
        private string outputTextColor;

        private const string ErrorTextColor = "#90F0F6FF";
        private const string NormalTextColor = "#FF727683";
        public LogViewModel() : base()
        {
            LoggingEnabled = true;
        }

        public RelayCommand ClearCommand { get { return new RelayCommand(ExecuteClearCommand); } }

        public RelayCommand ToggleLoggingCommand { get { return new RelayCommand(ExecuteLoggingToggleCommand); } }

        public string OutputText
        {
            get
            {
                return outputText;
            }
            set
            {
                outputText = value;
                RaisePropertyChanged();
            }
        }

        public string OutputTextColor
        {
            get
            {
                return outputTextColor;
            }
            set
            {
                outputTextColor= value;
                RaisePropertyChanged();
            }
        }

        public bool LoggingEnabled
        {
            get
            {
                return Common.Settings.AppSettings.LoggingEnabled;
            }
            set
            {
                Common.Settings.AppSettings.LoggingEnabled = value;
                RaisePropertyChanged();
            }
        }


        public void AppendLog(string serviceName, string text)
        {
            if (!LoggingEnabled)
                return;

            var builder = new StringBuilder(this.OutputText);

            if (string.IsNullOrEmpty(serviceName))
                builder.AppendLine(text);
            else
                builder.AppendLine($"[{serviceName} {DateTime.Now.ToString("hh:mm:ss")}]: {text}");

            this.OutputText = builder.ToString();
        }

        public void ExecuteClearCommand()
        {
            OutputText = null;
        }

        public void ExecuteLoggingToggleCommand()
        {
            var saveMsg = new SaveSettingsNotification() { SaveAppSettings = true };
            Messenger.Default.Send(saveMsg);
        }

    }
}
