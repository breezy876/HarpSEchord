using Common.Music;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.Settings
{
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class SongSettings
    {
        public Dictionary<string, MidiSequence> Settings;

        public SongSettings()
        {
            Settings = new Dictionary<string, MidiSequence>();
        }

        public void Clear()
        {
            Settings.Clear();
        }

        public bool HasItems => !Settings.IsNullOrEmpty();

        public void Dispose()
        {
            Clear();
            Settings = null;
        }
    }
}
