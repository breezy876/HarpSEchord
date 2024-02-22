using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.IO
{
    public class FileWatcher : IDisposable
    {
    
        private FileSystemWatcher watcher;

        public event EventHandler<FileSystemEventArgs> Created;
        public event EventHandler<FileSystemEventArgs> Deleted;
        public event EventHandler<FileSystemEventArgs> Changed;

        public DateTime? changedTimestamp;
        public DateTime? createdTimestamp;
        public DateTime? deletedTimestamp;

        public void Create(string filePath)
        {
            watcher = new FileSystemWatcher();
            watcher.Path = filePath;
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime | NotifyFilters.DirectoryName;
        }

        public bool Recursive
        {
            get => watcher.IncludeSubdirectories;
            set { watcher.IncludeSubdirectories = value; }
        }

        public string Filter
        {
            get => watcher.Filter;
            set { watcher.Filter = value; }
        }

        public void Start()
        {
            watcher.Created += HandleFileCreated;
            watcher.Deleted += HandleFileDeleted;
            watcher.Changed += HandleFileChanged;
            watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            if (watcher == null)
                return;

            watcher.Created -= HandleFileCreated;
            watcher.Deleted -= HandleFileDeleted;
            watcher.Changed -= HandleFileChanged;
            watcher.EnableRaisingEvents = false;
        }

        public void Dispose()
        {
            if (watcher == null)
                return;

            watcher.Dispose();
        }

        private void HandleFileDeleted(object sender, FileSystemEventArgs args)
        {
            if (deletedTimestamp == null || DateTime.Now.Subtract(deletedTimestamp.Value).Seconds >= 2)
                Deleted?.Invoke(sender, args);

            deletedTimestamp = DateTime.Now;
        }

        private void HandleFileCreated(object sender, FileSystemEventArgs args)
        {
            if (createdTimestamp == null || DateTime.Now.Subtract(createdTimestamp.Value).Seconds >= 2)
                Created?.Invoke(sender, args);

            createdTimestamp = DateTime.Now;
        }

        private void HandleFileChanged(object sender, FileSystemEventArgs args)
        {
            if (changedTimestamp == null || DateTime.Now.Subtract(changedTimestamp.Value).Seconds >= 2)
                Changed?.Invoke(sender, args);

            changedTimestamp = DateTime.Now;
        }
    }
}
