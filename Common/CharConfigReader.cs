using Common;
using Common.Helpers;
using Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CharConfigReader
    {
        public static CharacterConfig Load()
        {
            string filePath = Path.Combine(AppHelpers.GetAppAbsolutePath(), "characters.config");

            var charConfig =  FileHelpers.Load<CharacterConfig>(filePath);

            return charConfig;
        }
    }
}
