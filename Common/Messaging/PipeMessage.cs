using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messaging
{
    public enum PipeMessageType { EnsembleService, WorkerService, PlayerService, Client }

    [Serializable]
    public class PipeMessage
    {
        public PipeMessageType Type { get; set; }

        public object Message { get; set; }
    }
}
