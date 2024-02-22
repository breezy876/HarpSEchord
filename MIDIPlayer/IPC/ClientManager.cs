
using Common;
using Common.Messaging;
using Common.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hscm.Models.Ffxiv;
using TinyIpc.IO;
using TinyIpc.Synchronization;
using Hscm.UI.Services;

namespace Hscm.IPC
{

    public enum ClientMessageType
    {
        PlaybackStarted = 1,
        PlaybackFinished = 2,
        Stopped = 3,
        Paused = 4,
        Ticked = 5,
        LoadStarted = 6,
        LoadFinished = 7,
        Connect = 8,
        Disconnect = 9
    }

    public enum MessageType
    {
        ReloadPlaylist = 1,
        ReloadPlaylistSettings = 2,
        ChangeSong = 3,
        SwitchInstruments = 4,
        OpenInstrument = 5, //for other clients broadcasted from leader
        Close = 6,
        Midi = 7,
        Playback = 8,
        Client = 9,
        Connect = 10,
        Disconnect = 11
    }

    public enum PlaybackMessageType
    {
        Play = 1,
        Pause = 2,
        Stop = 3,
        Next = 4,
        Previous = 5,
        Seek = 6,
        ChangeSpeed = 7
    }


    public partial class ClientManager
    {
        const string MessageBusName = "HSC.MessageBus";
        static TinyIpc.Messaging.TinyMessageBus bus;

        //static Dictionary<int, EventWaitHandle> clientWaitHandles;
        //static Dictionary<int, EventWaitHandle> waitHandles;

        public static event EventHandler<int> Connected;
        public static event EventHandler<int> Disconnected;

        private static Dictionary<int,FfxivClient> clients;

        static ClientManager()
        {

            //clientWaitHandles = new Dictionary<int, EventWaitHandle>();
            //waitHandles = new Dictionary<int, EventWaitHandle>();
            clients = new Dictionary<int, FfxivClient>();
        }

        public static void Initialize()
        {
            try
            {
                const long maxFileSize = 1 << 24;
                bus  = new TinyIpc.Messaging.TinyMessageBus(new TinyMemoryMappedFile(MessageBusName, maxFileSize), true);


                bus.MessageReceived += Bus_MessageReceived;
                LogService.AppendLog("", $"IPC service initialized");


            }
            catch (Exception ex)
            {
                LogService.AppendLog("",$"Error initializing IPC service {ex.Message}");
            }
        }

        private static void Bus_MessageReceived(object sender, TinyIpc.Messaging.TinyMessageReceivedEventArgs e)
        {
            MessageHandler.HandleMessage((byte[])e.Message);
        }

        public static void Dispose()
        {
            bus.Dispose();

            if (clients.IsNullOrEmpty())
                return;

            //foreach (var wh in clientWaitHandles.ToList())
            //{
            //    wh.Value.Close();
            //    wh.Value.Dispose();
            //    clientWaitHandles.Remove(wh.Key);
            //}

            //foreach (var wh in waitHandles.ToList())
            //{
            //    wh.Value.Close();
            //    wh.Value.Dispose();
            //    waitHandles.Remove(wh.Key);
            //}

            clients.Clear();

        }
        public static void RemoveAll()
        {
            if (clients.IsNullOrEmpty())
                return;

            //if (waitHandles.IsNullOrEmpty())
            //    return;

            //foreach (var wh in waitHandles.ToList())
            //{
            //    wh.Value.Close();
            //    wh.Value.Dispose();
            //    waitHandles.Remove(wh.Key);
            //}

            //if (clientWaitHandles.IsNullOrEmpty())
            //    return;

            //foreach (var wh in clientWaitHandles.ToList())
            //{
            //    wh.Value.Close();
            //    wh.Value.Dispose();
            //    clientWaitHandles.Remove(wh.Key);
            //}

            clients.Clear();
        }

        public static void Remove(int index)
        {
            if (!clients.ContainsKey(index))
                return;

            clients.Remove(index);

            //clientWaitHandles[index].Close();
            //clientWaitHandles[index].Dispose();
            //clientWaitHandles.Remove(index);

            //waitHandles[index].Close();
            //waitHandles[index].Dispose();
            //waitHandles.Remove(index);
        }

        public static void Connect(int index)
        {
            //clientWaitHandles[index] = new EventWaitHandle(false, EventResetMode.ManualReset, $"MidiBard.WaitEvent.{index}");
            //waitHandles[index] = new EventWaitHandle(false, EventResetMode.ManualReset, $"HSCM.WaitEvent.{index}");

            //clientWaitHandles[index].Set();

   
                //waitHandles[index].WaitOne();
                //waitHandles[index].Reset();
                Connected.Invoke(null, index);

        }
        public static void Disconnect(int index)
        {
            if (!clients.ContainsKey(index))
                return;

 
                //waitHandles[index].WaitOne();
                //clientWaitHandles[index].Close();
                //clientWaitHandles[index].Dispose();
                //waitHandles[index].Close();
                //waitHandles[index].Dispose();
                Disconnected.Invoke(null, index);
        }

        public static void Add(string charName, int index, bool connect = true)
        {
            try
            {
                if (clients.ContainsKey(index))
                    return;

                //var clientWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, $"MidiBard.WaitEvent.{index}");
                //var waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, $"HSCM.WaitEvent.{index}");
                var client = new FfxivClient() { CharacterName = charName, Index = index };

                clients.Add(index, client);
                //clientWaitHandles.Add(index, clientWaitHandle);
                //waitHandles.Add(index, waitHandle);

                if (connect)
                    Connect(index);
            }
            catch (Exception ex)
            {
          
       
           }
        }

        private static async Task SendMessage(byte[] msg)
        {
            try
            {
                await bus.PublishAsync(msg);
            }

            catch (Exception ex)
            {

            }
        }
    }
}
