using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.IPC
{
    public partial class ClientManager
    {
        public static async Task Play()
        {
            await SendMessage(new byte[] { (byte)MessageType.Playback, (byte)PlaybackMessageType.Play } );
        }

        public static async Task Pause()
        {
            await SendMessage(new byte[] { (byte)MessageType.Playback, (byte)PlaybackMessageType.Pause });
        }

       
        public static async Task Stop()
        {
            await SendMessage(new byte[] { (byte)MessageType.Playback, (byte)PlaybackMessageType.Stop });
        }

        public static async Task Next()
        {
            await SendMessage(new byte[] { (byte)MessageType.Playback, (byte)PlaybackMessageType.Next });
        }

        public static async Task Previous()
        {
            await SendMessage(new byte[] { (byte)MessageType.Playback, (byte)PlaybackMessageType.Previous });
        }

        public static async Task ChangeSpeed(float speed)
        {
            var buffer = new byte[] { (byte)MessageType.Playback, (byte)PlaybackMessageType.ChangeSpeed };
            buffer = buffer.Concat(BitConverter.GetBytes((double)speed)).ToArray();
            await SendMessage(buffer);
        }

        public static async Task Seek(long time)
        {
            var buffer = new byte[] { (byte)MessageType.Playback, (byte)PlaybackMessageType.Seek };
            buffer = buffer.Concat(BitConverter.GetBytes(time)).ToArray();
            await SendMessage(buffer);
        }


    }
}
