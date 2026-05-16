using Task18.HanoiTowers;
using Tower = Task18.HanoiTowers.HanoiTowersSolution.Tower;

namespace Tasks.Tests;

public class Task18HanoiTowersTests : IDisposable
{
    private readonly TextWriter _oldOut;

    public Task18HanoiTowersTests()
    {
        _oldOut = Console.Out;
        Console.SetOut(TextWriter.Null);
    }

    public void Dispose() => Console.SetOut(_oldOut);

    private static (Tower t1, Tower t2, Tower t3) BuildInitial(int n)
    {
        var t1 = new Tower("A");
        var t2 = new Tower("B");
        var t3 = new Tower("C");
        for (int i = n; i > 0; i--)
        {
            t1.Push(i);
        }
        return (t1, t2, t3);
    }

    // ─── Состояние башен после решения ─────────────────────────────────────────────

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(8)]
    public void Move_TransfersAllDisksFromAToB(int n)
    {
        var s = new HanoiTowersSolution();
        var (t1, t2, t3) = BuildInitial(n);

        s.Move(t1, t2, t3, n);

        Assert.Equal(0, t1.Count);
        Assert.Equal(0, t3.Count);
        Assert.Equal(n, t2.Count);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(8)]
    public void Move_LeavesDisksOnTargetInCorrectOrder(int n)
    {
        var s = new HanoiTowersSolution();
        var (t1, t2, t3) = BuildInitial(n);

        s.Move(t1, t2, t3, n);

        for (int i = 1; i <= n; i++)
        {
            Assert.Equal(i, t2.Pop());
        }
    }

    // ─── Поведение Tower ───────────────────────────────────────────────────────────

    [Fact]
    public void Tower_PushPop_WorksAsStack()
    {
        var t = new Tower("X");
        t.Push(5);
        t.Push(3);
        t.Push(1);

        Assert.Equal(3, t.Count);
        Assert.Equal(5, t.GetDisk(0));
        Assert.Equal(3, t.GetDisk(1));
        Assert.Equal(1, t.GetDisk(2));

        Assert.Equal(1, t.Pop());
        Assert.Equal(3, t.Pop());
        Assert.Equal(5, t.Pop());
        Assert.Equal(0, t.Count);
    }

    [Fact]
    public void Run_ThreeDisks_MatchesReferenceOutput()
    {
        const string reference =
            "ИГРА Ханойские башни\n" +
            "Введите количество колец: \n" +
            "Шаг №0\n" +
            "1   0   0\n" +
            "2   0   0\n" +
            "3   0   0\n" +
            "\n" +
            "Шаг №1\n" +
            "0   0   0\n" +
            "2   0   0\n" +
            "3   1   0\n" +
            "\n" +
            "Шаг №2\n" +
            "0   0   0\n" +
            "0   0   0\n" +
            "3   1   2\n" +
            "\n" +
            "Шаг №3\n" +
            "0   0   0\n" +
            "0   0   1\n" +
            "3   0   2\n" +
            "\n" +
            "Шаг №4\n" +
            "0   0   0\n" +
            "0   0   1\n" +
            "0   3   2\n" +
            "\n" +
            "Шаг №5\n" +
            "0   0   0\n" +
            "0   0   0\n" +
            "1   3   2\n" +
            "\n" +
            "Шаг №6\n" +
            "0   0   0\n" +
            "0   2   0\n" +
            "1   3   0\n" +
            "\n" +
            "Шаг №7\n" +
            "0   1   0\n" +
            "0   2   0\n" +
            "0   3   0\n" +
            "ИГРА окончена\n";

        var sw = new StringWriter();
        var oldIn = Console.In;
        Console.SetOut(sw);
        Console.SetIn(new StringReader("3\n"));

        try
        {
            new HanoiTowersSolution().Run();
        }
        finally
        {
            Console.SetOut(TextWriter.Null);
            Console.SetIn(oldIn);
        }

        var actual = sw.ToString().Replace("\r\n", "\n");
        actual = System.Text.RegularExpressions.Regex.Replace(actual, "\u001b\\[[0-9;]*m", "");
        Assert.Equal(reference, actual);
    }
}
