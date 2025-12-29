using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace EvieEngine
{
    public class ConfigManager
    {
        private string workDir;
        private string configPath;
        private string openConfig;
        private Dictionary<string, string> configValues;
        private FileSystem fileSystem;

        public ConfigManager(FileSystem fs)
        {
            fileSystem = fs;
            workDir = Path.Combine(fileSystem.systemDocumentDir, Application.productName, "configs");
            if (!fileSystem.isDirExist(workDir))
            {
                Directory.CreateDirectory(workDir);
            }
            configValues = new Dictionary<string, string>();
        }

        public void SetWorkDir(string newWorkDir)
        {
            workDir = newWorkDir;
            if (!fileSystem.isDirExist(workDir))
            {
                Directory.CreateDirectory(workDir);
            }
        }

        public void LoadConfig(string configName)
        {
            configPath = Path.Combine(workDir, configName + ".EvieConfig");
            if (fileSystem.isFileExist(configPath))
            {
                openConfig = File.ReadAllText(configPath);
                ParseConfig(openConfig);
            }
        }

        public void SaveConfig()
        {
            if (!string.IsNullOrEmpty(configPath))
            {
                string configText = GenerateConfigText();
                File.WriteAllText(configPath, configText);
            }
        }

        private string GenerateConfigText()
        {
            string configText = $"[EVIE ENGINE CONFIG]\n\n";
            foreach (var entry in configValues)
            {
                configText += entry.Key + " = " + entry.Value + "\n";
            }
            return configText;
        }

        public void CreateConfig(string configName)
        {
            configPath = Path.Combine(workDir, configName + ".EvieConfig");
            if (!fileSystem.isFileExist(configPath))
            {
                configValues.Clear();
                SaveConfig();
            }
        }

        public void AddConfigValue<T>(string key, T value)
        {
            string formattedValue = FormatValue(value);
            if (!string.IsNullOrEmpty(formattedValue))
            {
                configValues[key] = formattedValue;
            }
        }

        public void RemoveConfigValue(string key)
        {
            if (configValues.ContainsKey(key))
            {
                configValues.Remove(key);
            }
        }

        private string FormatValue<T>(T value)
        {
            if (value is string)
            {
                return $"\"{value}\"";
            }
            else if (value is int || value is float || value is double)
            {
                return value.ToString();
            }
            else if (value is IEnumerable<string> enumerable) // ��� List<string>, Array<string>, � ������
            {
                return "[" + string.Join(",", enumerable) + "]";
            }
            else if (value is IEnumerable<object> objectEnumerable) // ��� List<T>, ��� T �� ����������� string
            {
                return "[" + string.Join(",", objectEnumerable.Select(obj => obj.ToString())) + "]";
            }
            else
            {
                return null;
            }
        }


        private void ParseConfig(string configText)
        {
            configValues.Clear();
            var lines = configText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                if (line.StartsWith("[") || line.StartsWith("#"))
                {
                    continue;
                }

                var parts = line.Split(new[] { '=' }, 2);
                if (parts.Length == 2)
                {
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();
                    configValues[key] = value;
                }
            }
        }

        public T GetConfigValue<T>(string key)
        {
            if (configValues.TryGetValue(key, out string value))
            {
                try
                {
                    if (typeof(T) == typeof(string))
                    {
                        return (T)(object)value.Trim('\"'); // ������� ������� ��� �����
                    }
                    else if (typeof(T) == typeof(int))
                    {
                        return (T)(object)int.Parse(value);
                    }
                    else if (typeof(T) == typeof(float))
                    {
                        return (T)(object)float.Parse(value);
                    }
                    else if (typeof(T) == typeof(double))
                    {
                        return (T)(object)double.Parse(value);
                    }
                    else if (typeof(T) == typeof(List<string>))
                    {
                        var list = value.Trim('[', ']').Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        return (T)(object)new List<string>(list);
                    }
                    else if (typeof(T) == typeof(List<int>))
                    {
                        var list = value.Trim('[', ']').Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                       .Select(int.Parse).ToList();
                        return (T)(object)list;
                    }
                    else
                    {
                        return default;
                    }
                }
                catch (Exception ex)
                {
                    return default;
                }
            }
            else
            {
                return default;
            }
        }
        
        public void ClearConfig()
        {
            configValues.Clear();
            configPath = null;
            openConfig = null;
        }

    }
}