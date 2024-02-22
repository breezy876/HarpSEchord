using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.Ensemble
{
    [Serializable]
    public class FFXIVCharacter
    {
        public int Index { get; set; }

        public string CharacterName { get; set; }

        public bool IsSelected { get; set; }
    }
}
