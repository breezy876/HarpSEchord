

using Melanchall.DryWetMidi.Interaction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Music
{
    [JsonObject(MemberSerialization.OptOut)]
    public class SequenceTimeSpan
    {

        public TimeSpan Current { get; private set; }

        public SequenceTimeSpan()
        {
            this.Current = new TimeSpan(0);
        }

        public SequenceTimeSpan(long ms)
        {
            this.Current = new TimeSpan(0, 0, 0, 0, (int)ms);
        }

        public SequenceTimeSpan(TimeSpan ts)
        {
            this.Current = ts;
        }

        public override string ToString()
        {
            return this.Current.ToString(@"hh\:mm\:ss");
        }

    }
}
