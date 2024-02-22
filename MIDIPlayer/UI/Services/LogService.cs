using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI.Services
{
    internal class LogService
    {
        public static void AppendLog(string serviceName, string text)
        {
            Hscm.App.Window.AppendLog(serviceName, text);
        }

    }
}
