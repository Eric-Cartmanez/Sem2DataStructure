using System.Text.RegularExpressions;
using Task23.Settlements;

namespace Tasks.Tests;

[Collection(nameof(ConsoleCollection))]
public class Task23SettlementsTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new SettlementsSolution()];
    }

    // ─── GetHash: базовые случаи ─────────────────────────────────────────────────

    [Fact]
    public void GetHash_EmptyString_ReturnsZero()
    {
        Assert.Equal(0, SettlementsSolution.GetHash(""));
    }

    [Theory]
    [InlineData("A", 65)]
    [InlineData("Z", 90)]
    [InlineData("a", 97)]
    [InlineData("z", 122)]
    public void GetHash_SingleAsciiChar_ReturnsItsCode(string s, int expected)
    {
        Assert.Equal(expected, SettlementsSolution.GetHash(s));
    }

    [Theory]
    [InlineData("AB", 131)]
    [InlineData("ABC", 198)]
    [InlineData("ABCD", 10)]   // 65+66+67+68 = 266; 266 % 256 = 10
    public void GetHash_MultipleAsciiChars_ReturnsSumModulo256(string s, int expected)
    {
        Assert.Equal(expected, SettlementsSolution.GetHash(s));
    }

    [Fact]
    public void GetHash_CyrillicAntipovo_Returns212()
    {
        // А=1040, Н=1053, Т=1058, И=1048, П=1055, О=1054, В=1042, О=1054 → 8404 % 256 = 212
        Assert.Equal(212, SettlementsSolution.GetHash("АНТИПОВО"));
    }

    [Fact]
    public void GetHash_DoesNotApplyToUpperItself()
    {
        // Контракт: ToUpper делает BuildTable, а сама GetHash работает с тем, что подали.
        Assert.NotEqual(SettlementsSolution.GetHash("a"), SettlementsSolution.GetHash("A"));
    }

    [Theory]
    [InlineData("ABC", "BCA")]
    [InlineData("ABC", "CAB")]
    [InlineData("ABC", "BAC")]
    [InlineData("АБВ", "ВБА")]
    public void GetHash_Permutations_ProduceSameHash(string a, string b)
    {
        // Известная слабость суммы кодов — любая перестановка символов даёт тот же хеш.
        Assert.Equal(SettlementsSolution.GetHash(a), SettlementsSolution.GetHash(b));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void GetHash_AlwaysInRange0To255(ISettlementsSolution _)
    {
        foreach (string town in SettlementsSolution.ArrTowns)
        {
            int h = SettlementsSolution.GetHash(town.ToUpper());
            Assert.InRange(h, 0, 255);
        }
    }

    // ─── BuildTable: вырожденные случаи ──────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BuildTable_EmptyInput_NoCollisionsAndAllCellsNull(ISettlementsSolution s)
    {
        string[] table = s.BuildTable(Array.Empty<string>(), out int collisions);

        Assert.Equal(0, collisions);
        Assert.Equal(256, table.Length);
        Assert.All(table, cell => Assert.Null(cell));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BuildTable_SingleWord_PlacedAtHashIndex_NoCollisions(ISettlementsSolution s)
    {
        // "Антипово".ToUpper() → "АНТИПОВО" → hash 212
        string[] table = s.BuildTable(new[] { "Антипово" }, out int collisions);

        Assert.Equal(0, collisions);
        Assert.Equal("Антипово", table[212]);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BuildTable_LowerAndUpperCase_LandInSameIndex(ISettlementsSolution s)
    {
        // ToUpper применяется к каждому слову → разные регистры дают одну позицию,
        // т.е. вторая запись обязана дать коллизию.
        string[] table = s.BuildTable(new[] { "Борок", "БОРОК" }, out int collisions);

        Assert.Equal(1, collisions);

        int hash = SettlementsSolution.GetHash("БОРОК");
        Assert.Equal("Борок", table[hash]);
        Assert.Equal("БОРОК", table[(hash + 3) % 256]);
    }

    // ─── BuildTable: разрешение коллизий по массиву смещений ─────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BuildTable_TwoCollidingWords_SecondShiftsBy3(ISettlementsSolution s)
    {
        string[] table = s.BuildTable(new[] { "ABC", "BCA" }, out int collisions);

        Assert.Equal(1, collisions);
        Assert.Equal("ABC", table[198]);
        Assert.Equal("BCA", table[201]); // 198 + 3
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BuildTable_ThreeCollidingWords_ThirdShiftsBy3Plus5(ISettlementsSolution s)
    {
        string[] table = s.BuildTable(new[] { "ABC", "BCA", "CAB" }, out int collisions);

        // ABC: 0; BCA: 1 (198→201); CAB: 2 (198 занят → 201 занят → 206 free)
        Assert.Equal(3, collisions);
        Assert.Equal("ABC", table[198]);
        Assert.Equal("BCA", table[201]);
        Assert.Equal("CAB", table[206]); // 198 + 3 + 5
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BuildTable_FifthCollidingWord_CyclesShiftsArrayBackTo3(ISettlementsSolution s)
    {
        // 5 строк с одинаковым хешем 198 — проверяем, что после 3-х смещений (3,5,6)
        // 4-е смещение снова берётся как arrShifts[0] = 3.
        string[] table = s.BuildTable(
            new[] { "ABC", "BCA", "CAB", "BAC", "ACB" }, out int collisions);

        // Коллизии по словам: 0 + 1 + 2 + 3 + 4 = 10
        Assert.Equal(10, collisions);

        Assert.Equal("ABC", table[198]);
        Assert.Equal("BCA", table[201]); // +3
        Assert.Equal("CAB", table[206]); // +3+5
        Assert.Equal("BAC", table[212]); // +3+5+6
        Assert.Equal("ACB", table[215]); // +3+5+6+3 ← цикл вернулся к arrShifts[0]
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BuildTable_CollisionAt254_WrapsAroundModulo256(ISettlementsSolution s)
    {
        // BuildTable вызывает town.ToUpper() перед хешированием, поэтому слова
        // должны быть устойчивы к ToUpper. Берём заглавные ASCII-буквы.
        // "ZZJ" = 90+90+74 = 254; "ZYK" = 90+89+75 = 254 — оба дают хеш 254.
        // Второе слово при коллизии должно сместиться на 3 → (254 + 3) % 256 = 1.
        string a = "ZZJ";
        string b = "ZYK";

        string[] table = s.BuildTable(new[] { a, b }, out int collisions);

        Assert.Equal(1, collisions);
        Assert.Equal(a, table[254]);
        Assert.Equal(b, table[1]); // wrap: (254 + 3) % 256
    }

    // ─── BuildTable: размер и сохранность данных ─────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BuildTable_TableSizeIsAlways256(ISettlementsSolution s)
    {
        Assert.Equal(256, s.BuildTable(Array.Empty<string>(), out _).Length);
        Assert.Equal(256, s.BuildTable(new[] { "x" }, out _).Length);
        Assert.Equal(256, s.BuildTable(SettlementsSolution.ArrTowns, out _).Length);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BuildTable_AllTownsAreStoredInTable(ISettlementsSolution s)
    {
        string[] table = s.BuildTable(SettlementsSolution.ArrTowns, out _);

        // Дубликатов во входе нет, поэтому в таблице должно лежать ровно 143 строки.
        int filled = table.Count(cell => cell != null);
        Assert.Equal(SettlementsSolution.ArrTowns.Length, filled);

        // И каждое исходное слово действительно присутствует.
        var bag = table.Where(c => c != null).ToHashSet();
        foreach (string town in SettlementsSolution.ArrTowns)
            Assert.Contains(town, bag);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BuildTable_IsDeterministic(ISettlementsSolution s)
    {
        string[] t1 = s.BuildTable(SettlementsSolution.ArrTowns, out int c1);
        string[] t2 = s.BuildTable(SettlementsSolution.ArrTowns, out int c2);

        Assert.Equal(c1, c2);
        Assert.Equal(t1, t2);
    }

    // ─── BuildTable: эталонные значения из условия задачи ────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BuildTable_ArrTowns_ProducesExpectedCollisionCount(ISettlementsSolution s)
    {
        s.BuildTable(SettlementsSolution.ArrTowns, out int collisions);

        Assert.Equal(121, collisions);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BuildTable_ArrTowns_ElementAtIndex4IsKrasovicy(ISettlementsSolution s)
    {
        string[] table = s.BuildTable(SettlementsSolution.ArrTowns, out _);

        Assert.Equal("Красовицы", table[4]);
    }

    // ─── Run(): консольный вывод ─────────────────────────────────────────────────

    [Fact]
    public void Run_PrintsCollisionsCountAndElementAtIndex4()
    {
        string output = CaptureRun();

        Assert.Contains("121", output);
        Assert.Contains("Красовицы", output);
    }

    [Fact]
    public void Run_PrintsHumanReadableLabels()
    {
        string output = CaptureRun();

        Assert.Contains("коллизий", output, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("индекс", output, StringComparison.OrdinalIgnoreCase);
    }

    private static string CaptureRun()
    {
        var sw = new StringWriter();
        var oldOut = Console.Out;
        Console.SetOut(sw);
        try
        {
            new SettlementsSolution().Run();
        }
        finally
        {
            Console.SetOut(oldOut);
        }

        var raw = sw.ToString().Replace("\r\n", "\n");
        return Regex.Replace(raw, "\u001b\\[[0-9;]*m", "");
    }
}
