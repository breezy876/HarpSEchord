using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messaging
{
    public enum ConductorMessageType { Player, Settings, Ensemble }

    [Serializable]
    public class ConductorMessage
    {
        public int[] Participants { get; set; }

        public ConductorMessageType Type { get; set; }

        public object Message { get; set; }
    }
}
