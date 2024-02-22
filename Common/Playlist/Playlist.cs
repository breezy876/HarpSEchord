using Common.Helpers;
using Common.Models.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Playlist
{
    public class Playlist
    {
        private const string PlaylistDefaultFileName = "playlist.pl";

        private static string GetDefaultPlaylistFilePath()
        {
            return Path.Combine($"Playlists", PlaylistDefaultFileName);
        }

        public static void Clear()
        {
            Common.Settings.Playlist.Clear();
            Common.Settings.PlaylistSettings.Clear();
        }


        public static async Task OpenPrevPlaylist()
        {
            var defPlaylistPath = GetDefaultPlaylistFilePath();

            var prevPlaylistPath = Settings.AppSettings.PrevPlaylistFileName;

            var filePath = !string.IsNullOrEmpty(prevPlaylistPath) ? prevPlaylistPath : defPlaylistPath;

            await OpenPlaylist(filePath, Common.Settings.AppSettings.PlaylistSettings.LoadPlaylistSettings);
        }


        private static async Task LoadPlaylistSettings(Models.Playlist.Playlist playlist)
        {
            if (string.IsNullOrEmpty(playlist.SettingsFile))
                return;

            string settingsFile = Path.Combine(AppHelpers.GetAppAbsolutePath(), playlist.SettingsFile);

            var playlistSettings = await Task.Run(() => FileHelpers.Load<SongSettings>(settingsFile));

            if (playlistSettings != null)
                Common.Settings.PlaylistSettings = playlistSettings;
        }

        private static async Task OpenPlaylist(string playlistFilePath, bool loadSettings = true)
        {
            Common.Settings.Playlist.Title = Path.GetFileNameWithoutExtension(playlistFilePath);

            if (!File.Exists(playlistFilePath))
                return;

            Settings.AppSettings.PrevPlaylistFileName = AppHelpers.GetAppRelativePath(playlistFilePath);

            var playlist = await Task.Run(() => FileHelpers.Load<Models.Playlist.Playlist>(playlistFilePath));

            if (playlist == null || !playlist.HasFiles)
                return;

            Common.Settings.Playlist = playlist;

            if (loadSettings)
                await LoadPlaylistSettings (playlist);
        }

    }
}
