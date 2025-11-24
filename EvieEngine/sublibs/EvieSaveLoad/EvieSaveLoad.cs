using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using UnityEngine;

namespace Eviecore
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

        private readonly EvieFS fileSystem;
        private readonly string saveDirectory;
        private readonly string defaultSaveFileName = "savegame.json";
        private string lastSaveFileName;

        private GameData currentData;

        private EvieSaveLoad()
        {
            fileSystem = EvieFS.Instance;
            saveDirectory = Path.Combine(fileSystem.systemDocumentDir, Application.productName, "saves");

            // Создаём директорию для сохранений, если её нет
            if (!fileSystem.isDirExist(saveDirectory)) fileSystem.CreateDir(saveDirectory);

            // Ищем последнее сохранение
            lastSaveFileName = GetLastSaveFileName();

            // Инициализируем текущие данные
            currentData = new GameData();
        }

        public static void Initialize()
        {
            if (Instance == null)
            {
                Instance = new EvieSaveLoad();
            }
        }

        // Сохранение данных в текущем объекте
        public void Save(string fileName = null)
        {
            fileName ??= defaultSaveFileName;
            string fullPath = Path.Combine(saveDirectory, fileName);

            // Используем DictionaryJSON для сериализации
            string json = DictionaryJSON.Serialize(currentData.keyValuePairs);
            File.WriteAllText(fullPath, json);

            lastSaveFileName = fileName;
            Debug.Log($"[EVIECORE/SYBLIBS/EVIESAVELOAD/LOG] Game saved to: {fullPath}");
        }

        // Загрузка данных
        public void Load(string fileName = null)
        {
            fileName ??= lastSaveFileName ?? defaultSaveFileName;
            string fullPath = Path.Combine(saveDirectory, fileName);

            if (fileSystem.isFileExist(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                // Используем DictionaryJSON для десериализации
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

        // Выполняет OnLoad на всех экземплярах EvieOnLoad
        private void ExecuteOnLoad()
        {
            // Найти все объекты на сцене
            EvieOnLoad[] evieObjects = FindObjectsOfType<MonoBehaviour>(true) // true включает неактивные объекты
                .OfType<EvieOnLoad>()
                .ToArray();

            // Вызвать OnLoad у всех найденных объектов
            foreach (EvieOnLoad evie in evieObjects)
            {
                evie.OnLoad();
            }
        }

        // Удаление сохранения
        public void DeleteSave(string fileName)
        {
            string fullPath = Path.Combine(saveDirectory, fileName);
            fileSystem.DeleteFile(fullPath);

            // Обновляем последнее сохранение
            lastSaveFileName = GetLastSaveFileName();
            Debug.Log($"[EVIECORE/SYBLIBS/EVIESAVELOAD/LOG] Save file {fileName} deleted.");
        }

        // Получение всех сохранений
        public List<string> GetAllSaveFiles()
        {
            return Directory.GetFiles(saveDirectory, "*.json")
                .Select(Path.GetFileName)
                .ToList();
        }

        // Получение имени последнего сохранения
        private string GetLastSaveFileName()
        {
            var files = GetAllSaveFiles();
            return files.OrderByDescending(f => File.GetLastWriteTime(Path.Combine(saveDirectory, f))).FirstOrDefault();
        }

        // Работа с ключами
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