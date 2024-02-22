using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messaging.Services
{
    public enum WorkerServiceMessageType { 
        LoggedIn,
        LoggedOut,
        GameWindowExited, 
        InstrumentEquipped, 
        InstrumentUnequipped, 
        EnsembleStarted, 
        EnsembleReadyCheckReceived,
        ChatLogActive, 
        ChatLogInactive,
        PartyInviteReceived,
        Play,
        ReadyInstruments,
        AcceptReadyCheck,
        Launched,
        FormPartyMessageReceived,
        PartyLeaderChangedMessageReceived,
        PartyDisbandedMessageReceived,
        EnsemblePacketReceived,
        PartyChangedPacketReceived,
        PartyChatPacketReceived,
        EnsembleFinished
    }


    [Serializable]
    public class WorkerServiceMessage
    {
        public WorkerServiceMessageType Type { get; set; }

        public object Message { get; set; }
    }
}
