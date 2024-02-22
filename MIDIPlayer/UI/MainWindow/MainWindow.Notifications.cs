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
        private void InitializeNotifications()
        {
     
            Messenger.Default.Register<AutofillOptionChangedNotification>(this, AutofillOptionChangedNotificationReceived);

            Messenger.Default.Register<ChoosePluginPathNotification>(this, ChoosePluginPathNotificationReceived);

            Messenger.Default.Register<ConnectFfxivClientNotification>(this, ConnectFfxivClientNotificationReceived);

            Messenger.Default.Register<ToggleInstrumentsNotification>(this, ToggleInstrumentsNotificationReceived);
            Messenger.Default.Register<CloseInstrumentsNotification>(this, CloseInstrumentsNotificationReceived);

            Messenger.Default.Register<ShowTracksNotification>(this, ShowTracksNotificationReceived);
            Messenger.Default.Register<HideTracksNotification>(this, HideTracksNotificationReceived);

            Messenger.Default.Register<EnableTrackNotification>(this, EnableTrackNotificationReceived);
            Messenger.Default.Register<DisableTrackNotification>(this, DisableTrackNotificationReceived);
            Messenger.Default.Register<TrackHasResetNotification>(this, ResetTrackNotificationReceived);

            Messenger.Default.Register<ReloadSequenceNotification>(this, ReloadSequenceNotificationReceived);

            Messenger.Default.Register<SequenceTempoChangedNotification>(this, SequenceTempoChangedNotificationReceived);
            Messenger.Default.Register<SequenceTempoUpdatedNotification>(this, SequenceTempoUpdatedNotificationReceived);


            Messenger.Default.Register<SeekChangedNotification>(this, SeekChangedNotificationReceived);

            Messenger.Default.Register<PlaylistEmptyNotification>(this, PlaylistEmptyNotificationReceived);

            Messenger.Default.Register<PlaylistSequenceSelectedNotification>(this, PlaylistSequenceSelectedNotificationReceived);

            Messenger.Default.Register<SequenceChordSettingsChangedNotification>(this, SequenceChordSettingsChangedNotificationReceived);

            Messenger.Default.Register<SaveSettingsNotification>(this, SaveSettingsNotificationReceived);

            Messenger.Default.Register<ShowMessageNotification>(this, ShowMessageNotificationReceived);


            Messenger.Default.Register<HideProgressNotification>(this, HideProgressNotificationReceived);
            Messenger.Default.Register<ShowProgressNotification>(this, ShowProgressNotificationReceived);

            Messenger.Default.Register<SaveAppSettingsNotification>(this, SaveAppSettingsNotificationReceived);
            Messenger.Default.Register<SaveSongSettingsNotification>(this, SaveSongSettingsNotificationReceived);
            Messenger.Default.Register<ShowSaveSongSettingsDialogNotification>(this, ShowSaveSongSettingsDialogNotificationReceived);
            Messenger.Default.Register<LoadSongSettingsNotification>(this, LoadSongSettingsNotificationReceived);
            Messenger.Default.Register<LoadCurrentSongSettingsNotification>(this, LoadCurrentSongSettingsNotificationReceived);


            Messenger.Default.Register<GameWindowFoundNotification>(this, GameWindowFoundNotificationReceived);
            Messenger.Default.Register<GameWindowLoggedOutOrExitedNotification>(this, GameWindowExitedNotificationReceived);

            Messenger.Default.Register<LayoutChangedNotification>(this, LayoutChangedNotificationReceived);


            Messenger.Default.Register<UpdateSelectedSequenceNotification>(this, UpdateSelectedSequenceNotificationReceived);

            Messenger.Default.Register<DuplicateTracksNotification>(this, DuplicateTrackNotificationReceived);


            Messenger.Default.Register<SplitPercussionNotification>(this, SplitPercussionNotificationReceived);

            Messenger.Default.Register<RemoveTracksNotification>(this, RemoveTracksNotificationReceived);
            Messenger.Default.Register<RemoveAllTracksNotification>(this, RemoveAllTracksNotificationReceived);
            Messenger.Default.Register<TrimTracksNotification>(this, TrimTracksNotificationReceived);

            Messenger.Default.Register<ItemsChangedNotification>(this, ItemsChangedReceived);


            InitializeMidiControlNotifications();

        }

 
    }

    #endregion

}
