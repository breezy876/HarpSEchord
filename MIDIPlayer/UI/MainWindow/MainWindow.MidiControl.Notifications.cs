using Hscm.UI.Notifications;
using Hscm.UI.Notifications.Playlist;
using Hscm.UI.Notifications.Settings;
using Hscm.UI.Notifications.Settings.Pipe;
using Hscm.UI.Notifications.Tracks;
using Hscm.UI.Notifications.Window;
using Hscm.UI.ViewModels.MainWindow;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI
{
    public partial class MainWindow 
    {

        #region notifications
        private void InitializeMidiControlNotifications()
        {

            Messenger.Default.Register<TempoMouseWheelNotification>(this, TempoMouseWheelNotificationReceived);

            //Messenger.Default.Register<TempoShiftNotification>(this, TempoShiftNotificationReceived);

            //Messenger.Default.Register<TempoRevertNotification>(this, TempoRevertNotificationReceived);

            Messenger.Default.Register<TempoMouseEnterNotification>(this, TempoMouseEnterNotificationReceived);

            Messenger.Default.Register<TempoResetNotification>(this, TempoResetNotificationReceived);

            Messenger.Default.Register<SeekMouseWheelNotification>(this, SeekMouseWheelNotificationReceived);
            Messenger.Default.Register<SeekMouseLeftButtonDownNotification>(this, SeekMouseLeftButtonDownNotificationReceived);
            Messenger.Default.Register<SeekMouseEnterNotification>(this, SeekMouseEnterNotificationReceived);
            Messenger.Default.Register<SeekMouseLeaveNotification>(this, SeekMouseLeaveNotificationReceived);

        }

 
    }

    #endregion

}
