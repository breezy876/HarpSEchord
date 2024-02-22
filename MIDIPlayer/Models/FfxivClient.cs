using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.Models.Ffxiv
{
        [Serializable]
        public class FfxivClient
        {
            public int Index { get; set; }

            public string CharacterName { get; set; }

            public bool IsConnected { get; set; }
        }
}
