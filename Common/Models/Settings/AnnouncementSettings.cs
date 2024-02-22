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
    public class AnnouncementSettings
    {

        public AnnouncementSettings()
        {
            Announce = false;
            AnnounceType = "/say";
            Text = "{Now playing [title]!|Next song up is [title]!}";
            Delay = 200;
        }

        public string AnnounceType { get; set; }

        public bool Announce { get; set; }

        public string Text { get; set; }

        public int Delay { get; set; }
    }
}
