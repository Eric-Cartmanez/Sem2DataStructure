using Task07.Kaleidoscope;

namespace Tasks.Tests;

public class Task07KaleidoscopeTests
{
    private static readonly IKaleidoscopeSolution[] Solutions =
    [
        new KaleidoscopeSolution()
    ];

    public static IEnumerable<object[]> SolutionsWithSize()
    {
        foreach (var s in Solutions)
            foreach (int half in new[] { 3, 5, 10, 20 })
                yield return [s, half];
    }

    public static IEnumerable<object[]> GetSolutions()
    {
        foreach (var s in Solutions) yield return [s];
    }

    // ─── Размер матрицы ───────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(SolutionsWithSize))]
    public void MatrixSize_Is2N(IKaleidoscopeSolution s, int halfSize)
    {
        int[,] m = s.GenerateKaleidoscope(halfSize);
        Assert.Equal(2 * halfSize, m.GetLength(0));
        Assert.Equal(2 * halfSize, m.GetLength(1));
    }

    // ─── Допустимые значения цветов ───────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ColorValues_InValidRange(IKaleidoscopeSolution s)
    {
        int[,] m = s.GenerateKaleidoscope(5);
        int size = m.GetLength(0);
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                Assert.InRange(m[i, j], 0, 15);
    }

    // ─── Симметрия по горизонтали ─────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void HorizontalSymmetry(IKaleidoscopeSolution s)
    {
        int[,] m = s.GenerateKaleidoscope(5);
        int size = m.GetLength(0);
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                Assert.Equal(m[i, j], m[size - 1 - i, j]);
    }

    // ─── Симметрия по вертикали ───────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void VerticalSymmetry(IKaleidoscopeSolution s)
    {
        int[,] m = s.GenerateKaleidoscope(5);
        int size = m.GetLength(0);
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                Assert.Equal(m[i, j], m[i, size - 1 - j]);
    }

    // ─── Симметрия по главной диагонали ──────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void DiagonalSymmetry(IKaleidoscopeSolution s)
    {
        int[,] m = s.GenerateKaleidoscope(5);
        int size = m.GetLength(0);
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                Assert.Equal(m[i, j], m[j, i]);
    }
}
