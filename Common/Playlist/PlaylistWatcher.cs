using Common.Helpers;
using Common.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Playlist
{
    public class PlaylistWatcher : IDisposable
    {

        FileWatcher watcher;

        public event EventHandler<string> OnFileDeleted;
        public event EventHandler<string> OnFileChanged;
        public event EventHandler<string> OnFileAdded;

        public void Dispose()
        {
            if (watcher == null)
                return;

            Stop();
            watcher.Dispose();
        }

        public void Stop()
        {
            if (watcher == null)
                return;

            watcher.Deleted -= FileDeleted;
            watcher.Created -= FileAdded;
            watcher.Changed -= FileChanged;
        }

        public void Start()
        {
            watcher = new FileWatcher();
            watcher.Create(AppHelpers.GetAppAbsolutePath(Paths.MidiFilePath));
            watcher.Filter = "*.mid, *.midi";
            watcher.Recursive = true;
            watcher.Deleted += FileDeleted;
            watcher.Created += FileAdded;
            watcher.Changed += FileChanged;
            watcher.Start();
        }

        private void FileChanged(object sender, FileSystemEventArgs args)
        {
            OnFileChanged(sender, args.FullPath);
        }

        private void FileDeleted(object sender, FileSystemEventArgs args)
        {
            OnFileDeleted(sender, args.FullPath);
        }

        private void FileAdded(object sender, FileSystemEventArgs args)
        {
            OnFileAdded(sender, args.FullPath);
        }
    }
}
