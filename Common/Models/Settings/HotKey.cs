
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Common.Models.Settings
{
    public class HotKey
    {
        public HotKey(Key val, HotKeyType type)
        {
            Type = type;
            Text = KeyHelpers.ToString(val);
            Value = val;
        }

        public bool IsEnabled { get; set; }

        public HotKeyType Type { get; set; }

        public string Text { get; set; }

        public Key Value { get; set; }
    }
}
