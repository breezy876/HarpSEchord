using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messaging
{
    public enum EnsembleMessageType { 
        ConductorHost, 
        ConductorEnd, 
        ParticipantJoin, 
        ParticipantLeave, 
        ConductorPing, 
        ParticipantPong,
        SendKeyUp, 
        SendKeyDown,
        ReadyInstrument
    }

    [Serializable]
    public class EnsembleMessage
    {
        public EnsembleMessageType Type { get; set; }

        public object[] Data { get; set; }
    }
}
