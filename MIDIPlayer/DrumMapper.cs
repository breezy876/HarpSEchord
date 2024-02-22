using Common;
using Common.Helpers;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.FFXIV.Enums;
using Melanchall.DryWetMidi.Standards;

namespace Hscm
{
    public class DrumMapper
    {
        const string DrumsFile = "drums.json";

        static Dictionary<GeneralMidiPercussion, Instrument> mappings;

        static DrumMapper()
        {
            mappings = new Dictionary<GeneralMidiPercussion, Instrument>() {
                { GeneralMidiPercussion.AcousticSnare, Instrument.SnareDrum },
                { GeneralMidiPercussion.ElectricSnare, Instrument.SnareDrum },
                { GeneralMidiPercussion.BassDrum1, Instrument.BassDrum },
                { GeneralMidiPercussion.AcousticBassDrum, Instrument.BassDrum },
                { GeneralMidiPercussion.LowBongo, Instrument.Bongo },
                { GeneralMidiPercussion.HiBongo, Instrument.Bongo },
                { GeneralMidiPercussion.CrashCymbal1, Instrument.Cymbal },
                { GeneralMidiPercussion.CrashCymbal2, Instrument.Cymbal },
                { GeneralMidiPercussion.SplashCymbal, Instrument.Cymbal },
                { GeneralMidiPercussion.HandClap, Instrument.SnareDrum },
                { GeneralMidiPercussion.OpenHiHat, Instrument.SnareDrum },
                { GeneralMidiPercussion.ClosedHiHat, Instrument.SnareDrum },
                { GeneralMidiPercussion.HighTimbale, Instrument.Timpani },
                { GeneralMidiPercussion.LowTimbale, Instrument.Timpani }
            };
        }
        public async Task SaveMappings()
        {
            var filePath = Path.Combine(AppHelpers.GetAppAbsolutePath(), DrumsFile);
            var fileData = ToTextFormat();
            await Task.Run(() => FileHelpers.Save(fileData, filePath));
        }

        public async Task LoadMappings()
        {
            var filePath = Path.Combine(AppHelpers.GetAppAbsolutePath(), DrumsFile);

            var fileData = await Task.Run(() => FileHelpers.Load<Dictionary<string, string>>(filePath));

            if (!fileData.IsNullOrEmpty())
                CreateMappings(fileData);
        }

        public static bool IsMapped(GeneralMidiPercussion midiDrum) => !mappings.IsNullOrEmpty() && mappings.ContainsKey(midiDrum);

        public static Instrument GetMappedInstrument(GeneralMidiPercussion midiDrum) => mappings.IsNullOrEmpty() || !mappings.ContainsKey(midiDrum) ? Instrument.BassDrum : mappings[midiDrum];

        private static Dictionary<string, string> ToTextFormat()
        {
            var fileData = new Dictionary<string, string>();

            foreach (var kvp in mappings)
            {
                fileData.Add(kvp.Key.ToString(), kvp.Value.ToString());
            }

            return fileData;
        }

        private static void CreateMappings(Dictionary<string, string> fileData)
        {
            foreach(var kvp in fileData)
            {
            
                var midiVal = (GeneralMidiPercussion)Enum.Parse(typeof(GeneralMidiPercussion), kvp.Key);
                var gameVal = (Instrument)Enum.Parse(typeof(Instrument), kvp.Value);
                mappings.Add(midiVal, gameVal);
            }
        }
    }
}
