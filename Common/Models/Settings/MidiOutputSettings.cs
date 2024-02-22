
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.Settings
{
    [JsonObject(MemberSerialization.OptOut)]
    public class MidiOutputSettings
    {
        public MidiOutputSettings()
        {
            PlaybackMode = PlaybackType.Preview;

            PlaybackSoundEnabled = false;
        }

        public PlaybackType PlaybackMode { get; set; }

        public bool PlaybackSoundEnabled { get; set; }
    }
}
