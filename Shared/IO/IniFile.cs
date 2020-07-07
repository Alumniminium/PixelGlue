using System.Collections.Generic;
using System.IO;

namespace Shared.IO
{
    public class IniFile
    {
        private readonly Dictionary<string, Dictionary<string, string>> _iniFile = new Dictionary<string, Dictionary<string, string>>();
        private readonly string _path = "";

        public IniFile(string path)
        {
            if (File.Exists(path))
                _path = path;
            else throw new FileNotFoundException();
        }

        public void Load()
        {
            using var reader = new StreamReader(File.OpenRead(_path));
            string curHeader = "";
            while (!reader.EndOfStream)
            {
                var curLine = reader.ReadLine();

                if (string.IsNullOrEmpty(curLine))
                    continue;

                if (curLine.StartsWith("["))
                {
                    curHeader = curLine;
                    _iniFile.Add(curHeader, new Dictionary<string, string>());
                }
                else
                {
                    var kvp = curLine.Split("=", 2);
                    if (string.IsNullOrEmpty(kvp[0]) || string.IsNullOrEmpty(kvp[1]))
                        continue;
                    _iniFile[curHeader].TryAdd(kvp[0], kvp[1]);
                }
            }
        }
        public Dictionary<string, Dictionary<string, string>> GetDictionary() => _iniFile;
        public string GetValue(string header, string key, object default_value)
        {
            if (_iniFile.ContainsKey(header))
            {
                if (_iniFile[header].ContainsKey(key))
                    return _iniFile[header][key];
                else
                    return (string)default_value;
            }
            return (string)default_value;
        }

        public void SetValue(string value, string header, string key)
        {
            if (!_iniFile.ContainsKey(header))
                _iniFile.Add(header, new Dictionary<string, string>());

            _iniFile[header][key] = value;
        }

        public void Save()
        {
            using var writer = new StreamWriter(File.OpenWrite(_path));
            foreach (var kvp in _iniFile)
            {
                writer.WriteLine(kvp.Key);
                foreach (var kvp2 in kvp.Value)
                {
                    writer.WriteLine(kvp2.Key + "=" + kvp2.Value);
                }
                writer.WriteLine();
            }
        }
    }
}