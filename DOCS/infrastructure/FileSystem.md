[Назад](../infrastructure.md)

# FileSystem

`FileSystem` — это оболочка над стандартным `System.IO`, которая упрощает работу с файлами (как мне кажется)

FileSystem это синглтон, т.е вы можете обращаться к нему из любого скрипта просто написав: 
```cs
FileSystem.Instance
```

## Инициализация

Перед использованием необходимо инициализировать систему:

```cs
FileSystem.Initialize();
```

После этого доступ осуществляется так:
```cs
FileSystem.Instance
```

## Системные пути

```cs
FileSystem.Instance.systemDocumentDir
```

`Environment.SpecialFolder.MyDocuments`  Путь к папке **Документы** пользователя.

```cs
FileSystem.Instance.UserProfile
```

`Environment.SpecialFolder.UserProfile`  Домашняя директория пользователя.

```cs
FileSystem.Instance.systemGamePathDir
```

`Application.persistentDataPath` Папка постоянных данных Unity.

```cs
FileSystem.Instance.systemGamePathFullpath
```

`persistentDataPath + Application.productName` Корневая папка данных конкретной игры.

## Работа с файлами

```cs
FileSystem.Instance.CreateFile(string dirPath, string fileName);
```
Создает пустой файл, если он не существует

```cs
FileSystem.Instance.CreateFile(string dirPath, string fileName, string fileContent);
```
Создает файл с указанным содержимым

```cs
FileSystem.Instance.EditFile(string filePath, string newFileContent);
```
Полностью перезаписывает содержимое файла

```cs
FileSystem.Instance.RenameFile(string filePath, string newFileName);
```

```cs
FileSystem.Instance.RenameFile(string dirPath, string fileName, string newFileName);
```
Переименовывает файл

```cs
FileSystem.Instance.MoveFile(string filePath, string newFilePath);
```
Перемещает файл по новому пути

```cs
FileSystem.Instance.DeleteFile(string filePath);
```
Удаляет файл

```cs
FileSystem.Instance.isFileExist(string filePath); // RETURN BOOL
```
Проверяет существование файла.

## Работа с директориями

```cs
FileSystem.Instance.CreateDir(string dirPath);
```

```cs
FileSystem.Instance.CreateDir(string dirPath, string folderPath);
```
Создает директорию, если она не существует

```cs
FileSystem.Instance.DeleteDir(string dirPath);
```
Удаляет директорию **рекурсивно**

```cs
FileSystem.Instance.isDirExist(string dirPath); // RETURN BOOL
```
Проверяет существование директории

## ConfigManager

`ConfigManager` это простой менеджер конфигурационных файлов для хранения и загрузки настроек проекта в формате `.EvieConfig`

Пример получаемой конфигцрации:

```ini
[EVIE ENGINE CONFIG]

volume = 0.8
playerName = "Evie"
resolutions = [1280,1920,2560]
```

По сути это аналог .ini файлов, где каждое значение хранится в формате `key = value`

`ConfigManager` создаётся автоматически внутри `FileSystem`:

```cs
FileSystem.Instance.configManager
```

По умолчанию менеджер конфигурации использует следующую директорию: `Документы /<ProductName>/configs`

```cs
FileSystem.Instance.configManager.SetWorkDir(string newWorkDir);
```
Устанавливает новую рабочую папку для конфигов  
Если директория не существует, она будет создана автоматически

```cs
FileSystem.Instance.configManager.CreateConfig(string configName);
```

Создаёт новый конфигурационный файл: `<configName>.EvieConfig`
Если файл уже существует, то функция ничего не сделает.


```cs
FileSystem.Instance.configManager.LoadConfig(string configName);
```
Загружает конфиг из файла.

```cs
FileSystem.Instance.configManager.SaveConfig();
```
Сохраняет текущие значения в открытый конфиг
[!!!] Работает только если ранее был создан или загружен конфиг

```cs
FileSystem.Instance.configManager.ClearConfig();
```
Очищает текущую конфигурацию из памяти и сбрасывает активный файл

### Запись новых значений

```cs
FileSystem.Instance.configManager.AddConfigValue<T>(string key, T value);
```
Добавляет или перезаписывает значение в конфиге

Поддерживаемые типы:
- `string`
- `int`
- `float`
- `double`
- `List<string>`
- `List<int>`
- массивы и перечисления (`IEnumerable`)

```cs
FileSystem.Instance.configManager.RemoveConfigValue(string key);
```
Удаляет значение из конфига

```cs
T GetConfigValue<T>(string key);
```
Возвращает значение по ключу с приведением типа.

Если:
- ключ не найден
- тип не поддерживается
- произошла ошибка парсинга

То:
- возвращается `default(T)`.

### Формат значений

- `string` -> `"text"`
- числа -> `10`, `0.5`
- списки -> `[a,b,c]`