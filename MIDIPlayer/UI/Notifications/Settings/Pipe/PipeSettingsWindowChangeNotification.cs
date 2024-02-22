
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI.Notifications.Settings.Pipe
{
    public enum SettingsWindowChangeType
    {
        ChangeMinNoteLength,
        ChangeHoldLongNotes,
        ChangeStartDelay,
        ChangeSequenceHighestOnly,
        ChangeChordReduceMaxNotes,
        ChangeChordReducePlayAll
    }

    public class PipeSettingsWindowChangeNotification : MessageBase
    {
        public SettingsWindowChangeType Type { get; set; }

        public object[] Data { get; set; }
    }
}
