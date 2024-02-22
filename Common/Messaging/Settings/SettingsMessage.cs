
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messaging.Settings
{
    public enum SettingsMessageType
    {
        ChangeTracks,

        ChangeKey,
        ChangeOctave,
        ChangeTracksKey,
        ChangeTracksOctave,

        ChangeMinNoteLength,
        ChangeHoldLongNotes,
        ChangeStartDelay,
        ChangeSequenceHighestOnly,
        ChangeChordReduceMaxNotes,
        ChangeChordReducePlayAll,


        MuteAllTracks,
        UnmuteAllTracks,
        MuteTracks,
        UnmuteTracks,
        ResetTracks,
        ResetAllTracks,

        EnsembleSendKeysEnabled
    }

    [Serializable]
    public class SettingsMessage 
    {
        public SettingsMessageType Type { get; set; }

        public object[] Data { get; set; }
    }
}
