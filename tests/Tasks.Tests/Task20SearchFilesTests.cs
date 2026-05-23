using System.Text.RegularExpressions;
using Task20.SearchFiles;

namespace Tasks.Tests;

[Collection(nameof(ConsoleCollection))]
public class Task20SearchFilesTests : IDisposable
{
    private readonly string _testRoot;

    public Task20SearchFilesTests()
    {
        _testRoot = Path.Combine(Path.GetTempPath(), "Task20Tests_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_testRoot);
    }

    public void Dispose()
    {
        try { Directory.Delete(_testRoot, recursive: true); } catch { }
    }

    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new SearchFilesSolution()];
    }

    // ─── CalculateHash: базовые свойства ───────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Hash_ReturnsByteInRange0To255(ISearchFilesSolution s)
    {
        string[] names = ["a", "main.m", "very_long_file_name_with_lots_of_characters.txt", "Я"];

        foreach (string name in names)
        {
            byte hash = s.CalculateHash(name);
            Assert.InRange(hash, (byte)0, (byte)255);
        }
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Hash_IsDeterministic(ISearchFilesSolution s)
    {
        byte first = s.CalculateHash("main.m");
        byte second = s.CalculateHash("main.m");

        Assert.Equal(first, second);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Hash_IsCaseInsensitive(ISearchFilesSolution s)
    {
        byte lower = s.CalculateHash("main.m");
        byte upper = s.CalculateHash("MAIN.M");
        byte mixed = s.CalculateHash("Main.M");

        Assert.Equal(lower, upper);
        Assert.Equal(lower, mixed);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Hash_DifferentByPermutation(ISearchFilesSolution s)
    {
        // Лекция, раздел 18: простая сумма даёт одинаковый хеш для перестановок.
        // Улучшенная функция (с домножением на степень) должна различать.
        byte abc = s.CalculateHash("abc");
        byte cba = s.CalculateHash("cba");

        Assert.NotEqual(abc, cba);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Hash_DifferentForDifferentStrings(ISearchFilesSolution s)
    {
        byte main = s.CalculateHash("main.m");
        byte readme = s.CalculateHash("readme.txt");

        Assert.NotEqual(main, readme);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Hash_NullThrows(ISearchFilesSolution s)
    {
        Assert.Throws<ArgumentNullException>(() => s.CalculateHash(null!));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Hash_EmptyString_IsZero(ISearchFilesSolution s)
    {
        byte hash = s.CalculateHash("");

        Assert.Equal(0, hash);
    }

    // ─── BuildTable: индексация ────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BuildTable_ReturnsArrayOfSize256(ISearchFilesSolution s)
    {
        List<FileEntry>[] table = s.BuildTable(_testRoot);

        Assert.Equal(256, table.Length);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BuildTable_AllBucketsAreInitialized(ISearchFilesSolution s)
    {
        List<FileEntry>[] table = s.BuildTable(_testRoot);

        for (int i = 0; i < table.Length; i++)
        {
            Assert.NotNull(table[i]);
        }
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BuildTable_EmptyDirectory_AllBucketsEmpty(ISearchFilesSolution s)
    {
        List<FileEntry>[] table = s.BuildTable(_testRoot);

        for (int i = 0; i < table.Length; i++)
        {
            Assert.Empty(table[i]);
        }
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BuildTable_FindsSingleFile(ISearchFilesSolution s)
    {
        CreateFile("main.m");

        List<FileEntry>[] table = s.BuildTable(_testRoot);
        int total = CountEntries(table);

        Assert.Equal(1, total);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BuildTable_RecursivelyWalksSubdirectories(ISearchFilesSolution s)
    {
        CreateFile("root.txt");
        CreateFile("Application/main.m");
        CreateFile("Application/helper.cs");
        CreateFile("Docs/readme.txt");
        CreateFile("Docs/Sub/nested.txt");

        List<FileEntry>[] table = s.BuildTable(_testRoot);
        int total = CountEntries(table);

        Assert.Equal(5, total);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BuildTable_FileGoesIntoBucketEqualToItsHash(ISearchFilesSolution s)
    {
        CreateFile("main.m");
        byte expectedHash = s.CalculateHash("main.m");

        List<FileEntry>[] table = s.BuildTable(_testRoot);

        Assert.Single(table[expectedHash]);
        Assert.Equal("main.m", table[expectedHash][0].Name);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BuildTable_StoresFullPath(ISearchFilesSolution s)
    {
        string filePath = CreateFile("Application/main.m");
        byte hash = s.CalculateHash("main.m");

        List<FileEntry>[] table = s.BuildTable(_testRoot);

        Assert.Equal(filePath, table[hash][0].FullPath);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BuildTable_NonExistentPath_DoesNotThrow(ISearchFilesSolution s)
    {
        string fakePath = Path.Combine(_testRoot, "definitely_not_here");

        var ex = Record.Exception(() => s.BuildTable(fakePath));

        Assert.Null(ex);
    }

    // ─── Find: поиск ───────────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Find_ExistingFile_ReturnsIt(ISearchFilesSolution s)
    {
        string path = CreateFile("Application/main.m");
        List<FileEntry>[] table = s.BuildTable(_testRoot);

        List<FileEntry> result = s.Find("main.m", table);

        Assert.Single(result);
        Assert.Equal(path, result[0].FullPath);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Find_IsCaseInsensitive(ISearchFilesSolution s)
    {
        string path = CreateFile("Application/main.m");
        List<FileEntry>[] table = s.BuildTable(_testRoot);

        List<FileEntry> result = s.Find("MAIN.M", table);

        Assert.Single(result);
        Assert.Equal(path, result[0].FullPath);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Find_NonExistingFile_ThrowsFileNotFoundException(ISearchFilesSolution s)
    {
        List<FileEntry>[] table = s.BuildTable(_testRoot);

        Assert.Throws<FileNotFoundException>(() => s.Find("missing.txt", table));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Find_TwoFilesWithSameNameInDifferentFolders_ReturnsBoth(ISearchFilesSolution s)
    {
        string a = CreateFile("A/readme.txt");
        string b = CreateFile("B/readme.txt");

        List<FileEntry>[] table = s.BuildTable(_testRoot);
        List<FileEntry> result = s.Find("readme.txt", table);

        Assert.Equal(2, result.Count);
        var paths = result.Select(e => e.FullPath).ToHashSet();
        Assert.Contains(a, paths);
        Assert.Contains(b, paths);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Find_DoesNotReturnFileWithSameHashDifferentName(ISearchFilesSolution s)
    {
        // Подберём коллизию: "ab" и "ba" имеют одинаковую сумму кодов,
        // но улучшенная хеш-функция должна различать их по умолчанию.
        // Поэтому подберём коллизию через сам алгоритм.
        CreateFile("ab.txt");
        CreateFile("zz.txt");

        List<FileEntry>[] table = s.BuildTable(_testRoot);

        List<FileEntry> result = s.Find("ab.txt", table);

        Assert.Single(result);
        Assert.Equal("ab.txt", result[0].Name);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Find_NullName_Throws(ISearchFilesSolution s)
    {
        List<FileEntry>[] table = s.BuildTable(_testRoot);

        Assert.Throws<ArgumentNullException>(() => s.Find(null!, table));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Find_TableWrongSize_Throws(ISearchFilesSolution s)
    {
        List<FileEntry>[] wrongSized = new List<FileEntry>[10];

        Assert.Throws<ArgumentException>(() => s.Find("any.txt", wrongSized));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Find_NullTable_Throws(ISearchFilesSolution s)
    {
        Assert.Throws<ArgumentException>(() => s.Find("any.txt", null!));
    }

    // ─── Run(): UI-сценарии ───────────────────────────────────────────────────────

    [Fact]
    public void Run_TaskExample_FindsFileAndExits()
    {
        CreateFile("Application/main.m");

        string output = CaptureRun([_testRoot, "1", "main.m", "2"]);

        Assert.Contains("Введите имя папки", output);
        Assert.Contains("1: - Поиск файла по имени  2: - Выход", output);
        Assert.Contains(Path.Combine(_testRoot, "Application", "main.m"), output);
    }

    [Fact]
    public void Run_CaseInsensitive_FindsFile()
    {
        string path = CreateFile("Application/main.m");

        string output = CaptureRun([_testRoot, "1", "MAIN.M", "2"]);

        Assert.Contains(path, output);
    }

    [Fact]
    public void Run_FileNotFound_PrintsErrorAndContinues()
    {
        string output = CaptureRun([_testRoot, "1", "nope.txt", "2"]);

        Assert.Contains("не найден", output);
    }

    [Fact]
    public void Run_InvalidMenuChoice_AsksAgain()
    {
        string output = CaptureRun([_testRoot, "abc", "99", "2"]);

        Assert.Contains("Неверный ввод", output);
    }

    [Fact]
    public void Run_NonExistentFolder_AsksAgain()
    {
        string bogus = Path.Combine(_testRoot, "no_such_folder");

        string output = CaptureRun([bogus, _testRoot, "2"]);

        Assert.Contains("не найдена", output);
    }

    [Fact]
    public void Run_EmptyFolderInput_AsksAgain()
    {
        string output = CaptureRun(["", _testRoot, "2"]);

        Assert.Contains("Имя папки не может быть пустым", output);
    }

    [Fact]
    public void Run_EmptyFileNameInput_DoesNotCrash()
    {
        string output = CaptureRun([_testRoot, "1", "", "2"]);

        Assert.Contains("Имя файла не может быть пустым", output);
    }

    [Fact]
    public void Run_MultipleSearches_AllWork()
    {
        CreateFile("Application/main.m");
        CreateFile("Docs/readme.txt");

        string output = CaptureRun([_testRoot, "1", "main.m", "1", "readme.txt", "2"]);

        Assert.Contains(Path.Combine(_testRoot, "Application", "main.m"), output);
        Assert.Contains(Path.Combine(_testRoot, "Docs", "readme.txt"), output);
    }

    // ─── Вспомогательные методы ───────────────────────────────────────────────────

    private string CreateFile(string relativePath)
    {
        string normalized = relativePath.Replace('/', Path.DirectorySeparatorChar);
        string fullPath = Path.Combine(_testRoot, normalized);
        string? dir = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);
        File.WriteAllText(fullPath, "test");
        return fullPath;
    }

    private static int CountEntries(List<FileEntry>[] table)
    {
        int total = 0;
        foreach (var list in table)
            total += list.Count;
        return total;
    }

    private static string CaptureRun(string[] inputs)
    {
        var sw = new StringWriter();
        var sr = new StringReader(string.Join(Environment.NewLine, inputs) + Environment.NewLine);
        var oldOut = Console.Out;
        var oldIn = Console.In;
        Console.SetOut(sw);
        Console.SetIn(sr);
        try
        {
            new SearchFilesSolution().Run();
        }
        finally
        {
            Console.SetOut(oldOut);
            Console.SetIn(oldIn);
        }

        var raw = sw.ToString().Replace("\r\n", "\n");
        return Regex.Replace(raw, "\u001b\\[[0-9;]*m", "");
    }
}
