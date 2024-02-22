using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class ProcessHelpers
    {

        public static Process GetByName(string processName)
        {
            var processes = Process.GetProcessesByName(processName);

            if (!processes.Any())
                return null;

            var process = processes.First();

            return process;
        }

        public static bool IsRunning(string processName)
        {
            return GetByName(processName) != null;

        }

        public static void KillByName(string processName)
        {
            var process = GetByName(processName);

            if (process == null)
                return;

            Kill(process);
        }

        public static Process RunAsAdmin(string processName, bool redirectInput = false, bool redirectOutput = false, bool hidden = true, string args = null)
        {

            KillIfRunning(processName);

            var appDir = Directory.GetCurrentDirectory();
            var filePath = System.IO.Path.Combine(appDir, $"{processName}.exe");

            var process = new Process();

            process.StartInfo.FileName = filePath;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.Verb = "runas";

            process.StartInfo.Arguments = args;

            if (redirectInput)
                process.StartInfo.RedirectStandardInput = true;

            if (redirectOutput)
            {
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
            }

            if (hidden)
            {
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }

            process.Start();

            if (redirectOutput)
            {
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
            }

            return process;
        }

        private static void KillIfRunning(string processName)
        {
            var process = GetByName(processName);

            if (IsRunning(processName))
                Kill(process);
        }

        private static void Kill(Process process)
        {
            try
            {
                if (!process.HasExited)
                    process.Kill();
            }
            catch (Exception ex)
            {

            }
        }

    }
}
