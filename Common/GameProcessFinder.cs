using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class GameProcessFinder
    {
        public static Process[] Find()
        {
            Process[] processes = Process.GetProcessesByName("ffxiv_dx11");

            if (processes.IsNullOrEmpty())
                return null;

            return processes.Where(p => p.MainWindowHandle.ToInt32() > 0 && !p.HasExited && !p.MainWindowTitle.IsNullOrEmpty()).OrderBy(p => p.StartTime).ToArray();
        }

    }
}
