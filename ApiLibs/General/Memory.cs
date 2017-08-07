﻿using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ApiLibs.General
{
    public class Memory
    {
        public readonly string DirectoryPath;

        public static readonly string ApplicationPath = Environment.GetEnvironmentVariable(
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "LocalAppData" : "Home");

        public Memory(string baseUrl)
        {
            DirectoryPath = baseUrl;
        }

        public T ReadFile<T>(string filename) where T : new()
        {
            string text = ReadFile(filename);

            if (text != "")
            {
                T res = JsonConvert.DeserializeObject<T>(text);
                return res;
            }
            else
            {
                T res = new T();
                WriteFile(filename, res);
                return res;
            }
        }

        public string ReadFile(string filename)
        {
            string filePath = DirectoryPath + filename;

            string FileDirectoryPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(FileDirectoryPath))
            {
                Directory.CreateDirectory(FileDirectoryPath);
            }

            FileStream stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Read);
            string text;
            using (StreamReader reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
                reader.Dispose();
            }
            stream.Dispose();
            return text;
        }

        public void WriteFile(string v, object obj)
        {
            var s = obj as string;
            File.WriteAllText(DirectoryPath + v, s ?? JsonConvert.SerializeObject(obj, Formatting.Indented));
        }
    }
}
