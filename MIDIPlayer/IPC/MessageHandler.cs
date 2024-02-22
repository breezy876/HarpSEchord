
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyIpc.Messaging;

namespace Hscm.IPC
{
    /// <summary>
    /// author:  SpuriousSnail86
    /// </summary>
    /// 

    internal partial class MessageHandler
    {
        public static event EventHandler<int> ConnectMessageReceived;

        public static event EventHandler<int> DisconnectMessageReceived;

        public static event EventHandler<int> LoadStartedMessageReceived;

        public static event EventHandler<(int, long)> LoadFinishedMessageReceived;

        public static event EventHandler<int> PlaybackStartedMessageReceived;

        public static event EventHandler PlaybackFinishedMessageReceived;

        public static event EventHandler PausedMessageReceived;

        public static event EventHandler StoppedMessageReceived;

        public static event EventHandler<(TimeSpan ts, int position)> TickedMessageReceived;


        public static void HandleMessage(byte[] buffer)
        {
            try
            {
                if (buffer.Length == 0)
                    return;

                ProcessMessage(buffer);
            }
            catch (Exception ex)
            {
                //PluginLog.Error($"Error processing IPC message buffer: {ex.Message}");
            }
        }

        private static void ProcessMessage(byte[] buffer)
        {
            var ms = new MemoryStream(buffer);
            var br = new BinaryReader(ms);

            var type = (MessageType)br.ReadByte();

            switch (type)
            {
                case MessageType.Client:
                    var playbackEvType = (ClientMessageType)br.ReadByte();
                    HandleClientMessage(playbackEvType, buffer.Skip(2).ToArray());
                    break;
            }

            br.Close();
            ms.Close();
        }

        private static void HandleClientMessage(ClientMessageType type, byte[] buffer)
        {
            var ms = new MemoryStream(buffer);
            var br = new BinaryReader(ms);

            switch (type)
            {
                case ClientMessageType.Connect:
                    int index = (int)br.ReadByte();
                    ConnectMessageReceived.Invoke(null, index);
                    break;

                case ClientMessageType.Disconnect:
                    index = (int)br.ReadByte();
                    DisconnectMessageReceived.Invoke(null, index);
                    break;

                case ClientMessageType.LoadStarted:
                    index = br.ReadInt32();
                    LoadStartedMessageReceived.Invoke(null, index);
                    break;

                case ClientMessageType.LoadFinished:
                    index = br.ReadInt32();
                    long length = br.ReadInt64();
                    LoadFinishedMessageReceived.Invoke(null, (index, length));
                    break;

                case ClientMessageType.PlaybackStarted:
                   index = br.ReadInt32();
                    PlaybackStartedMessageReceived.Invoke(null, index);
                    break;

                case ClientMessageType.PlaybackFinished:
                    PlaybackFinishedMessageReceived.Invoke(null, EventArgs.Empty);
                    break;

                case ClientMessageType.Paused:
                    PausedMessageReceived.Invoke(null, EventArgs.Empty);
                    break;

                case ClientMessageType.Stopped:
                    StoppedMessageReceived.Invoke(null, EventArgs.Empty);
                    break;

                case ClientMessageType.Ticked:
                    int mins = (int)br.ReadByte();
                    int secs = (int)br.ReadByte();
                    int position = (int)br.ReadInt64();
                    TickedMessageReceived.Invoke(null, (new TimeSpan(0, mins, secs), position));
                    break;
            }
        }
    }
}

