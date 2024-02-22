using GalaSoft.MvvmLight.Messaging;

namespace Hscm.UI.Notifications
{
    public class InstrumentChangedNotification : MessageBase
    {
        public InstrumentChangedNotification()
        {
        }

        public int? EnsembleMember { get; set; }

        public int TrackIndex { get; set; }

        public string Instrument { get; set; }
    }
}