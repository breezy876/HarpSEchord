using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;


namespace Common.Models.Settings
{

    [JsonObject(MemberSerialization.OptOut)]
    public class AutomationSettings
    {

        public AutomationSettings()
        {
            PlayOnStart = true;
            SendReadyCheckOnEquip = true;
            AcceptReadyChecks = true;
            GuitarMode = 0;
            StopOnClose = true;
        }


        public bool EnableInstrumentSwitching { get; set; }
        public bool CloseOnFinish { get; set; }
        public bool SendReadyCheckOnEquip { get; set; }

        public int GuitarMode { get; set; }
        public bool AcceptReadyChecks { get; set; }
        public bool PlayOnStart { get; set; }

        public bool StopOnClose { get; set; }
    }
}
