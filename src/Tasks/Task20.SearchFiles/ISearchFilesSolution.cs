using Tasks.Common;

namespace Task20.SearchFiles;

// Элемент хеш-таблицы: имя файла и его полный путь
public struct FileEntry
{
    public FileEntry(string fullPath, string name)
    {
        FullPath = fullPath;
        Name = name;
    }

    public string FullPath { get; }
    public string Name { get; }
}

public interface ISearchFilesSolution : ISolution
{
    // Хеш-код имени файла в диапазоне 0..255 (без учёта регистра)
    byte CalculateHash(string name);

    // Индексирует все файлы в папке и подпапках в массив списков [0..255]
    List<FileEntry>[] BuildTable(string rootPath);

    // Возвращает все файлы с указанным именем; кидает FileNotFoundException, если не найдено
    List<FileEntry> Find(string name, List<FileEntry>[] table);
}
