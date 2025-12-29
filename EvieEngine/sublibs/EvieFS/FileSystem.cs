using System;
using System.IO;
using UnityEngine;

namespace EvieEngine
{
    public class FileSystem : MonoBehaviour
    {
        public static FileSystem Instance { get; private set; }
        
        public string systemDocumentDir;
        public string systemUserDir;
        public string systemGamePathDir;
        public string systemGamePathFullpath;
        
        public ConfigManager configManager;
        
        private FileSystem()
        {
            systemDocumentDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            systemUserDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            systemGamePathDir = Application.persistentDataPath;
            systemGamePathFullpath = Path.Combine(systemGamePathDir, Application.productName);

            configManager = new ConfigManager(this);
        }
        
        public static void Initialize()
        {
            if (Instance == null)
            {
                Instance = new FileSystem();
            }
        }
        
        public void CreateFile(string dirPath, string fileName)
        {
            string fullPath = Path.Combine(dirPath, fileName);
            if (!File.Exists(fullPath))
            {
                File.Create(fullPath).Dispose();
            }
            else
            {
                Debug.LogWarning($"Файл по пути: {fullPath} уже существует");
            }
        }
        
        public void CreateFile(string dirPath, string fileName, string fileContent)
        {
            string fullPath = Path.Combine(dirPath, fileName);
            if (!File.Exists(fullPath))
            {
                File.WriteAllText(fullPath, fileContent);
            }
            else
            {
                Debug.LogWarning($"Файл по пути: {fullPath} уже существует");
            }
        }
        
        public void CreateDir(string dirPath, string folderPath)
        {
            string fullDirPath = Path.Combine(dirPath, folderPath);
            if (!Directory.Exists(fullDirPath))
            {
                Directory.CreateDirectory(fullDirPath);
            }
            else
            {
                Debug.LogWarning($"Директория по пути: {fullDirPath} уже существует");
            }
        }

        public void CreateDir(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            else
            {
                Debug.LogWarning($"Директория по пути: {dirPath} уже существует");
            }
        }
        
        public void EditFile(string filePath, string newFileContent)
        {
            if (File.Exists(filePath))
            {
                File.WriteAllText(filePath, newFileContent);
            }
            else
            {
                Debug.LogWarning($"Файл по пути: {filePath} не найден");
            }
        }
        
        public void RenameFile(string filePath, string newFileName)
        {
            if (File.Exists(filePath))
            {
                string newFilePath = Path.Combine(Path.GetDirectoryName(filePath), newFileName);
                File.Move(filePath, newFilePath);
            }
            else
            {
                Debug.LogWarning($"Файл по пути: {filePath} не найден");
            }
        }

        public void RenameFile(string dirPath, string fileName, string newFileName)
        {
            string fullPath = Path.Combine(dirPath, fileName);
            if (File.Exists(fullPath))
            {
                string newFilePath = Path.Combine(dirPath, newFileName);
                File.Move(fullPath, newFilePath);
            }
            else
            {
                Debug.LogWarning($"Файл по пути: {fullPath} не найден");
            }
        }
        
        public void MoveFile(string filePath, string newFilePath)
        {
            if (File.Exists(filePath))
            {
                File.Move(filePath, newFilePath);
            }
            else
            {
                Debug.LogWarning($"Файл по пути: {filePath} не найден");
            }
        }

        public void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            else
            {
                Debug.LogWarning($"Файл по пути: {filePath} не найден");
            }
        }
        
        public void DeleteDir(string dirPath)
        {
            if (Directory.Exists(dirPath))
            {
                Directory.Delete(dirPath, recursive: true);
            }
            else
            {
                Debug.LogWarning($"Директория по пути: {dirPath} не найдена");
            }
        }
        
        public bool isFileExist(string filePath)
        {
            return File.Exists(filePath);
        }

        public bool isDirExist(string dirPath)
        {
            return Directory.Exists(dirPath);
        }
    }
}
