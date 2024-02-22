using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class CharacterConfig
    {
        public CharacterConfig()
        {
            Characters = new Dictionary<string, bool>();
        }

        public Dictionary<string, bool> Characters { get; set; }


        public Dictionary<string, int> ToDictionary() =>
            Characters.Keys.IsNullOrEmpty() ? null :
            Characters.Keys.Select((c, i) => new { CharName = c, Index = i })
            .ToDictionary(c => c.CharName, c => c.Index);
    }
}
