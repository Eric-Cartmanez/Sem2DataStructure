using Task02.Nword;

namespace Tasks.Tests;

public class Task02NwordTests
{
    private const string RussianAlphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new NwordRecursive()];
        yield return [new NwordIterative()];
    }

    // ─── Количество слов = M^N ────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void WordCount_EqualsMPowerN(INwordSolution solution)
    {
        Assert.Equal(64, solution.GenerateWords(3, "АБВГ").Count());  // 4^3
        Assert.Single(solution.GenerateWords(1, "А"));                  // 1^1
        Assert.Equal(4,  solution.GenerateWords(2, "АБ").Count());    // 2^2
        Assert.Equal(8,  solution.GenerateWords(3, "АБ").Count());    // 2^3
        Assert.Equal(25, solution.GenerateWords(2, "АБВГД").Count()); // 5^2
    }

    // ─── Нет дубликатов ───────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Words_NoDuplicates(INwordSolution solution)
    {
        var words = solution.GenerateWords(3, "АБВГ").ToList();
        Assert.Equal(words.Count, words.Distinct().Count());
    }

    // ─── Все слова длиной N ───────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Words_AllHaveCorrectLength(INwordSolution solution)
    {
        var words = solution.GenerateWords(3, "АБВГ");
        Assert.All(words, w => Assert.Equal(3, w.Length));
    }

    // ─── Только буквы из алфавита ─────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Words_ContainOnlyAllowedLetters(INwordSolution solution)
    {
        string alphabet = "АБВГ";
        var words = solution.GenerateWords(3, alphabet);
        Assert.All(words, w => Assert.All(w, c => Assert.Contains(c, alphabet)));
    }

    // ─── Оба метода дают одинаковое множество ─────────────────────────────────────

    [Fact]
    public void RecursiveAndIterative_ProduceSameSet()
    {
        var recursive = new NwordRecursive().GenerateWords(3, "АБВГ").ToHashSet();
        var iterative = new NwordIterative().GenerateWords(3, "АБВГ").ToHashSet();
        Assert.Equal(recursive, iterative);
    }

    // ─── Конкретные значения ──────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SmallCase_M2N2_ExactWords(INwordSolution solution)
    {
        var expected = new HashSet<string> { "АА", "АБ", "БА", "ББ" };
        Assert.Equal(expected, solution.GenerateWords(2, "АБ").ToHashSet());
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SmallCase_M1N1_OnlyА(INwordSolution solution)
    {
        var word = Assert.Single(solution.GenerateWords(1, "А"));
        Assert.Equal("А", word);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SmallCase_M3N1_ContainsАБВ(INwordSolution solution)
    {
        Assert.Equal(new HashSet<string> { "А", "Б", "В" },
                     solution.GenerateWords(1, "АБВ").ToHashSet());
    }
}
