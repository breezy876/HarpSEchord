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
    public class TrackSettings
    {

        public TrackSettings()
        {
            ShowSettings = false;
            PopulateFromPlaylist = true;
            EnableChooseDrums = true;
        }

        public bool ShowSettings { get; set; }
        public bool TransposeDrums { get; set; }
        public bool TransposeInstruments { get; set; }
        public bool TransposeFromTitle { get; set; }
        public bool EnableChooseDrums { get; set; }
        public bool PopulateFromPlaylist { get; set; }

        public bool PopulateFromMidi { get; set; }
        
    }
}
