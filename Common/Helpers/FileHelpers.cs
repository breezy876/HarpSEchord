﻿
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class FileHelpers
    {

        public static void WriteText(string text, string fileName)
        {
            File.AppendAllText(fileName, text);
        }

        public static void Save(object obj, string fileName)
        {
            var dirName = Path.GetDirectoryName(fileName);

            if (!Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);

            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            WriteAllText(fileName, json);
        }

        public static T Load<T>(string filePath)
        {
            if (!File.Exists(filePath))
                return default(T);

            string json = "";
            try
            {
                json = ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(json);

            }
            catch (Exception ex)
            {
                return default(T);
            }


        }

        private static void WriteAllText(string path, string text)
        {
            //File.WriteAllText(path, text);
            //text += "\0";

            FileStream fs;
            if (File.Exists(path))

            fs = File.Open(path, FileMode.Truncate,
                           FileAccess.Write, FileShare.ReadWrite);
            else
                fs = File.Open(path, FileMode.CreateNew,
               FileAccess.Write, FileShare.ReadWrite);
            using (var sw = new StreamWriter(fs, Encoding.UTF8))
            {
                sw.Write(text);
                sw.Flush();
            }
            fs.Close();
        }


            private static string ReadAllText(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, Encoding.UTF8))
            return  sr.ReadToEnd();
        }


        public static bool IsDirectory(string path)
        {
            var attrs = File.GetAttributes(path);
            return (attrs & FileAttributes.Directory) == FileAttributes.Directory;
        }

    }
}
