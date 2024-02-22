using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Common.Models.Settings
{

    [JsonObject(MemberSerialization.OptOut)]
    public class EnsembleSettings
    {

        public EnsembleSettings()
        {
            ConductorCanPlay = true;

            Mode = EnsembleMode.Local;

            IsVisible = false;

            MaxThreads = 10;
        }

        public bool ConductorCanPlay { get; set; }

        //public bool ConductorCanSendKeyPresses { get; set; }

        public EnsembleMode Mode { get; set; }

        public string PrevConductor { get; set; }

        public string PrevLocalConductor { get; set; }

        public string CharacterName { get; set; }

        public string WorldName { get; set; }

        public bool HoldLongNotes { get; set; }

        public bool ThreadPerInstance { get; set; }

        public bool IsVisible { get; set; }

        public int MaxThreads { get; set; }

        public bool AutoFillFromMidi { get; set; }

        public bool RememberPrevious { get; set; }

        public bool AutoFillFromSettings { get; set; }

        public Common.TackAutofillType AutofillType { get; set; }
    }
}
