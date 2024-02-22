
using Common.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Common
{
    public class KeyHelpers
    {

        public static int GetVirtualKey(Key key) {
            return KeyInterop.VirtualKeyFromKey(key);
        }

        public static bool IsPerformanceKey(KeySettings keySettings, Key key)
        {
            return keySettings.KeyMappings.Any(k => k == key);
        }

        public static int? GetVkCode(int noteVal, KeySettings keySettings)
        {
           return noteVal < keySettings.KeyMappings.Length ? KeyInterop.VirtualKeyFromKey(keySettings.KeyMappings[noteVal]) : (int?)null;
        }

        public static string ToString(Key keyCode)
        {
            //var keyNames = new Dictionary<Key, string>()
            //{
  
            //}
            return keyCode.ToString().Replace("VK", "").Replace("_", "");
        }


    }
}
