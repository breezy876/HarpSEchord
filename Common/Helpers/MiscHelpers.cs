using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Common.Helpers
{
    public class MiscHelpers
    {

        public static void ShowInfoMessageBox(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2} ", b);
            return hex.ToString();
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static void LogError(string msg, string stackTrace, string filePath)
        {

            var text = "--------------------------------------------------------------------------------------------------------------------------------\n\r";
            text += $"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}\n\r";
            text += "--------------------------------------------------------------------------------------------------------------------------------\n\r";
            text += msg;
            text += "\n\r--------------------------------------------------------------------------------------------------------------------------------\n\r";
            text += stackTrace;

            File.WriteAllText(filePath, text);
        }

        public static void TimerTrigger(System.Action action, int delay)
        {
            var timer = new System.Timers.Timer();
            timer.Elapsed += (sender, args) => action();
            timer.Interval = delay;
            timer.AutoReset = false;
            timer.Enabled = true;
        }

        public static async Task InvokeTimedAction(System.Action action, CancellationToken token, int delay, bool reset = false)
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (token.IsCancellationRequested)
                        break;

                    Thread.Sleep(delay);
                    action();
                }
            }, token);

            //var timer = new System.Timers.Timer();
            //timer.Elapsed += (sender, args) => action(timer);
            //timer.Interval = delay;
            //timer.AutoReset = reset;
            //timer.Enabled = true;
        }

    }
}
