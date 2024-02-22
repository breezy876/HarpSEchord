using GalaSoft.MvvmLight.Messaging;

namespace Hscm.UI.Notifications.Tracks
{
    internal class TrackEnsembleMemberChangedNotification :  MessageBase
    {
        public TrackEnsembleMemberChangedNotification()
        {
        }

        public int TrackIndex { get; set; }
        public string Member { get; set; }

    }
}