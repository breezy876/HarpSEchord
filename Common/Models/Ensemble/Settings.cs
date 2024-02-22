using Common.Models.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.Ensemble
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Settings
    {
        public string ConductorIp { get; set; }

        public string CharacterName { get; set; }

        public string ConductorName { get; set; }

        public string WorldName { get; set; }
    }
}
