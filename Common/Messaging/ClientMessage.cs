
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messaging
{
    //messages from client to a service
    public enum ClientMessageType
    {
        Launched,
        LoggedIn,
        PluginChosen
    }

    [Serializable]
    public class ClientMessage 
    {
        public ClientMessageType Type { get; set; }

        public object Data { get; set; }
    }
}
