using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace Hscm.UI.Services
{
    internal class ToastService
    {
        public static void DisplayMessage(string text, MessageType type = MessageType.Info, bool center = true, int y = 0)
        {

                Notifier notifier = new Notifier(cfg =>
                {

                    cfg.PositionProvider = center ? new WindowPositionProvider(

                        parentWindow: System.Windows.Application.Current.MainWindow,
                        corner: Corner.BottomCenter,
                        offsetX: 5,
                        offsetY: 200) : new WindowPositionProvider(

                        parentWindow: System.Windows.Application.Current.MainWindow,
                        corner: Corner.BottomRight,
                        offsetX: 0,
                        offsetY: y);

                    cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                        notificationLifetime: TimeSpan.FromSeconds(3),
                        maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                    cfg.Dispatcher = System.Windows.Application.Current.Dispatcher;
                });

                switch (type)
                {
                    case MessageType.Info:
                        notifier.ShowInformation(text);
                        break;
                    case MessageType.Success:
                        notifier.ShowSuccess(text);
                        break;
                    case MessageType.Error:
                        notifier.ShowError(text);
                        break;
                }
        }
    }
}
