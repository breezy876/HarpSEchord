using Common.FFXIV;
using Common.Models.Ensemble;
using Common.Models.FFXIV;
using Common.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using static Common.FFXIV.Enums;
using static Common.FFXIV.FFXIVHotbarDat;
using static Common.FFXIV.FFXIVKeybindDat;

namespace Common.Helpers
{
    //credits goes to Paru - BMP author for this code
    public class PerformanceHelpers
    {
        private const string InstrumentMapFileName = "instruments.config";

        private const string InstrumentCompensationFileName = "compensation.config";
        //credits to Akira/Ori

        public static Instrument GetInstrumentIDByName(string name)
        {
            if (string.IsNullOrEmpty(name) || name.ToLower().StartsWith("untitled"))
                return Instrument.None;

            if (name.Contains("+"))
            {
                string[] split = name.Split('+');
                if (split.Length > 0)
                {
                    name = split[0];
                }
            }
            else if (name.Contains("-"))
            {
                string[] split = name.Split('-');
                if (split.Length > 0)
                {
                    name = split[0];
                }
            }

            name = name.ToLower().Replace("\0","");
            name = String.Concat(name.Where(c => !Char.IsWhiteSpace(c)));

            return GetInstrumentFromName(name);
        }

        public static int GetTransposeByName(string name)
        {
            int octave = 0;
            if (name.Contains("+"))
            {
                string[] split = name.Split('+');
                if (split.Length > 1)
                {
                    Int32.TryParse(split[1], out octave);
                }
            }
            else if (name.Contains("-"))
            {
                string[] split = name.Split('-');
                if (split.Length > 1)
                {
                    Int32.TryParse(split[1], out octave);
                    octave = -octave;
                }
            }

            //PluginLog.LogDebug("Transpose octave: " + octave);
            return octave;
        }

        public static Instrument GetInstrument(int ins)
        {
            return (Instrument)ins;
        }

        public static string GetInstrumentDisplayName(string insName)
        {
            if (string.IsNullOrEmpty(insName) || insName.Equals("None"))
                return "Harp";

            if (insName.Equals("Guitar (Overdriven)") || insName.Equals("ElectricGuitarOverdriven"))
                return "Electric Guitar: Overdriven";

            if (insName.Equals("Guitar (Clean)") || insName.Equals("ElectricGuitarClean"))
                return "Electric Guitar: Clean";

            if (insName.Equals("Guitar (Muted)") || insName.Equals("ElectricGuitarMuted"))
                return "Electric Guitar: Muted";

            if (insName.Equals("Guitar (Distorted)") || insName.Equals("ElectricGuitarPowerChords"))
                return "Electric Guitar: Power Chords";

            if (insName.Equals("ElectricGuitarSpecial"))
                return "Electric Guitar: Special";

            if (insName.Equals("DoubleBass"))
                return "Double Bass";


            if (insName.Equals("BassDrum"))
                return "Bass Drum";


            if (insName.Equals("SnareDrum"))
                return "Snare Drum";


            return insName;
        }

        public static Instrument GetInstrumentFromName(string insName)
        {
            if (string.IsNullOrEmpty(insName) || insName == "None")
                return Instrument.None;

            if (insName.Equals("Guitar (Overdriven)"))
                return Instrument.ElectricGuitarOverdriven;

            if (insName.Equals("Guitar (Clean)"))
                return Instrument.ElectricGuitarClean;

            if (insName.Equals("Guitar (Muted)"))
                return Instrument.ElectricGuitarMuted;

            if (insName.Equals("Guitar (Distorted)"))
                return Instrument.ElectricGuitarPowerChords;

            Instrument result;
            Enum.TryParse(insName, true, out result);
            return result;
        }

        public static bool IsHeldInstrument(Instrument instrument)
        {
            return instrument != Instrument.None &&
                instrument == Instrument.Clarinet ||
                instrument == Instrument.Fife ||
                instrument == Instrument.Flute ||
                instrument == Instrument.Oboe ||
                instrument == Instrument.Panpipes ||
                instrument == Instrument.Horn ||
                instrument == Instrument.Saxophone ||
                instrument == Instrument.Trumpet ||
                instrument == Instrument.Trombone ||
                instrument == Instrument.Tuba ||
                instrument == Instrument.DoubleBass ||
                instrument == Instrument.Cello ||
                instrument == Instrument.Viola ||
                instrument == Instrument.Violin ||
                instrument == Instrument.ElectricGuitarClean ||
                instrument == Instrument.ElectricGuitarOverdriven ||
                instrument == Instrument.ElectricGuitarPowerChords ||
                instrument == Instrument.ElectricGuitarMuted;

        }


        public static Instrument RandomInstrument(Instrument[] instruments)
        {
            var rand = new Random();
            return instruments[rand.Next(0, instruments.Length)];
        }

        private static string GetRandomInstrument(string[] instruments)
        {
            if (instruments.IsNullOrEmpty())
                return null;

            if (instruments.Length == 0)
                return instruments[0];

    
                int randIndex = new Random().Next(instruments.Length);

                return instruments[randIndex];

        }

        //TODO read instrument mappings from JSON
        public static Instrument GetInstrumentFromMidi(InstrumentMap insMap, string insName)
        {
            if (string.IsNullOrEmpty(insName) || insName.ToLower().StartsWith("untitled"))
                return Instrument.None; 

            insName = insName.ToLower().Trim();

            //piano + organ

            if (insName == "harpsichord")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Harpsichord));

