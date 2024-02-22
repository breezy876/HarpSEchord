using GalaSoft.MvvmLight.Messaging;
using System.Windows;

namespace Hscm.UI
{
    public enum MessageType {   Info, Error, Success }
    public class ShowMessageNotification : MessageBase
    {
        public FrameworkElement Parent { get; set; }

        public bool Center { get; set; }
        public MessageType Type { get; set; }

        public string Text { get; set; }

        public string Title { get; set; }
    }
}