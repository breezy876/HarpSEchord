using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messaging.Services
{
    public enum PlayerServiceMessageType { 

        PlaybackStarted,
        PlaybackFinished,
        Stopped,
        Paused,
        Resumed,
        SkippedStart,
        SkippedEnd,
        PositionChanged,
        TempoChanged,
        Ticked,
        NoteOn,
        NoteOff,
        IsReady
    }


    [Serializable]
    public class PlayerServiceMessage
    {
        public PlayerServiceMessageType Type { get; set; }

        public object Message { get; set; }
    }
}