            else if (insName == "clavi")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Clavi));

            else if (insName.Contains("piano"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Piano));

            else if (insName == "clarinet")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Clarinet));

            else if (insName == "harmonica")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Harmonica));

            else if (insName.Contains("organ"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Organ));

            else if (insName.Contains("accordion"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Accordion));

            //harp
            else if (insName.Contains("harp"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Harp));


            //brass section
            else if (insName == "bassoon")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Bassoon));

            else if (insName.Contains("brass"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Brass));

            else if (insName.Contains("sax"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Sax));

            else if (insName.Contains("trumpet"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Trumpet));

            else if (insName.Contains("horn"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Horn));

            else if (insName.Contains("tuba"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Tuba));

            else if (insName.Contains("trombone"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Trombone));

            //bass + guitar
            else if (insName.Contains("acoustic guitar"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.AcousticGuitar));

            else if (insName.Contains("guitar harmonics"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.GuitarHarmonics));

            else if (insName.Contains("overdriven guitar"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.OverdrivenGuitar));

            else if (insName.Contains("distortion guitar"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.DistortedGuitar));

            else if (insName.Contains("electric guitar (clean)"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.CleanGuitar));

            else if (insName.Contains("electric guitar (muted)"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.MutedGuitar));

            else if (insName.Contains("electric guitar (jazz)"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.JazzGuitar));

            else if (insName.Contains("electric guitar"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.CleanGuitar));

            else if (insName.Contains("bass"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Bass));


            //pipe section

            else if (insName == "piccolo")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Piccolo));

            else if (insName == "recorder")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Recorder));

            else if (insName == "whistle")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Whistle));

            else if (insName == "blown bottle")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.BlownBottle));

            else if (insName == "ocarina")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Ocarina));

            else if (insName == "shakuhachi")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Shakuhachi));

            else if (insName == "pan flute")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.PanFlute));

            else if (insName.Contains("flute"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Flute));


            //chromatic section
            else if (insName == "celesta")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Celesta));

            else if (insName == "glockenspiel")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Glockenspiel));

            else if (insName == "vibraphone")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Vibraphone));

            else if (insName == "marimba")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Marimba));

            else if (insName == "xylophone")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Xylophone));

            else if (insName == "dulcimer")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Dulcimer));

            else if (insName.Contains("bells"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Bells));

            //percussion section
            else if (insName == "steel drums")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.SteelDrums));

            else if (insName == "melodic tom")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.MelodicTom));

            else if (insName == "takio drum")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.TakioDrum));

            else if (insName == "synth drum")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.SynthDrum));

            else if (insName == "agogo")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Agogo));

            else if (insName.Contains("cymbal"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Cymbal));


            //strings section

            else if (insName == "violin")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Violin));

            else if (insName == "viola")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Viola));

            else if (insName == "cello")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Cello));

            else if (insName == "contrabass")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Contrabass));

            else if (insName.Contains("pizzicato"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Pizzicato));

            else if (insName.Contains("string"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.String));


            else if (insName.Contains("choir"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Choir));

            else if (insName.Contains("voice"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Voice));

            //ethnic section

            else if (insName == "banjo")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Banjo));

            else if (insName == "shamisen")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Shamisen));

            else if (insName == "koto")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Koto));

            else if (insName == "sitar")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Sitar));

            else if (insName == "kalimba")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Kalimba));

            else if (insName == "bag pipe")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.BagPipe));

            else if (insName == "fiddle")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Fiddle));

            else if (insName == "shanai")
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Shanai));


            else if (insName.Contains("pad"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Pad));

            else if (insName.Contains("lead"))
                return (Instrument)GetInstrumentFromName(GetRandomInstrument(insMap.Lead));

            else
            {
                Instrument result;

                bool success = Enum.TryParse<Instrument>(insName, out result);

                if (success)
                    return result;
            }

            return Instrument.None;

        }

        public static InstrumentCompensation LoadInstrumentCompensation()
        {
            var filePath = $"{AppHelpers.GetAppAbsolutePath()}\\{InstrumentCompensationFileName}";
            var comp = FileHelpers.Load<InstrumentCompensation>(filePath);

            return comp;
        }


        public static InstrumentMap LoadInstrumentMap()
        {
            var filePath = $"{AppHelpers.GetAppAbsolutePath()}\\{InstrumentMapFileName}";
            var insMap = FileHelpers.Load<InstrumentMap>(filePath);

            return insMap;
        }

        public static string[] LoadInstrumentMapFromList()
        {
            var filePath = $"{AppHelpers.GetAppAbsolutePath()}\\{InstrumentMapFileName}";
            var insMap = FileHelpers.Load<string[]>(filePath);

            return insMap;
        }

        //public static Instrument[] LoadInstruments()
        //{
        //    var filePath = $"{AppHelpers.GetAppAbsolutePath()}\\{InstrumentMapFileName}";
        //    var insMap = FileHelpers.Load<string[]>(filePath);

        //    if (insMap.IsNullOrEmpty())
        //        return null;

        //    return insMap.Select(i => (Instrument)GetInstrumentFromName(i)).ToArray();
        //}
    }
}
