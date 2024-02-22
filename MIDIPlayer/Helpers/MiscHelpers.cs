using Common;
using Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm
{
    public class MiscHelpers
    {

        public static string ParseAnnouncementText(string text, string midiTitle)
        {
            text = text.Replace("{", "").Replace("}", "").Replace("[title]", midiTitle);

            var tokens = text.Split(new char[] { '|' });

            if (tokens.IsNullOrEmpty())
                return text;

            int randToken = new Random().Next(0, tokens.Length);

            return tokens[randToken];
        }

    }
}
