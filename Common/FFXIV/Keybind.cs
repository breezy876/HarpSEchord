using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common.FFXIV
{
    [Serializable]
    public class Keybind
    {
        public Keys Key { get; set; }

        public int MainKey1 { get; set; }

        public int MainKey2 { get; set; }

        public int ModKey1 { get; set; }

        public int ModKey2 { get; set; }
    }
}
