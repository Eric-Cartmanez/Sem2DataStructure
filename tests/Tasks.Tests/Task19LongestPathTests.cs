using System.Text.RegularExpressions;
using Task19.LongestPath;

namespace Tasks.Tests;

[Collection(nameof(ConsoleCollection))]
public class Task19LongestPathTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new LongestPathSolution()];
        yield return [new LongestPathAltSolution()];
    }

    // ─── Граничные случаи ──────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void EmptyArray_ReturnsZeroLengthAndEmptyPath(ILongestPathSolution s)
    {
        int[,] arr = new int[0, 0];

        Point[] path = s.FindLongestPath(arr, out int length);

        Assert.Equal(0, length);
        Assert.Empty(path);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void EmptyByZeroColumns_ReturnsZeroLengthAndEmptyPath(ILongestPathSolution s)
    {
        int[,] arr = new int[3, 0];

        Point[] path = s.FindLongestPath(arr, out int length);

        Assert.Equal(0, length);
        Assert.Empty(path);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SingleCell_ReturnsPathOfLengthOne(ILongestPathSolution s)
    {
        int[,] arr = { { 42 } };

        Point[] path = s.FindLongestPath(arr, out int length);

        Assert.Equal(1, length);
        Assert.Single(path);
        Assert.Equal(0, path[0].Row);
        Assert.Equal(0, path[0].Column);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void AllEqualValues_ReturnsPathOfLengthOne(ILongestPathSolution s)
    {
        int[,] arr =
        {
            { 5, 5, 5 },
            { 5, 5, 5 },
            { 5, 5, 5 },
        };

        Point[] path = s.FindLongestPath(arr, out int length);

        Assert.Equal(1, length);
        Assert.Single(path);
    }

    // ─── Пример из условия (Task.md) ───────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TaskExample_ReturnsLengthSix(ILongestPathSolution s)
    {
        int[,] arr =
        {
            { 2, 5, 1, 0 },
            { 3, 3, 1, 9 },
            { 4, 4, 7, 8 },
        };

        Point[] path = s.FindLongestPath(arr, out int length);

        Assert.Equal(6, length);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TaskExample_PathValuesAre_1_3_4_7_8_9(ILongestPathSolution s)
    {
        int[,] arr =
        {
            { 2, 5, 1, 0 },
            { 3, 3, 1, 9 },
            { 4, 4, 7, 8 },
        };

        Point[] path = s.FindLongestPath(arr, out _);
        int[] values = ToValues(arr, path);

        Assert.Equal(new[] { 1, 3, 4, 7, 8, 9 }, values);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TaskExample_PathStartsAt_1_2(ILongestPathSolution s)
    {
        // (1, 2) — единственная клетка с памяти 6, поэтому путь обязан начинаться там.
        int[,] arr =
        {
            { 2, 5, 1, 0 },
            { 3, 3, 1, 9 },
            { 4, 4, 7, 8 },
        };

        Point[] path = s.FindLongestPath(arr, out _);

        Assert.Equal(1, path[0].Row);
        Assert.Equal(2, path[0].Column);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TaskExample_PathEndsAt_1_3(ILongestPathSolution s)
    {
        // 9 встречается в массиве один раз — в (1, 3).
        int[,] arr =
        {
            { 2, 5, 1, 0 },
            { 3, 3, 1, 9 },
            { 4, 4, 7, 8 },
        };

        Point[] path = s.FindLongestPath(arr, out _);

        Assert.Equal(1, path[^1].Row);
        Assert.Equal(3, path[^1].Column);
    }

    // ─── Известные конструкции с известной длиной ──────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void StrictlyIncreasingRow_ReturnsRowAsPath(ILongestPathSolution s)
    {
        int[,] arr = { { 1, 2, 3, 4, 5 } };

        Point[] path = s.FindLongestPath(arr, out int length);

        Assert.Equal(5, length);
        Assert.Equal(new[] { 1, 2, 3, 4, 5 }, ToValues(arr, path));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void StrictlyIncreasingColumn_ReturnsColumnAsPath(ILongestPathSolution s)
    {
        int[,] arr =
        {
            { 1 },
            { 2 },
            { 3 },
            { 4 },
        };

        Point[] path = s.FindLongestPath(arr, out int length);

        Assert.Equal(4, length);
        Assert.Equal(new[] { 1, 2, 3, 4 }, ToValues(arr, path));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Snake3x3_FillsWholeMatrix(ILongestPathSolution s)
    {
        // 1→2→3→4→5→6→7→8→9 змейкой по 4-соседям.
        int[,] arr =
        {
            { 1, 2, 3 },
            { 6, 5, 4 },
            { 7, 8, 9 },
        };

        Point[] path = s.FindLongestPath(arr, out int length);

        Assert.Equal(9, length);
        Assert.Equal(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, ToValues(arr, path));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TwoByTwoStrictIncrease_ReturnsLengthThree(ILongestPathSolution s)
    {
        // Любая возрастающая цепочка по 4-соседям имеет длину 3.
        // Например: 1 → 2 → 4  или  1 → 3 → 4.
        int[,] arr =
        {
            { 1, 2 },
            { 3, 4 },
        };

        Point[] path = s.FindLongestPath(arr, out int length);

        Assert.Equal(3, length);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void DiagonalNeighbours_AreNotConsidered(ILongestPathSolution s)
    {
        // Если бы диагонали считались соседями, длина была бы 4 (1→2→3→4).
        // По 4-соседям максимум — 1 (никакие два соседа по горизонтали/вертикали
        // не образуют возрастающей пары, т.к. одинаковы).
        int[,] arr =
        {
            { 1, 9, 2, 9 },
            { 9, 9, 9, 9 },
            { 9, 9, 9, 9 },
            { 9, 3, 9, 4 },
        };

        // Здесь самый длинный путь — это просто переход в любую "9" из соседа.
        // Но проверим только что длина не учитывает диагональ для (0,0)→(1,1).
        Point[] path = s.FindLongestPath(arr, out int length);

        AssertPathIsValid(arr, path, length);
    }

    // ─── Свойства любого корректного результата ───────────────────────────────────

    public static IEnumerable<object[]> ValidPathArrays()
    {
        yield return [new int[,] { { 42 } }];
        yield return [new int[,] { { 1, 2, 3 }, { 6, 5, 4 }, { 7, 8, 9 } }];
        yield return [new int[,] { { 2, 5, 1, 0 }, { 3, 3, 1, 9 }, { 4, 4, 7, 8 } }];
        yield return [new int[,] { { 5, 5 }, { 5, 5 } }];
        yield return [new int[,] { { 1, 2 }, { 4, 3 } }];
        yield return [new int[,]
            {
                { 9, 9, 4 },
                { 6, 6, 8 },
                { 2, 1, 1 },
            }];
    }

    [Theory]
    [MemberData(nameof(ValidPathArrays))]
    public void ReturnedPathIsAlwaysValid(int[,] arr)
    {
        var s = new LongestPathSolution();

        Point[] path = s.FindLongestPath(arr, out int length);

        AssertPathIsValid(arr, path, length);
    }

    [Theory]
    [MemberData(nameof(ValidPathArrays))]
    public void OutLengthEqualsPathLength(int[,] arr)
    {
        var s = new LongestPathSolution();

        Point[] path = s.FindLongestPath(arr, out int length);

        Assert.Equal(path.Length, length);
    }

    [Theory]
    [MemberData(nameof(ValidPathArrays))]
    public void ReturnedPathIsActuallyLongest(int[,] arr)
    {
        // Сравниваем длину найденного пути с длиной, посчитанной независимым
        // эталонным DFS+мемоизация (запасной подсчёт максимума).
        var s = new LongestPathSolution();

        s.FindLongestPath(arr, out int length);
        int expected = ReferenceMaxLength(arr);

        Assert.Equal(expected, length);
    }

    // ─── Поведение Point ──────────────────────────────────────────────────────────

    [Fact]
    public void Point_ValidCoordinates_StoresValues()
    {
        var p = new Point(3, 7);

        Assert.Equal(3, p.Row);
        Assert.Equal(7, p.Column);
    }

    [Fact]
    public void Point_NegativeRow_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Point(-1, 0));
    }

    [Fact]
    public void Point_NegativeColumn_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Point(0, -1));
    }

    [Fact]
    public void Point_BothNegative_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Point(-5, -5));
    }

    [Fact]
    public void Point_Zeros_DoesNotThrow()
    {
        var p = new Point(0, 0);

        Assert.Equal(0, p.Row);
        Assert.Equal(0, p.Column);
    }

    // ─── Run() ─────────────────────────────────────────────────────────────────────

    [Fact]
    public void Run_PrintsPathLengthSix()
    {
        string output = CaptureRun();

        Assert.Contains("Длина пути: 6", output);
    }

    [Fact]
    public void Run_PrintsExpectedValueSequence()
    {
        string output = CaptureRun();

        Assert.Contains("1-3-4-7-8-9", output);
    }

    [Fact]
    public void Run_PrintsExpectedCellSequence()
    {
        string output = CaptureRun();

        Assert.Contains("(1,2) — (1,1) — (2,1) — (2,2) — (2,3) — (1,3)", output);
    }

    [Fact]
    public void Run_DoesNotThrow()
    {
        var ex = Record.Exception(() => CaptureRun());
        Assert.Null(ex);
    }

    // ─── Вспомогательные методы ───────────────────────────────────────────────────

    private static int[] ToValues(int[,] arr, Point[] path)
    {
        int[] values = new int[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            values[i] = arr[path[i].Row, path[i].Column];
        }
        return values;
    }

    private static void AssertPathIsValid(int[,] arr, Point[] path, int length)
    {
        Assert.Equal(path.Length, length);

        if (length == 0)
        {
            Assert.Empty(path);
            return;
        }

        int rows = arr.GetLength(0);
        int cols = arr.GetLength(1);

        var seen = new HashSet<(int, int)>();
        for (int i = 0; i < path.Length; i++)
        {
            Point p = path[i];
            Assert.InRange(p.Row, 0, rows - 1);
            Assert.InRange(p.Column, 0, cols - 1);
            Assert.True(seen.Add((p.Row, p.Column)),
                $"Точка ({p.Row},{p.Column}) встречается повторно в пути");
        }

        for (int i = 1; i < path.Length; i++)
        {
            Point prev = path[i - 1];
            Point cur = path[i];

            int dr = Math.Abs(prev.Row - cur.Row);
            int dc = Math.Abs(prev.Column - cur.Column);
            Assert.True(dr + dc == 1,
                $"Точки ({prev.Row},{prev.Column}) и ({cur.Row},{cur.Column}) " +
                "не являются соседями по горизонтали/вертикали");

            int prevValue = arr[prev.Row, prev.Column];
            int curValue = arr[cur.Row, cur.Column];
            Assert.True(prevValue < curValue,
                $"Значения в пути не строго возрастают: {prevValue} → {curValue}");
        }
    }

    private static int ReferenceMaxLength(int[,] arr)
    {
        int rows = arr.GetLength(0);
        int cols = arr.GetLength(1);
        if (rows == 0 || cols == 0) return 0;

        int[,] memo = new int[rows, cols];
        int max = 0;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int v = ReferenceLongestFrom(arr, r, c, memo);
                if (v > max) max = v;
            }
        }
        return max;
    }

    private static int ReferenceLongestFrom(int[,] arr, int r, int c, int[,] memo)
    {
        if (memo[r, c] != 0) return memo[r, c];

        int rows = arr.GetLength(0);
        int cols = arr.GetLength(1);
        int best = 0;

        int[] dr = { -1, 0, 0, 1 };
        int[] dc = { 0, -1, 1, 0 };
        for (int k = 0; k < 4; k++)
        {
            int nr = r + dr[k];
            int nc = c + dc[k];
            if (nr < 0 || nr >= rows || nc < 0 || nc >= cols) continue;
            if (arr[nr, nc] <= arr[r, c]) continue;

            int v = ReferenceLongestFrom(arr, nr, nc, memo);
            if (v > best) best = v;
        }

        memo[r, c] = best + 1;
        return best + 1;
    }

    private static string CaptureRun()
    {
        var sw = new StringWriter();
        var oldOut = Console.Out;
        Console.SetOut(sw);
        try
        {
            new LongestPathSolution().Run();
        }
        finally
        {
            Console.SetOut(oldOut);
        }

        var raw = sw.ToString().Replace("\r\n", "\n");
        return Regex.Replace(raw, "\u001b\\[[0-9;]*m", "");
    }
}
