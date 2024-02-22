using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Processes
{
    public abstract class ProcessMessageSender
    {
        protected Common.Processes.Process process;

        protected ProcessMessageSender(Common.Processes.Process process)
        {
            this.process = process;
        }

        protected void Send(string text)
        {
            this.process.Write(text);
        }
    }
}
