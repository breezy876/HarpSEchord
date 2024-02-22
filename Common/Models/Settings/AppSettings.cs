
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Common.Models.Settings
{
    [JsonObject(MemberSerialization.OptOut)]
    public class AppSettings
    {

        public AppSettings()
        {
            this.GeneralSettings = new GeneralSettings();
            this.TrackSettings = new TrackSettings();
            this.PlaylistSettings = new PlaylistSettings();
            this.AutomationSettings = new AutomationSettings();
        }

        public AutomationSettings AutomationSettings { get; set; }

        public TrackSettings TrackSettings { get; set; }

        public PlaylistSettings PlaylistSettings { get; set; }

        public GeneralSettings GeneralSettings { get; set; }

        public string HSCPluginConfigPath { get; set; }
        public string PluginConfigPath { get; set; }
        public string PrevPlaylistFileName { get; set; }

        public string PrevPlaylistPath { get; set; }


        public string PrevSequenceTitle { get; set; }


        public string PrevMidiPath { get; set; }


        public bool ShowTracks { get; set; }

        public bool ShowLog { get; set; }

        public bool ShowInfo { get; set; }


        public bool ShowAdvancedLayout { get; set; }

        public bool SettingsVisible { get; set; }

        public bool ShowSongSettings { get; set; }

        public bool LoggingEnabled { get; set; }

        public bool IsDev { get; set; }
        public bool ShowFfxiv { get; set; }

        public static AppSettings Create()
        {
            var appSettings = new AppSettings();

            appSettings.SettingsVisible = true;

            appSettings.ShowLog = false;

            appSettings.ShowTracks = true;

            appSettings.ShowFfxiv = true;

            appSettings.ShowInfo = false;

            appSettings.ShowAdvancedLayout = false;

            appSettings.LoggingEnabled = false;


            appSettings.GeneralSettings = new GeneralSettings();
            appSettings.TrackSettings = new TrackSettings();
            appSettings.PlaylistSettings = new PlaylistSettings();
            appSettings.PlaylistSettings.SettingsVisible = false;


            if (appSettings.PrevPlaylistPath.IsNullOrEmpty())
                appSettings.PrevPlaylistPath = Common.Helpers.AppHelpers.GetAppRelativePath(Paths.PlaylistPath);

            if (appSettings.PrevMidiPath.IsNullOrEmpty())
                appSettings.PrevMidiPath = Common.Helpers.AppHelpers.GetAppRelativePath(Paths.MidiFilePath);

            appSettings.PluginConfigPath = $"%AppData%\\XIVLauncher\\pluginConfigs".ExpandEnvironmentVars();

            return appSettings;
        }


    }
}
