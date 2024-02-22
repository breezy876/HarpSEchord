using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.Settings
{
    public class OctaveSettings
    {

        public static Dictionary<int, string> Octaves { get; } = new Dictionary<int, string>()
        {
            {1, "1"},
            {2, "2"},
            {3, "3"},
        };

        public static Dictionary<int, int[]> DefaultOctaveMappings { get; } = new Dictionary<int, int[]>()
            {
                { 2, new [] { 1, 2 } },
                { 3, new [] { 1, 2, 3 } },
                { 4, new[] { 1, 1, 2, 3 } },
                { 5, new[] { 1, 1, 2, 2, 3 } },
                { 6, new[] { 1, 1, 2, 2, 3, 3 } },
                { 7, new[] { 1, 1, 1, 2, 2, 3, 3 } },
                { 8, new[] { 1, 1, 1, 2, 2, 2, 3, 3 } },
                { 9, new[] { 1, 1, 1, 2, 2, 2, 3, 3, 3 } },
                { 10, new[] { 1, 1, 1, 1, 2, 2, 2, 3, 3, 3 } },
            };


        //public static Dictionary<int, int[]> DefaultOctaveMappings { get; } = new Dictionary<int, int[]>()
        //    {
        //        { 2, new [] { 1, 2 } },
        //        { 3, new [] { 1, 2, 3 } },
        //        { 4, new[] { 1, 2, 3, 4 } },
        //        { 5, new[] { 1, 2, 3, 4, 5 } },
        //        { 6, new[] { 1, 1, 2, 3, 4, 5 } },
        //        { 7, new[] { 1, 1, 2, 2, 3, 4, 5 } },
        //        { 8, new[] { 1, 1, 2, 2, 3, 3, 4, 5 } },
        //        { 9, new[] { 1, 1, 2, 2, 3, 3, 4, 4, 5 } },
        //        { 10, new[] { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5 } },
        //    };
    }
}
