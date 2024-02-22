
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Common
{

    public enum SizeUnits
    {
        Byte, KB, MB, GB, TB, PB, EB, ZB, YB
    }

    public static class Extensions
    {
        //public static IEnumerable<Music.NoteEvent> Clone(this IEnumerable<Music.NoteEvent> src)
        //{
        //    return src.Select(nEv => nEv.Clone()).ToList();
        //}


        //public static void RemoveWhere<T>(this IList<T> items, IEnumerable<int> indexes) 
        //{
        //    return items.
        //}

        public static IEnumerable<string> GetDirectories(this string path, string filter)
        {
            var dirs = Directory.GetDirectories(path).
                Where((d => d.Contains(filter)));
            return dirs;
        }

        public static string GetMostRecentDirectory(this string path, string filter = null)
        {
            try
            {
                var dirs = Directory.EnumerateDirectories(path)
                    .Select(d => new { path = d, dateCreated = Directory.GetCreationTime(d) });

                if (dirs.IsNullOrEmpty())
                    return null;

                if (dirs.Count() == 1)
                    return dirs.First().path;

                if (!string.IsNullOrEmpty(filter))
                    dirs = dirs.Where(d => d.path.Contains(filter));

                var latestDate = dirs.Select(d => d.dateCreated).Max();
                var latestCreated = dirs.FirstOrDefault(d => d.dateCreated.Equals(latestDate)).path;

                return latestCreated;
            }
            catch (DirectoryNotFoundException)
            {
                return null;
            }
        }

        public static string GetParent(this string path) => Directory.GetParent(path).FullName;

        public static string ExpandEnvironmentVars(this string str) => Environment.ExpandEnvironmentVariables(str);


        public static TSource MaxElement<TSource, R>(this IEnumerable<TSource> container, Func<TSource, R> valuingFoo) where R : IComparable
        {
            var enumerator = container.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new ArgumentException("Container is empty!");

            var maxElem = enumerator.Current;
            var maxVal = valuingFoo(maxElem);

            while (enumerator.MoveNext())
            {
                var currVal = valuingFoo(enumerator.Current);

                if (currVal.CompareTo(maxVal) > 0)
                {
                    maxVal = currVal;
                    maxElem = enumerator.Current;
                }
            }

            return maxElem;
        }

        public static TSource MinElement<TSource, R>(this IEnumerable<TSource> container, Func<TSource, R> valuingFoo) where R : IComparable
        {
            var enumerator = container.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new ArgumentException("Container is empty!");

            var maxElem = enumerator.Current;
            var maxVal = valuingFoo(maxElem);

            while (enumerator.MoveNext())
            {
                var currVal = valuingFoo(enumerator.Current);

                if (currVal.CompareTo(maxVal) < 0)
                {
                    maxVal = currVal;
                    maxElem = enumerator.Current;
                }
            }

            return maxElem;
        }
        public static Dictionary<TKey,List<TVal>> DictionaryGroupBy<TVal, TKey>(
            this IEnumerable<TVal> items, 
            Func<TVal, TKey> selector,
            Func<KeyValuePair<TKey, List<TVal>>, bool> cond = null)
        {
            var groups = new Dictionary<TKey, List<TVal>>();

            foreach (var item in items)
            {
                if (!groups.ContainsKey(selector(item)))
                    groups.Add(selector(item), new List<TVal>() { item });
                else
                    groups[selector(item)].Add(item);
            }

            if (cond == null)
                return groups;

            return groups.AsParallel().Where(g => cond(g)).ToDictionary(g => g.Key, g => g.Value);

        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector)
        {
            var groups = source.GroupBy(selector).Select(grp => new { Key = grp.Key, Value = grp });
            return groups.Select(x => x.Value.First());
        }

        public static int ToInt(this bool boolVal)
        {
            return boolVal ? 1 : 0;
        }

        public static bool ToBool(this int intVal)
        {
            return intVal >= 1;
        }

        public static string JoinWithSpaces(this string[] args)
        {
            return args.IsNullOrEmpty() ? null : String.Join(" ", args);
        }

        public static Point GetPoint(this IntPtr _xy)
        {
            uint xy = unchecked(IntPtr.Size == 8 ? (uint)_xy.ToInt64() : (uint)_xy.ToInt32());
            uint x = unchecked((ushort)xy);
            uint y = unchecked((ushort)(xy >> 16));
            return new Point(x, y);
        }

        public static string ToCsv(this IEnumerable<int> nums)
        {
            return nums.Select(n => n.ToString()).Aggregate((x, y) => x + "," + y);
        }

        public static bool IsMidiFile(this string filePath)
        {
            return Path.GetExtension(filePath) == "mid" || Path.GetExtension(filePath) == "midi";
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> items)
        {
            return items == null || !items.Any();
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static T ToEnum<T>(this int value)
        {
            var name = Enum.GetName(typeof(T), value);
            return name.ToEnum<T>();
        }

        public static void AddIfNotKeyExists<TKey, TVal>(this Dictionary<TKey, TVal> src, TKey key, TVal val)
        {
            if (!src.ContainsKey(key))
            {
                src.Add(key, val);
            }
        }

        public static void AddOrUpdate<TKey, TVal>(this Dictionary<TKey, TVal> src, TKey key, TVal val)
        {
            if (!src.ContainsKey(key))
            {
                src.Add(key, val);
            }
            else
            {
                src[key] = val;
            }
        }

        public static string CommaSeparated<T>(this IEnumerable<T> values)
        {
            return values.Select(v => v.ToString()).Aggregate((x, y) => x + "," + y);
        }

    
        private static readonly KeyValuePair<long, string>[] Thresholds =
         {
        // new KeyValuePair<long, string>(0, " Bytes"), // Don't devide by Zero!
        new KeyValuePair<long, string>(1, " Byte"),
        new KeyValuePair<long, string>(2, " Bytes"),
        new KeyValuePair<long, string>(1024, " KB"),
        new KeyValuePair<long, string>(1048576, " MB"), // Note: 1024 ^ 2 = 1026 (xor operator)
        new KeyValuePair<long, string>(1073741824, " GB"),
        new KeyValuePair<long, string>(1099511627776, " TB"),
        new KeyValuePair<long, string>(1125899906842620, " PB"),
        new KeyValuePair<long, string>(1152921504606850000, " EB"),

        // These don't fit into a int64
        // new KeyValuePair<long, string>(1180591620717410000000, " ZB"), 
        // new KeyValuePair<long, string>(1208925819614630000000000, " YB") 
    };

        /// <summary>
        /// Returns x Bytes, kB, Mb, etc... 
        /// </summary>
        public static string ToByteSize(this long value)
        {
            if (value == 0) return "0 Bytes"; // zero is plural
            for (int t = Thresholds.Length - 1; t > 0; t--)
                if (value >= Thresholds[t].Key) return ((double)value / Thresholds[t].Key).ToString("0.00") + Thresholds[t].Value;
            return "-" + ToByteSize(-value); // negative bytes (common case optimised to the end of this routine)
        }
    }
}
