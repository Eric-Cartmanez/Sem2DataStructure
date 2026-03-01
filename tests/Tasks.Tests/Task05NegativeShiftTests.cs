using Task05.NegativeShift;

namespace Tasks.Tests;

public class Task05NegativeShiftTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new NegativeShiftSolution()];
    }

    private static void Run(INegativeShiftSolution s, int[] arr) => s.ShiftNegativeLeft(arr);

    private static bool IsPartitioned(int[] arr)
    {
        bool seenNonNeg = false;
        foreach (int x in arr)
        {
            if (x >= 0) seenNonNeg = true;
            else if (seenNonNeg) return false;
        }
        return true;
    }

    private static bool SameElements(int[] a, int[] b)
        => a.Length == b.Length && a.OrderBy(x => x).SequenceEqual(b.OrderBy(x => x));

    // ─── Основной инвариант: все отрицательные перед неотрицательными ─────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Result_IsPartitioned(INegativeShiftSolution s)
    {
        int[] arr = [-3, 1, -2, 4, -1, 5];
        Run(s, arr);
        Assert.True(IsPartitioned(arr));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Result_PreservesElements(INegativeShiftSolution s)
    {
        int[] original = [-3, 1, -2, 4, -1, 5];
        int[] arr = (int[])original.Clone();
        Run(s, arr);
        Assert.True(SameElements(original, arr));
    }

    // ─── Граничные случаи ─────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void EmptyArray_NoException(INegativeShiftSolution s)
        => Run(s, []);

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void SingleElement_Unchanged(INegativeShiftSolution s)
    {
        int[] arr = [-7];
        Run(s, arr);
        Assert.Equal([-7], arr);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void AllNegatives_Partitioned(INegativeShiftSolution s)
    {
        int[] arr = [-1, -2, -3];
        Run(s, arr);
        Assert.True(IsPartitioned(arr));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void AllPositives_Partitioned(INegativeShiftSolution s)
    {
        int[] arr = [1, 2, 3];
        Run(s, arr);
        Assert.True(IsPartitioned(arr));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void AllZeros_Partitioned(INegativeShiftSolution s)
    {
        int[] arr = [0, 0, 0];
        Run(s, arr);
        Assert.True(IsPartitioned(arr));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void ZeroTreatedAsNonNegative(INegativeShiftSolution s)
    {
        int[] arr = [0, -1];
        Run(s, arr);
        Assert.True(arr[0] < 0);
        Assert.True(arr[1] >= 0);
    }

    // ─── Чередующийся массив ──────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Alternating_Partitioned(INegativeShiftSolution s)
    {
        int[] arr = [-1, 2, -3, 4, -5, 6];
        Run(s, arr);
        Assert.True(IsPartitioned(arr));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Alternating_PreservesElements(INegativeShiftSolution s)
    {
        int[] original = [-1, 2, -3, 4, -5, 6];
        int[] arr = (int[])original.Clone();
        Run(s, arr);
        Assert.True(SameElements(original, arr));
    }

    // ─── int.MinValue / int.MaxValue ──────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void IntMinValue_TreatedAsNegative(INegativeShiftSolution s)
    {
        int[] arr = [int.MinValue, 1];
        Run(s, arr);
        Assert.True(IsPartitioned(arr));
        Assert.Equal(int.MinValue, arr[0]);
    }

    // ─── Большой случайный массив ─────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void LargeArray_Partitioned_ElementsPreserved(INegativeShiftSolution s)
    {
        var rng = new Random(42);
        int[] arr = Enumerable.Range(0, 1000).Select(_ => rng.Next(-500, 500)).ToArray();
        int[] original = (int[])arr.Clone();
        Run(s, arr);
        Assert.True(IsPartitioned(arr));
        Assert.True(SameElements(original, arr));
    }
}
