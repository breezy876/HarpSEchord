using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.FFXIV.Enums;

namespace Common.Models.FFXIV
{
    public class PerformanceInfo
    {
        public bool IsHeldInstrument { get; set; }

        public Instrument Instrument { get; set; }

        public bool InstrumentEquipped { get; set; }
    }
}
