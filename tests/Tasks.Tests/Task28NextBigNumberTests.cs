using Task28.NextBigNumber;

namespace Tasks.Tests;

public class Task28NextBigNumberTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new NextBigNumberSolution()];
    }

    // ─── Примеры из условия задачи ───────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void NextBigNumber_12_Returns21(INextBigNumberSolution s)
    {
        Assert.Equal(21, s.NextBigNumber("12"));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void NextBigNumber_513_Returns531(INextBigNumberSolution s)
    {
        Assert.Equal(531, s.NextBigNumber("513"));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void NextBigNumber_2184_Returns2418(INextBigNumberSolution s)
    {
        Assert.Equal(2418, s.NextBigNumber("2184"));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void NextBigNumber_222_ReturnsMinusOne(INextBigNumberSolution s)
    {
        Assert.Equal(-1, s.NextBigNumber("222"));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void NextBigNumber_931_ReturnsMinusOne(INextBigNumberSolution s)
    {
        Assert.Equal(-1, s.NextBigNumber("931"));
    }

    // ─── Вырожденные случаи ──────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void NextBigNumber_EmptyString_ReturnsMinusOne(INextBigNumberSolution s)
    {
        Assert.Equal(-1, s.NextBigNumber(""));
    }

    [Theory]
    [InlineData("0")]
    [InlineData("5")]
    [InlineData("9")]
    public void NextBigNumber_SingleDigit_ReturnsMinusOne(string n)
    {
        Assert.Equal(-1, new NextBigNumberSolution().NextBigNumber(n));
    }

    [Theory]
    [InlineData("00")]
    [InlineData("11")]
    [InlineData("99")]
    public void NextBigNumber_TwoEqualDigits_ReturnsMinusOne(string n)
    {
        Assert.Equal(-1, new NextBigNumberSolution().NextBigNumber(n));
    }

    [Theory]
    [InlineData("21")]
    [InlineData("54321")]
    [InlineData("987654321")]
    public void NextBigNumber_StrictlyDescending_ReturnsMinusOne(string n)
    {
        // Уже максимальная перестановка своих цифр.
        Assert.Equal(-1, new NextBigNumberSolution().NextBigNumber(n));
    }

    // ─── Соседние перестановки ───────────────────────────────────────────────────

    [Theory]
    [InlineData("1234", 1243)]
    [InlineData("12345", 12354)]
    public void NextBigNumber_AscendingTail_OnlyLastTwoSwap(string input, int expected)
    {
        Assert.Equal(expected, new NextBigNumberSolution().NextBigNumber(input));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void NextBigNumber_PicksRightmostGreaterThanPivot(INextBigNumberSolution s)
    {
        // 1432: pivot = '1', в хвосте {4,3,2} все больше pivot. Выбрать нужно
        // самую правую (наименьшую из подходящих) — двойку, иначе получится не
        // следующая, а просто бо́льшая перестановка.
        Assert.Equal(2134, s.NextBigNumber("1432"));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void NextBigNumber_LongTail_GetsReversed(INextBigNumberSolution s)
    {
        // 11321 → 12113. После swap'а хвост 3,1,1 переворачивается в 1,1,3.
        Assert.Equal(12113, s.NextBigNumber("11321"));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void NextBigNumber_WithZeroInTail_HandlesCorrectly(INextBigNumberSolution s)
    {
        // 120 → 201: pivot = '1', swap c '2', хвост '1,0' → '0,1'.
        Assert.Equal(201, s.NextBigNumber("120"));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void NextBigNumber_WithZeroInsideMiddle_HandlesCorrectly(INextBigNumberSolution s)
    {
        // 1023 → 1032: pivot — двойка, рядом тройка, остальное не трогаем.
        Assert.Equal(1032, s.NextBigNumber("1023"));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void NextBigNumber_RepeatedDigitInTail_PicksRightmostOccurrence(INextBigNumberSolution s)
    {
        // 1531: pivot = '1', в хвосте {5,3,1}. Подходит только '3' и '5';
        // самая правая большая pivot — '3' на индексе 2. swap → 3,5,1,1
        // → reverse хвоста → 3,1,1,5.
        Assert.Equal(3115, s.NextBigNumber("1531"));
    }

    // ─── Свойство: результат — перестановка тех же цифр и строго больше ─────────

    [Theory]
    [InlineData("12")]
    [InlineData("513")]
    [InlineData("2184")]
    [InlineData("1432")]
    [InlineData("11321")]
    [InlineData("1023")]
    public void NextBigNumber_Result_IsPermutationOfInput_AndGreater(string input)
    {
        int result = new NextBigNumberSolution().NextBigNumber(input);

        Assert.True(result > int.Parse(input),
            $"Результат {result} должен быть строго больше входа {input}");

        string resultStr = result.ToString();
        Assert.Equal(input.Length, resultStr.Length);

        var inputDigits = input.OrderBy(c => c).ToArray();
        var resultDigits = resultStr.OrderBy(c => c).ToArray();
        Assert.Equal(inputDigits, resultDigits);
    }

    // ─── Run(): консольный вывод ─────────────────────────────────────────────────

    [Fact]
    public void Run_PrintsAllExamplesFromTask()
    {
        string output = CaptureRun();

        Assert.Contains("12 --> 21", output);
        Assert.Contains("513 --> 531", output);
        Assert.Contains("2184 --> 2418", output);
        Assert.Contains("222 --> -1", output);
        Assert.Contains("931 --> -1", output);
    }

    private static string CaptureRun()
    {
        var sw = new StringWriter();
        var oldOut = Console.Out;
        Console.SetOut(sw);
        try
        {
            new NextBigNumberSolution().Run();
        }
        finally
        {
            Console.SetOut(oldOut);
        }
        return sw.ToString().Replace("\r\n", "\n");
    }
}
