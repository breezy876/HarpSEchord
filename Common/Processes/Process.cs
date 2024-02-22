using Common.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Processes
{
    public abstract class Process
    {
        protected System.Diagnostics.Process process;

        public Process()
        {

        }

        protected string processName;

        public delegate void OutputDataReceivedDelegate(object sender, DataReceivedEventArgs args);

        public event OutputDataReceivedDelegate OutputDataReceived;

        public virtual System.Diagnostics.Process Run(string processName, string[] args)
        {
            this.processName = processName;

            var argsWithSpaces = args.JoinWithSpaces();

            process = ProcessHelpers.RunAsAdmin(processName, true, true, true, argsWithSpaces);

            process.OutputDataReceived += new DataReceivedEventHandler(OnOutputDataReceived);

            return process;
        }

        public virtual void Write(string text)
        {
            if (process.HasExited)
                return;

            process.StandardInput.WriteLine(text);
            process.StandardInput.Flush();
        }

        public virtual void Exit()
        {
            if (!process.HasExited)
                process.Kill();
        }

        private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            OutputDataReceived(process, e);
            //var msg = new AddLogNotification() { ServiceName = "PlaybackService", Text = e.Data };

            //Messenger.Default.Send(msg);

            //Console.WriteLine(e.Data);
        }
    }
}
