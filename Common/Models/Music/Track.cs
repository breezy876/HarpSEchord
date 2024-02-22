
using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melanchall.DryWetMidi.Core;
using Newtonsoft.Json;
using Common.Helpers;

namespace Common.Music
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Track
    {

        public Track Clone()
        {
            var track = new Track();

            track.OctaveOffset = OctaveOffset;
            track.KeyOffset = KeyOffset;
            track.Title = Title.ToString();
            track.Index = Index;
            track.EnsembleMember = EnsembleMember;
            track.EnsembleInstrument = EnsembleInstrument;
            track.PlayAll = PlayAll;
            track.ReduceMaxNotes = ReduceMaxNotes;
            track.HoldLongNotes = HoldLongNotes;
            track.Muted = Muted;
            track.Cloned = true;

            track.HighestChordSize = HighestChordSize;
            track.TotalChords = TotalChords;
            track.TotalNotes = TotalNotes;
            track.IsPercussion = IsPercussion;
            track.PercussionNote = PercussionNote;

            track.HighestNote = HighestNote;
            track.LowestNote = LowestNote;
            track.TotalNotes = TotalNotes;
            track.Instrument = Instrument;

            return track;
        }

        public Track()
        {
            OctaveOffset = 0;
            KeyOffset = 0;
            TimeOffset = 0;
            EnsembleMember = 0;
            AutofilledInstrument = "None";
            EnsembleInstrument = "None";
            PlayAll = true;
            ReduceMaxNotes = 2;
            Enabled = true;
            HoldLongNotes = true;
        }

        public Track(string name, int index, int highestNote, int lowestNote, int totalNotes ) : base()
        {
            HighestNote = highestNote;
            LowestNote = lowestNote;
            TotalNotes = totalNotes;

            EnsembleMember = 0;
            AutofilledInstrument = "None";
            EnsembleInstrument = "None";
            PlayAll = true;
            ReduceMaxNotes = 2;
            HoldLongNotes = true;

            Title = name.ToString();
   
            Index = index;
            Enabled = true;

        }

        [JsonProperty]
        public int HighestNote { get; set; }
        [JsonProperty]
        public int LowestNote { get; set; }

        [JsonProperty]
        public bool Enabled { get; set; }

        public int Index { get; set;  }

        [JsonProperty]
        public bool IsSplit { get; set; }

        [JsonProperty]
        public int? ParentIndex { get; set; }

        [JsonProperty]
        public int? PercussionNote { get; set; }

        [JsonProperty]
        public int OctaveOffset { get; set; }

        [JsonProperty]
        public int TimeOffset { get; set; }

        [JsonProperty]
        public int KeyOffset { get; set; }

        [JsonProperty]
        public string Title { get; set; }

        [JsonProperty]
        public string Instrument { get; set; }

        [JsonProperty]
        public bool Muted { get; set; }

        public bool HasChanged { get; set; }

        [JsonProperty]
        public int? AutofilledMember { get; set; }

        [JsonProperty]
        public int? EnsembleMember { get; set; }

        [JsonProperty]
        public string AutofilledInstrument { get; set; }

        [JsonProperty]
        public string EnsembleInstrument { get; set; }


        [JsonProperty]
        public bool HoldLongNotes { get; set; }

        [JsonProperty]
        public int ReduceMaxNotes { get; set; }

        [JsonProperty]
        public bool HighestOnly { get; set; }

        [JsonProperty]
        public bool PlayAll { get; set; }

        public int HighestChordSize { get; set; }

        public int TotalChords { get; set; }

        [JsonProperty]
        public int TotalNotes { get; set; }

        [JsonProperty]
        public bool IsPercussion { get; set; }

        [JsonProperty]
        public bool Cloned { get; set; }

        public void Reset()
        {
            OctaveOffset = 0;
            KeyOffset = 0;
            TimeOffset = 0;
            EnsembleMember = -1;
            AutofilledInstrument = "None";
            EnsembleInstrument = "None";
            PlayAll = true;
            ReduceMaxNotes = 2;
            HoldLongNotes = true;
            Muted = false;
            Enabled = true;
        }

        public string Range => Common.Midi.Helpers.GetNoteRangeText(this);
    }
}
