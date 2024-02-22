using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Common.Models.Settings
{

    [JsonObject(MemberSerialization.OptOut)]
    public class GeneralSettings
    {

        public GeneralSettings()
        {
            EnableTranspose = true;
            UseMidiCache = true;
            Topmost = true;
            InputMode = 0;
            KeyDelta = 30;
        }

        public bool EnableLogging { get; set; }

        public bool Topmost { get; set; }

        public bool TestMode { get; set; }

        public bool UseMidiCache { get; set; }

        public bool EnableTranspose { get; set; }

        public bool EnableTrim { get; set; }

        public bool EnableTrimFromTracks { get; set; }
        public int InputMode { get; set; }
        public int KeyDelta { get; set; }
    }
}
