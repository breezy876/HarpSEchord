using Common.FFXIV;
using Melanchall.DryWetMidi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Common.FFXIV.Enums;

namespace Common
{
    public class PlayerGameClientInfo
    {
        public IntPtr WindowHandle { get; set; }

        public bool InstrumentEquipped { get; set; }

        public bool ChatInputOpen { get; set; }

        public bool IsSelected { get; set; }

        public bool IsHeldInstrument { get; set; }

        public Common.FFXIV.Keybind[] Keybinds { get; set; }

        public Instrument Instrument { get; set; }
    }

    [Serializable]
    public class GameClientInfo
    {
        public GameClientInfo()
        {
            InstrumentKeyBinds = new Dictionary<Instrument, Common.FFXIV.Keybind>();

            EmoteKeyBinds = new Dictionary<int, Common.FFXIV.Keybind>();

            MiscKeybinds = new Dictionary<string, Common.FFXIV.Keybind>();
        }
        public Instrument Instrument { get; set; }

        public Common.FFXIV.Keybind InstrumentKeybind { get; set; }

        public string CharacterName { get; set; }

        public string WorldName { get; set; }

        public string CharacterId { get; set; }

        public IntPtr WindowHandle { get; set; }

        public Int64 Id { get; set; }

        public int Index { get; set; }

        public bool IsConductor { get; set; }

        public bool InstrumentEquipped { get; set; }

        public bool ChatInputOpen { get; set; }

        public bool LoggedIn { get; set; }

        public bool PausedOnUneqip { get; set; }

        public bool IsSelected { get; set; }

        public bool EnsembleStarted { get; set; }

        public bool ReadyCheckSent { get; set; }

        public bool IsHeldInstrument { get; set; }

        public bool LoopOnEnd { get; set; }


        public Dictionary<Instrument, Common.FFXIV.Keybind> InstrumentKeyBinds { get; set; }

        public Dictionary<int, Common.FFXIV.Keybind> EmoteKeyBinds { get; set; }

        public Dictionary<string, Common.FFXIV.Keybind> MiscKeybinds { get; set; }

        public Keybind[] PerformanceKeybinds { get; set; }

        public bool Finished { get; set; }

        public bool AutomationStoppedOnUnequip { get; set; }

        public bool AutomationStoppedOnChatOpen { get; set; }

        public bool ShiftKeyDown { get; set; }

        public bool CtrlKeyDown { get; set; }

        public uint? ActorId { get; set; }

        public int ProcessId { get; set; }

        public bool EnsemblePacketReceived { get; set; }
    }
}
