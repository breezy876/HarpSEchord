using Common.Music;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.Playlist
{
    public class PlaylistItem : IEquatable<PlaylistItem>
    {
       public MidiSequence Sequence { get; set; }

        public string Title => Sequence.Info.Title;


        //public int Index
        //{
        //    get => Sequence.Index;
        //    set { Sequence.Index = value; }
        //}
        public int Index { get; set; }


        public bool IsPlaying { get; set; }
        public bool IsFiltered { get; set; }

        public bool Equals(PlaylistItem other)
        {
            return this.Sequence.Info.FilePath == other.Sequence.Info.FilePath;
        }
    }
}
