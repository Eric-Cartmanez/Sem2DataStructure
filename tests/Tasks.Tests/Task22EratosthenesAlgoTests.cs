using System.Text.RegularExpressions;
using Task22.EratosthenesAlgo;

namespace Tasks.Tests;

[Collection(nameof(ConsoleCollection))]
public class Task22EratosthenesAlgoTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new EratosthenesAlgoSolution()];
    }

    // ─── FindPrimesNumbers: конкретные значения ──────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPrimes_Max2_ReturnsSingleTwo(IEratosthenesAlgoSolution s)
    {
        List<int> primes = s.FindPrimesNumbers(2);

        Assert.Equal([2], primes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPrimes_Max3_ReturnsTwoAndThree(IEratosthenesAlgoSolution s)
    {
        List<int> primes = s.FindPrimesNumbers(3);

        Assert.Equal([2, 3], primes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPrimes_Max10_ReturnsExpectedPrimes(IEratosthenesAlgoSolution s)
    {
        List<int> primes = s.FindPrimesNumbers(10);

        Assert.Equal([2, 3, 5, 7], primes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPrimes_Max30_ReturnsExpectedPrimes(IEratosthenesAlgoSolution s)
    {
        List<int> primes = s.FindPrimesNumbers(30);

        Assert.Equal([2, 3, 5, 7, 11, 13, 17, 19, 23, 29], primes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPrimes_Max100_ReturnsExpectedPrimes(IEratosthenesAlgoSolution s)
    {
        int[] expected =
        [
            2, 3, 5, 7, 11, 13, 17, 19, 23, 29,
            31, 37, 41, 43, 47, 53, 59, 61, 67, 71,
            73, 79, 83, 89, 97,
        ];

        List<int> primes = s.FindPrimesNumbers(100);

        Assert.Equal(expected, primes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    [Trait("Category", "Slow")]
    public void FindPrimes_Max1000_HasCorrectCount(IEratosthenesAlgoSolution s)
    {
        // pi(1000) = 168
        List<int> primes = s.FindPrimesNumbers(1000);

        Assert.Equal(168, primes.Count);
    }

    // ─── FindPrimesNumbers: граничные значения ───────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPrimes_MaxIsPrime_IncludesMax(IEratosthenesAlgoSolution s)
    {
        List<int> primes = s.FindPrimesNumbers(13);

        Assert.Contains(13, primes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPrimes_MaxIsComposite_DoesNotIncludeMax(IEratosthenesAlgoSolution s)
    {
        List<int> primes = s.FindPrimesNumbers(15);

        Assert.DoesNotContain(15, primes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPrimes_DoesNotContainOne(IEratosthenesAlgoSolution s)
    {
        List<int> primes = s.FindPrimesNumbers(50);

        Assert.DoesNotContain(1, primes);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPrimes_DoesNotContainZeroOrNegatives(IEratosthenesAlgoSolution s)
    {
        List<int> primes = s.FindPrimesNumbers(50);

        Assert.DoesNotContain(0, primes);
        Assert.All(primes, p => Assert.True(p > 1));
    }

    // ─── FindPrimesNumbers: структурные свойства результата ──────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPrimes_ResultIsSortedAscending(IEratosthenesAlgoSolution s)
    {
        List<int> primes = s.FindPrimesNumbers(200);

        for (int i = 1; i < primes.Count; i++)
            Assert.True(primes[i] > primes[i - 1], $"Не отсортировано на позиции {i}");
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPrimes_ResultHasNoDuplicates(IEratosthenesAlgoSolution s)
    {
        List<int> primes = s.FindPrimesNumbers(200);

        Assert.Equal(primes.Count, primes.Distinct().Count());
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPrimes_AllElementsArePrimes(IEratosthenesAlgoSolution s)
    {
        List<int> primes = s.FindPrimesNumbers(500);

        foreach (int p in primes)
            Assert.True(IsPrime(p), $"{p} не простое");
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPrimes_ContainsAllPrimesUpToMax(IEratosthenesAlgoSolution s)
    {
        const int max = 500;
        List<int> primes = s.FindPrimesNumbers(max);
        var set = primes.ToHashSet();

        for (int i = 2; i <= max; i++)
        {
            if (IsPrime(i))
                Assert.Contains(i, set);
        }
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPrimes_AllValuesAreWithinRange(IEratosthenesAlgoSolution s)
    {
        const int max = 200;
        List<int> primes = s.FindPrimesNumbers(max);

        Assert.All(primes, p => Assert.InRange(p, 2, max));
    }

    // ─── FindPrimesNumbers: ошибки ───────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPrimes_MaxLessThan2_Throws(IEratosthenesAlgoSolution s)
    {
        Assert.Throws<ArgumentException>(() => s.FindPrimesNumbers(1));
        Assert.Throws<ArgumentException>(() => s.FindPrimesNumbers(0));
        Assert.Throws<ArgumentException>(() => s.FindPrimesNumbers(-1));
        Assert.Throws<ArgumentException>(() => s.FindPrimesNumbers(-100));
    }

    // ─── FindPrimesNumbers: детерминизм / чистота ────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPrimes_IsDeterministic(IEratosthenesAlgoSolution s)
    {
        List<int> first = s.FindPrimesNumbers(50);
        List<int> second = s.FindPrimesNumbers(50);

        Assert.Equal(first, second);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPrimes_LargerMax_IsSupersetOfSmallerMax(IEratosthenesAlgoSolution s)
    {
        List<int> small = s.FindPrimesNumbers(20);
        List<int> large = s.FindPrimesNumbers(100);

        foreach (int p in small)
            Assert.Contains(p, large);
    }

    // ─── Run(): UI-сценарии ──────────────────────────────────────────────────────

    [Fact]
    public void Run_PrintsPrimesForMax10()
    {
        string output = CaptureRun("10");

        Assert.Contains("2 3 5 7", output);
    }

    [Fact]
    public void Run_PrintsPromptForMax()
    {
        string output = CaptureRun("2");

        Assert.Contains("максимальное", output, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Run_PrintsAllPrimesUpToMax30()
    {
        string output = CaptureRun("30");

        Assert.Contains("2 3 5 7 11 13 17 19 23 29", output);
    }

    // ─── Вспомогательные методы ──────────────────────────────────────────────────

    private static bool IsPrime(int n)
    {
        if (n < 2) return false;
        if (n < 4) return true;
        if (n % 2 == 0) return false;
        for (int i = 3; (long)i * i <= n; i += 2)
            if (n % i == 0) return false;
        return true;
    }

    private static string CaptureRun(params string[] inputs)
    {
        var sw = new StringWriter();
        var sr = new StringReader(string.Join(Environment.NewLine, inputs) + Environment.NewLine);
        var oldOut = Console.Out;
        var oldIn = Console.In;
        Console.SetOut(sw);
        Console.SetIn(sr);
        try
        {
            new EratosthenesAlgoSolution().Run();
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
