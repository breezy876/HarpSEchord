
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI.Notifications.Settings.Pipe
{
    public enum TracksWindowChangeType
    {
        ChangeTracks,
        ChangeTracksKey,
        ChangeTracksOctave,
        MuteAllTracks,
        UnmuteAllTracks,
        ResetAllTracks,
        MuteTracks,
        UnmuteTracks,
        ResetTracks
    }

    public class PipeTracksChangeNotification : MessageBase
    {
        public TracksWindowChangeType Type { get; set; }

        public object[] Data { get; set; }
    }
}
