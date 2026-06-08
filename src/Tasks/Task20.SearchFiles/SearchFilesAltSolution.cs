using Tasks.Common;

namespace Task20.SearchFiles;

using File = FileEntry;

[Task(20, "Индексированный поиск файлов", "alt")]
public class SearchFilesAltSolution : ISearchFilesSolution
{

    // private readonly struct File(string path, string name)
    // {
    //     public readonly string Path = path;
    //     public readonly string Name = name;
    // }

    private const byte MaxAttempts = 3;
    private const byte HashCodeRange = byte.MaxValue;
    private const byte PrimeForHash = 31;
    private List<File>[] _hashTable = [];

    public void Run()
    {
        try
        {
            _hashTable = BuildHashTable(ReadValidRootPath(), HashCodeRange + 1);
            Menu();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private List<File>[] BuildHashTable(string rootPath, int hashTableLength)
    {
        List<File>[] hashTable = new List<File>[hashTableLength];
        for (int i = 0; i < hashTableLength; i++)
        {
            hashTable[i] = new List<File>();
        }
        IndexFolder(rootPath, hashTable);
        return hashTable;
    }

    /// <summary>
    /// Спрашивает пользователя о дальнейшем действии
    /// </summary>
    /// <exception cref="Exception">Исчерпан лимит попыток</exception>
    private void Menu()
    {
        byte attempts = MaxAttempts;

        while (attempts > 0)
        {
            string input = ReadNonEmptyInput($"1: - Поиск файла по имени\n2: - Выход\n");

            try
            {
                byte option = byte.Parse(input);

                switch (option)
                {
                    case 1:
                        Search(_hashTable);
                        attempts = MaxAttempts;
                        continue;
                    case 2:
                        return;
                    default:
                        attempts--;
                        Console.WriteLine($"Такого пункта меню нет, попробуйте еще раз\nОсталось попыток: {attempts}");
                        break;
                }
            }
            catch (Exception e) when (e is FormatException or OverflowException)
            {
                attempts--;
                Console.WriteLine($"Недопустимый ввод, введите 1 или 2\nОсталось попыток: {attempts}");
            }
        }

        throw new Exception("Исчерпано количество попыток ввода пунктов меню");
    }

    /// <summary>
    /// Читает ввод пути до папки
    /// </summary>
    /// <returns>Существующий и доступный путь до папки</returns>
    /// <exception cref="Exception">Исчерпан лимит попыток</exception>
    private static string ReadValidRootPath()
    {
        byte attempts = MaxAttempts;

        while (attempts > 0)
        {
            string rootPath = ReadNonEmptyInput("Введите путь до папки: ");

            try
            {
                Directory.GetFiles(rootPath);
                return rootPath;
            }
            catch (UnauthorizedAccessException)
            {
                attempts--;
                Console.WriteLine($"К данной папке нет доступа: {rootPath}\nОсталось попыток: {attempts}");
            }
            catch (DirectoryNotFoundException)
            {
                attempts--;
                Console.WriteLine($"Папка по пути '{rootPath}' не найдена\nОсталось попыток: {attempts}");
            }
            catch (PathTooLongException)
            {
                attempts--;
                Console.WriteLine($"Путь до папки '{rootPath}' слишком длинный\nОсталось попыток: {attempts}");
            }
            catch (IOException)
            {
                attempts--;
                Console.WriteLine($"Недопустимые символы или неверный формат ввода\nОсталось попыток: {attempts}");
            }
        }

        throw new Exception("Исчерпано количество попыток ввода пути к папке");
    }

    /// <summary>
    /// Читает не пустой ввод в консоль
    /// </summary>
    /// <param name="label">Строка приветствия</param>
    /// <returns>Не пустую строку</returns>
    private static string ReadNonEmptyInput(string label)
    {
        Console.Write(label);

        while (true)
        {
            string? input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
                return input;

            Console.WriteLine("Строка не должна быть пустой");
        }
    }

    /// <summary>
    /// Индексирует папку
    /// </summary>
    /// <param name="dirPath">Путь до папки</param>
    /// <param name="hashTable"></param>
    private void IndexFolder(string dirPath, List<File>[] hashTable)
    {
        try
        {
            foreach (string file in Directory.GetFiles(dirPath))
                IndexFile(file, hashTable);

            foreach (string dir in Directory.GetDirectories(dirPath))
                IndexFolder(dir, hashTable);
        }
        catch (UnauthorizedAccessException)
        {
            // нет прав доступа - пропуск
        }
        catch (DirectoryNotFoundException)
        {
            // директория была удалена - пропуск
        }
        catch (PathTooLongException)
        {
            // путь слишком длинный - пропуск
        }
        catch (IOException)
        {
            // неправильный синтаксис имени папки – пропускаем
        }
    }

    /// <summary>
    /// Записывает файл в hash таблицу
    /// </summary>
    /// <param name="filePath">Имя файла</param>
    /// <param name="hashTable"></param>
    private void IndexFile(string filePath, List<File>[] hashTable)
    {
        string normalizedStr = NormalizeStr(Path.GetFileName(filePath));
        byte hashCode = GetHashCode(normalizedStr, hashTable.Length);
        File file = new File(filePath, normalizedStr);

        hashTable[hashCode].Add(file);
    }

    /// <summary>
    /// Создает hash код на основе target строки
    /// </summary>
    /// <returns>Hash код в диапазоне byte типа</returns>
    private byte GetHashCode(string target, int hashTableLenght)
    {
        uint hash = 0;

        // используется полиномиальное хеширование
        foreach (char c in target)
            hash = hash * PrimeForHash + c;

        return (byte)(hash % hashTableLenght);
    }

    /// <summary>
    /// Приводит строки к одному виду
    /// </summary>
    /// <param name="str">Исходная строка</param>
    /// <returns>Нормализованную строку</returns>
    private string NormalizeStr(string str)
    {
        return str.ToLower();
    }

    /// <summary>
    /// Организует процесс поиска в hash таблице
    /// </summary>
    private void Search(List<File>[] hashTable)
    {
        string fileName = ReadNonEmptyInput("Введите имя файла для поиска (регистр не учитывается): ");
        List<File> files = FindFilesByName(fileName, hashTable);

        if (files.Count == 0)
        {
            Console.WriteLine("Файл не найден");
            return;
        }

        foreach (File file in files)
            Console.WriteLine(file.FullPath);
    }

    private List<File> FindFilesByName(string name, List<File>[] hashTable)
    {
        if (name == null)
            throw new ArgumentNullException(nameof(name));
        if (hashTable == null || hashTable.Length != (HashCodeRange + 1))
            throw new ArgumentException($"Таблица должна быть массивом длины {HashCodeRange + 1}", nameof(hashTable));

        string normalizedStr = NormalizeStr(name);
        byte hashCode = GetHashCode(normalizedStr, hashTable.Length);
        List<File>? files = hashTable[hashCode];

        if (files == null)
            return [];

        List<File> result = [];
        foreach (File file in files)
            if (file.Name == normalizedStr)
                result.Add(file);

        return result;
    }

    public byte CalculateHash(string name)
    {
        if (name == null)
            throw new ArgumentNullException(nameof(name));

        return GetHashCode(NormalizeStr(name), HashCodeRange + 1);
    }

    public List<FileEntry>[] BuildTable(string rootPath)
    {
        return BuildHashTable(rootPath, HashCodeRange + 1);
    }

    public List<FileEntry> Find(string name, List<FileEntry>[] table)
    {
        var r = FindFilesByName(name, table);

        if (r.Count == 0)
            throw new FileNotFoundException($"Файл с именем {name} не найден");

        return r;
    }
}
