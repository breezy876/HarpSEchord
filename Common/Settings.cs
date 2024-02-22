
using Common.Helpers;
using Common.Models;
using Common.Models.FFXIV;
using Common.Models.Playlist;
using Common.Models.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Common
{
    public static class Settings
    {

        private const string AppSettingsFileName = "settings.config";
        private const string InstrumentMapFileName = "instruments.config";
        private const string InstrumentShiftsFileName = "shifts.instruments.config";
        private const string CharConfigFileName = "characters.config";


        static Settings()
        {
            AppSettings = new AppSettings();

            SongSettings = new SongSettings();

            PlaylistSettings = new SongSettings();

            Playlist = new Common.Models.Playlist.Playlist();

            CharConfig = new CharacterConfig();
        }

        public static AppSettings AppSettings { get; private set; }

        public static SongSettings SongSettings { get; private set; }

        public static Common.Models.Playlist.Playlist Playlist { get; set; }

        public static SongSettings PlaylistSettings { get; set; }

        public static InstrumentMap InstrumentMap { get; set; }

        public static InstrumentShifts InstrumentShifts { get; set; }

        public static OctaveShifts OctaveShifts { get; set; }

        public static List<Account> Accounts { get; set; }

        public static CharacterConfig CharConfig { get; set; }

        public static void InitializeAppSettings()
        {
            AppSettings = AppSettings.Create();
        }

        public static async Task LoadAppSettings()
        {

            var filePath = Path.Combine(Common.Helpers.AppHelpers.GetAppAbsolutePath(), AppSettingsFileName);

            if (!File.Exists(filePath)) return;

            var appSettings = await Task.Run(() => FileHelpers.Load<AppSettings>(filePath)) ?? new AppSettings();

            AppSettings = appSettings;

           CharConfig = await Task.Run(() => CharConfigReader.Load());
        }

        //public static void LoadAccounts()
        //{
        //    var filePath = $"{AppHelpers.GetAppAbsolutePath()}\\{AccountsFileName}";
        //    var accounts = FileHelpers.Load<string[]>(filePath);

        //    if (accounts.IsNullOrEmpty())
        //        return;

        //    Accounts = new List<Account>();

        //    foreach(var account in accounts)
        //    {

        //        var args = account.Split(new[] { ':' });
        //        string username = args[0];
        //        string password = args[1];

        //        Accounts.Add(new Account() { Username = username, Password = password });
        //    }
        //}

        public static async Task LoadInstrumentMap()
        {
            var filePath = $"{AppHelpers.GetAppAbsolutePath()}\\{InstrumentMapFileName}";
            var insMap = await Task.Run(() => FileHelpers.Load<InstrumentMap>(filePath));

            if (insMap != null)
                InstrumentMap = insMap;
        }

        public static async Task LoadInstrumentShifts()
        {
            var filePath = $"{AppHelpers.GetAppAbsolutePath()}\\{InstrumentShiftsFileName}";
            var insShift = await Task.Run(() => FileHelpers.Load<InstrumentShifts>(filePath));

            if (insShift != null)
                InstrumentShifts = insShift;
        }

        public static async Task LoadPlaylistSettings()
        {
            var filePath = $"{AppHelpers.GetAppAbsolutePath()}\\{Playlist.SettingsFile}";
            var songSettings = await Task.Run(() => FileHelpers.Load<SongSettings>(filePath));

            if (songSettings != null)
                PlaylistSettings = songSettings;
        }

        public static async Task SaveAppSettings()
        {
            try
            {
                var filePath = $"{AppHelpers.GetAppAbsolutePath()}\\{AppSettingsFileName}";
               await Task.Run(() => FileHelpers.Save(AppSettings, filePath));
                await Task.Run(() => FileHelpers.Save(AppSettings, Path.Combine(Settings.AppSettings.HSCPluginConfigPath, AppSettingsFileName)));
            }
            catch (Exception ex) { }
        }

        public static async Task SaveCharConfig()
        {
            try
            {
                var filePath = $"{AppHelpers.GetAppAbsolutePath()}\\{CharConfigFileName}";
                await Task.Run(() => FileHelpers.Save(CharConfig, filePath));
                await Task.Run(() => FileHelpers.Save(CharConfig, Path.Combine(Settings.AppSettings.HSCPluginConfigPath, CharConfigFileName)));
            }
            catch (Exception ex) { }
        }

        public static async Task SavePlaylistSettings(string filePath = null)
        {
            try
            {

                    //Common.Settings.PlaylistSettings.Settings = Common.Settings.PlaylistSettings.Settings
                    //    .Where(s => Common.Settings.Playlist.Items.Select(i => i.Sequence.Info.Title).Contains(s.Key))
                    //    .ToDictionary(i => i.Key, i => i.Value);

                    await Task.Run(() => FileHelpers.Save(PlaylistSettings, filePath ?? $"{AppHelpers.GetAppAbsolutePath()}\\{Playlist.SettingsFile}"));
              
            }
            catch (Exception ex) { }
        }
    }
}
