using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.IPC
{
    public partial class ClientManager
    {

        public static void SendConnect(int index)
        {
            SendMessage(new byte[] { (byte)MessageType.Connect, (byte)index });
        }

        public static void SendDisconnect(int index)
        {
            SendMessage(new byte[] { (byte)MessageType.Disconnect, (byte)index });
        }

        public static void ChangeSong(int index)
        {
            var buffer = new byte[] { (byte)MessageType.ChangeSong };
            buffer = buffer.Concat(BitConverter.GetBytes(index)).ToArray();
            SendMessage(buffer);
        }

        public static void ReloadPlaylist()
        {
            SendMessage(new byte[] { (byte)MessageType.ReloadPlaylist });
        }

        public static void ReloadPlaylistSettings()
        {
            SendMessage(new byte[] { (byte)MessageType.ReloadPlaylistSettings });
        }

        public static void SwitchInstruments()
        {
            SendMessage(new byte[] { (byte)MessageType.SwitchInstruments });
        }

        public static void CloseInstruments()
        {
            SendMessage(new byte[] { (byte)MessageType.Close });
        }

    }
}
