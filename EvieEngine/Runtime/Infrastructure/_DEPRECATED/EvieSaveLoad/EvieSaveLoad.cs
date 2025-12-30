using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace EvieEngine
{
    [System.Serializable]
    public class GameData
    {
        public Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
    }

    public interface EvieOnLoad
    {
        void OnLoad();
    }

    public class EvieSaveLoad : MonoBehaviour
    {
        public static EvieSaveLoad Instance { get; private set; }

        private readonly FileSystem fileSystem;
        private readonly string saveDirectory;
        private readonly string defaultSaveFileName = "savegame.json";
        private string lastSaveFileName;

        private GameData currentData;

        private EvieSaveLoad()
        {
            fileSystem = FileSystem.Instance;
            saveDirectory = Path.Combine(fileSystem.systemDocumentDir, Application.productName, "saves");

            // ������ ���������� ��� ����������, ���� � ���
            if (!fileSystem.isDirExist(saveDirectory)) fileSystem.CreateDir(saveDirectory);

            // ���� ��������� ����������
            lastSaveFileName = GetLastSaveFileName();

            // �������������� ������� ������
            currentData = new GameData();
        }

        public static void Initialize()
        {
            if (Instance == null)
            {
                Instance = new EvieSaveLoad();
            }
        }

        // ���������� ������ � ������� �������
        public void Save(string fileName = null)
        {
            fileName ??= defaultSaveFileName;
            string fullPath = Path.Combine(saveDirectory, fileName);

            // ���������� DictionaryJSON ��� ������������
            string json = DictionaryJSON.Serialize(currentData.keyValuePairs);
            File.WriteAllText(fullPath, json);

            lastSaveFileName = fileName;
            Debug.Log($"[EVIECORE/SYBLIBS/EVIESAVELOAD/LOG] Game saved to: {fullPath}");
        }

        // �������� ������
        public void Load(string fileName = null)
        {
            fileName ??= lastSaveFileName ?? defaultSaveFileName;
            string fullPath = Path.Combine(saveDirectory, fileName);

            if (fileSystem.isFileExist(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                // ���������� DictionaryJSON ��� ��������������
                currentData.keyValuePairs = DictionaryJSON.Deserialize(json);
                ExecuteOnLoad();
                Debug.Log($"[EVIECORE/SYBLIBS/EVIESAVELOAD/LOG] Game loaded from: {fullPath}");
            }
            else
            {
                Debug.LogWarning($"[EVIECORE/SYBLIBS/EVIESAVELOAD/WARNING] Save file {fileName} not found! Initializing new data.");
                currentData = new GameData();
            }
        }

        // ��������� OnLoad �� ���� ����������� EvieOnLoad
        private void ExecuteOnLoad()
        {
            // ����� ��� ������� �� �����
            EvieOnLoad[] evieObjects = FindObjectsOfType<MonoBehaviour>(true) // true �������� ���������� �������
                .OfType<EvieOnLoad>()
                .ToArray();

            // ������� OnLoad � ���� ��������� ��������
            foreach (EvieOnLoad evie in evieObjects)
            {
                evie.OnLoad();
            }
        }

        // �������� ����������
        public void DeleteSave(string fileName)
        {
            string fullPath = Path.Combine(saveDirectory, fileName);
            fileSystem.DeleteFile(fullPath);

            // ��������� ��������� ����������
            lastSaveFileName = GetLastSaveFileName();
            Debug.Log($"[EVIECORE/SYBLIBS/EVIESAVELOAD/LOG] Save file {fileName} deleted.");
        }

        // ��������� ���� ����������
        public List<string> GetAllSaveFiles()
        {
            return Directory.GetFiles(saveDirectory, "*.json")
                .Select(Path.GetFileName)
                .ToList();
        }

        // ��������� ����� ���������� ����������
        private string GetLastSaveFileName()
        {
            var files = GetAllSaveFiles();
            return files.OrderByDescending(f => File.GetLastWriteTime(Path.Combine(saveDirectory, f))).FirstOrDefault();
        }

        // ������ � �������
        public void SetKeyValue<T>(string key, T value)
        {
            if (currentData.keyValuePairs.ContainsKey(key))
            {
                currentData.keyValuePairs[key] = value;
            }
            else
            {
                currentData.keyValuePairs.Add(key, value);
            }
        }

        public T GetKeyValue<T>(string key, T defaultValue = default)
        {
            if (currentData.keyValuePairs.ContainsKey(key))
            {
                try
                {
                    return (T)Convert.ChangeType(currentData.keyValuePairs[key], typeof(T));
                }
                catch (InvalidCastException)
                {
                    Debug.LogWarning($"[EVIECORE/SYBLIBS/EVIESAVELOAD/WARNING] Key '{key}' cannot be cast to {typeof(T)}. Returning default value.");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[EVIECORE/SYBLIBS/EVIESAVELOAD/ERROR] Error retrieving key '{key}': {ex.Message}");
                }
            }
            return defaultValue;
        }
    }
}