using Common.FFXIV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.FFXIV.Enums;
using static Common.FFXIV.FFXIVKeybindDat;

namespace Common.Helpers
{
    public class KeybindHelpers
    {
        private static Common.FFXIV.Keybind ToKeybind(FFXIVKeybindDat.Keybind keybind)
        {
            var newKeybind = new Common.FFXIV.Keybind()
            {
                Key = keybind.GetKey(),
                MainKey1 = FFXIVKeybindDat.Keybind.GetMapped(keybind.mainKey1),
                MainKey2 = FFXIVKeybindDat.Keybind.GetMapped(keybind.mainKey2),
                ModKey1 = keybind.modKey1,
                ModKey2 = keybind.modKey2
            };

            return newKeybind;
        }

        public static Dictionary<int, Common.FFXIV.Keybind> GetEmoteKeybinds(string charId)
        {
            var hotbars = new FFXIVHotbarDat();
            var keybinds = new FFXIVKeybindDat();

            hotbars.LoadHotbarDat(charId);
            keybinds.LoadKeybindDat(charId);

            var slots = hotbars.GetSlotsFromType(6);

            var emoteMap = new Dictionary<int, Common.FFXIV.Keybind>();

            foreach (var slot in slots)
            {
                var keybind = keybinds[slot.ToString()];

                var newKeybind = ToKeybind(keybind);

                if (!emoteMap.ContainsKey(slot.action))
                    emoteMap.Add(slot.action, newKeybind);
            }

            return emoteMap;
        }

        public static Common.FFXIV.Keybind[] GetPerformanceKeybinds(FFXIVKeybindDat keybinds)
        {
            return keybinds.GetPerformanceKeybinds().Select(k => ToKeybind(k)).ToArray();
        }

        public static Dictionary<string, Common.FFXIV.Keybind> GetMiscKeybinds(FFXIVKeybindDat keybinds)
        {
            var keybind1 = ToKeybind(keybinds["OK"]);
            var keybind2 = ToKeybind(keybinds["VIRTUAL_PAD_SELECT"]);
            var keybind3 = ToKeybind(keybinds["VIRTUAL_PAD_START"]);
            var keybind4 = ToKeybind(keybinds["VIRTUAL_PAD_LEFT"]);
            var keybind5 = ToKeybind(keybinds["ESC"]);
            var keybind6 = ToKeybind(keybinds["LEFT"]);

            return new Dictionary<string, Common.FFXIV.Keybind>
            {
                { "OK", keybind1 },
                { "VIRTUAL_PAD_SELECT", keybind2 },
                { "VIRTUAL_PAD_START", keybind3 },
                { "VIRTUAL_PAD_LEFT", keybind4 },
                { "ESC", keybind5 },
                { "LEFT", keybind6 }
            };
        }


        public static Dictionary<Instrument, Common.FFXIV.Keybind> GetInstrumentkeybinds(FFXIVKeybindDat keybinds, FFXIVHotbarDat hotbars)
        {

            var slots = hotbars.GetPerformanceSlots();

            var instrumentMap = new Dictionary<Instrument, Common.FFXIV.Keybind>();

            foreach (var slot in slots)
            {
                var ins = (Instrument)slot.action;

                var keybind = keybinds[slot.ToString()];

                var newKeybind = ToKeybind(keybind);

                if (!instrumentMap.ContainsKey(ins))
                    instrumentMap.Add(ins, newKeybind);
            }

            return instrumentMap;

        }
    }
}
