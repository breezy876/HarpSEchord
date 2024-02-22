
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Specialized;
using Common;
using Common.Helpers;

namespace Common.Models.Playlist
{

    public enum RepeatMode {  Single, SingleRepeat, ListOrdered, ListRepeat, Shuffle }


    [JsonObject(MemberSerialization.OptIn)]
    public class Playlist
    {

        private List<PlaylistItem> items;

        private bool isFiltered;

        private int prevIndex;

        [JsonProperty]
        public int SelectedIndex { get; set; }

        [JsonProperty]
        public string SettingsFile { get; set; }
        
        [JsonProperty]
        public string Title { get; set; }

        public PlaylistItem Current => GetCurrent();

        public int CurrentIndex { get; private set; }

        public List<PlaylistItem> Items { get; private set; }

        public int Total => this.Items.Count;

        public string FilePath { get; set; }

        public string FullPath => GetFullPath();

        public bool HasItems => this.Total > 0;

        public bool HasFiles => this.Files.Count > 0;

        [JsonProperty]
        public List<string> Files { get; set; }

        public RepeatMode RepeatMode { get; set; }

        public bool IsFiltered { get; set; }




        public Playlist() 
        {
            this.Title = "playlist";
            this.FilePath = GetDefaultFilePath();
            this.Items = new List<PlaylistItem>();
            this.Files = new List<string>();
            this.CurrentIndex = 0;
            this.RepeatMode = RepeatMode.Single;
            this.SettingsFile = Path.Combine(GetDefaultFilePath(), "playlist.settings.json");
        }
        
        public Playlist(string title) : this()
        {
            this.Title = title;
        }

        public Playlist(string title, IEnumerable<PlaylistItem> items) : this(title)
        {
            this.Items = new List<PlaylistItem>(items);
            this.Files = new List<string>(items.Select(i => i.Sequence.Info.FilePath));
        }

        public static string GetDefaultFilePath()
        {
            return $"Playlists";
        }

        public PlaylistItem GetByIndex(int index)
        {
            return this.Items[index];
        }

        public void Sort()
        {
            this.Items = this.Items.OrderBy(i => i.Title).ToList();
        }

        public void ClearFilter()
        {
            isFiltered = false;

            if (this.items.IsNullOrEmpty())
                return;

            this.Items = new List<PlaylistItem>(this.items);

            this.Files = this.Items.Select(i => i.Title).ToList();

            UpdateIndexes();

            CurrentIndex = prevIndex;
        }

        public void Filter(IEnumerable<string> titles)
        {
            prevIndex = this.CurrentIndex;

            if (!isFiltered)
            {
                this.items = new List<PlaylistItem>(this.Items);

                isFiltered = true;
            }

            this.Items = this.items.Where(i => titles.Contains(i.Title)).ToList();

            this.Files = this.Items.Select(i => i.Title).ToList();

            UpdateIndexes();

            this.CurrentIndex = this.Items.First().Index;
        }


        public void Add(IEnumerable<PlaylistItem> items)
        {
            this.Items.AddRange(items);
            this.Files.AddRange(items.Select(i => i.Sequence.Info.FilePath));
        }

        public void Add(PlaylistItem item)
        {
            this.Items.Add(item);
            this.Files.Add(item.Sequence.Info.FilePath);
        }

        public void Remove(PlaylistItem item)
        {
            this.Items.Remove(item);

            this.Files.Remove(item.Sequence.Info.FilePath);

            //UpdateIndexes();

            //CurrentIndex++;

        }

        public void Remove(IEnumerable<PlaylistItem> items)
        {

            this.Items.RemoveAll(i => items.Contains(i));

            this.Files.RemoveAll(f => items.Select(i => i.Sequence.Info.FilePath).Contains(f));

            //int lastIndex = items.Last().Index;

            //foreach (var item in Items)
            //{
            //    if (item.Index > lastIndex)
            //        item.Index -= items.Count();
            //}

            //UpdateIndexes();

            //CurrentIndex += items.Count();

        }

        public void Clear()
        {
            this.CurrentIndex = 0;
            this.Items.Clear();
            this.Files.Clear();
        }

        public void MoveTo(int index)
        {
            this.CurrentIndex = index;
        }

        public void Change(string title)
        {
            var item =  this.Items.FirstOrDefault(i => i.Sequence.Info.Title == title);
            this.CurrentIndex = item.Index;
        }

        public PlaylistItem GetByTitle(string title)
        {
            return this.Items.FirstOrDefault(i => i.Sequence.Info.Title == title);
        }

        public PlaylistItem GetCurrent()
        {
            if (this.CurrentIndex > this.Items.Count - 1)
                this.CurrentIndex = this.Items.Count - 1;

            return this.Items.Any() ? this.Items[this.CurrentIndex] : null;
        }

        public void PrepareForSave()
        {
            Files = Files.Select(f => AppHelpers.GetAppRelativePath(f)).ToList();
        }


        private PlaylistItem GetAt(int index)
        {
            return this.Items[index];
        }

        private string GetFullPath()
        {
            return Path.Combine(this.FilePath, this.Title + ".pl");
        }

        private void UpdateIndexes()
        {
            int index = 0;

            foreach (var item in this.Items)
            {
                item.Index = index;
                index++;
            }
        }

    }
}
