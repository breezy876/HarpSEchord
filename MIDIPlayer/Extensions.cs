using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm
{
    public static class Extensions
    {
        public static string ToAbsolutePath(this string path)
        {
            return Path.Combine(Common.Helpers.AppHelpers.GetAppAbsolutePath(), path);
        }
    }
}
