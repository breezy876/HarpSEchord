
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messaging.Player
{
    public enum PlayerMessageType
    {
        Start,
        End,
        Play,
        Pause,
        Stop,
        Resume,
        GotoBeginning,
        GotoEnd,
        FastForward,
        Rewind,
        Next,
        Previous,
        Seek,

        ChangeSequence,
        ChangeTempo,
        Reload,
        ToggleInstruments,
        EquipInstruments,
        UneqipInstruments,

        SelectMember,
        DeselectMember,
        StartLocalEnsemble,
        StopLocalEnsemble,
        CloneTrack
    }

    [Serializable]
    public class PlayerMessage 
    {
        public PlayerMessageType Type { get; set; }

        public object[] Data { get; set; }
    }
}
