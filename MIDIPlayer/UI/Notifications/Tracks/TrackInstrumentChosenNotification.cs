using GalaSoft.MvvmLight.Messaging;

namespace Hscm.UI.Notifications.Tracks
{
    public class TrackInstrumentChosenNotification :  MessageBase
    {
        public TrackInstrumentChosenNotification()
        {
        }

        public int TrackIndex { get; set; }

        public string Instrument { get; set; }

    }
}