using Tasks.Common;

namespace Task20.SearchFiles;

[Task(20, "Индексированный поиск файлов")]
public class SearchFilesSolution : ISearchFilesSolution
{
    // Размер хеш-таблицы и верхняя граница хеш-кода (диапазон 0..B-1)
    public const int B = 256;

    // Хеш-таблица со списками (открытое хеширование): каждая ячейка — список файлов с этим хешем
    private List<FileEntry>[] _table = new List<FileEntry>[0];

    // Точка входа: запрашивает папку, индексирует её, затем в цикле обрабатывает меню «поиск/выход»
    public void Run()
    {
        string rootPath = ReadFolderPath();

        Console.WriteLine("...................................");
        _table = BuildTable(rootPath);

        while (true)
        {
            Console.WriteLine("1: - Поиск файла по имени  2: - Выход");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Неверный ввод, попробуйте снова");
                continue;
            }

            if (choice == 2) break;

            if (choice != 1)
            {
                Console.WriteLine("Неверный ввод, попробуйте снова");
                continue;
            }

            Console.WriteLine("Введите имя файла для поиска (регистр не учитывается)");
            string? name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Имя файла не может быть пустым");
                continue;
            }

            try
            {
                List<FileEntry> result = Find(name, _table);
                foreach (FileEntry entry in result)
                {
                    Console.WriteLine(entry.FullPath);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine();
        }
    }

    // Запрашивает имя папки у пользователя, пока тот не введёт существующую
    private static string ReadFolderPath()
    {
        while (true)
        {
            Console.WriteLine("Введите имя папки");
            string? rootPath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(rootPath))
            {
                Console.WriteLine("Имя папки не может быть пустым");
                continue;
            }

            if (!Directory.Exists(rootPath))
            {
                Console.WriteLine($"Папка \"{rootPath}\" не найдена");
                continue;
            }

            return rootPath;
        }
    }

    // Хеш-функция: сумма кодов символов, домноженных на степень двойки от позиции, mod 256
    public byte CalculateHash(string name)
    {
        if (name == null)
            throw new ArgumentNullException(nameof(name));

        string lower = name.ToLower();
        int sum = 0;
        for (int i = 0; i < lower.Length; i++)
        {
            int shift = i % 8;
            sum += lower[i] << shift;
        }
        return (byte)((uint)sum % B);
    }

    // Создаёт хеш-таблицу из 256 пустых списков и запускает рекурсивный обход дерева
    public List<FileEntry>[] BuildTable(string rootPath)
    {
        List<FileEntry>[] table = new List<FileEntry>[B];
        for (int i = 0; i < B; i++)
            table[i] = new List<FileEntry>();

        RecursiveBuildTable(rootPath, table);
        return table;
    }

    // Поиск по хеш-коду имени с проверкой коллизий по полному имени (без учёта регистра)
    public List<FileEntry> Find(string name, List<FileEntry>[] table)
    {
        if (name == null)
            throw new ArgumentNullException(nameof(name));
        if (table == null || table.Length != B)
            throw new ArgumentException($"Таблица должна быть массивом длины {B}", nameof(table));

        byte hash = CalculateHash(name);
        List<FileEntry> result = new List<FileEntry>();

        foreach (FileEntry entry in table[hash])
        {
            if (string.Equals(entry.Name, name, StringComparison.OrdinalIgnoreCase))
                result.Add(entry);
        }

        if (result.Count == 0)
            throw new FileNotFoundException($"Файл с именем {name} не найден");

        return result;
    }

    // Рекурсивно обходит подпапки, добавляя каждый файл в ячейку с его хеш-кодом;
    // ошибки доступа к ФС игнорируются, чтобы одна «битая» папка не сломала весь обход
    private void RecursiveBuildTable(string path, List<FileEntry>[] table)
    {
        try
        {
            foreach (string file in Directory.GetFiles(path))
            {
                string name = Path.GetFileName(file);
                byte hash = CalculateHash(name);
                table[hash].Add(new FileEntry(file, name));
            }

            foreach (string directory in Directory.GetDirectories(path))
            {
                RecursiveBuildTable(directory, table);
            }
        }
        catch (UnauthorizedAccessException) { }
        catch (IOException) { }
    }
}
