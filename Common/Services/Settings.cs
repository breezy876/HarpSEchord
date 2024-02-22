using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Settings
    {
        public Settings()
        {
            ThreadDelay = 1;
            ShowLogMessages = true;
            LogErrors = true;
        }

        public int ThreadDelay { get; set; }

        public bool ShowLogMessages { get; set; }

        public bool LogErrors { get; set; }
    }
}
