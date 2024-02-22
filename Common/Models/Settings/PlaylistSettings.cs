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
    public class PlaylistSettings
    {

        public PlaylistSettings()
        {
            SavePlaylist = true;
            LoadPlaylistSettings = true;
            LoadPrevPlaylist = true;
            SavePlaylistSettings = true;
            SettingsVisible = false;
            IsGroupingEnabled = true;
        }

        public int RepeatMode { get; set; }

        public bool SavePlaylistSettings { get; set; }

        public bool SavePlaylist { get; set; }

        public bool LoadPlaylistSettings { get; set; }

        public bool LoadPrevPlaylist { get; set; }

        public bool IsGroupingEnabled { get; set; }

        public bool SettingsVisible { get; set; }
    }
}
