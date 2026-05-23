using Task29.BiggestNumber;

namespace Tasks.Tests;

public class Task29BiggestNumberTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new BiggestNumberSolution()];
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BiggestNumber_ExampleFromTask_ReturnsCorrectResult(IBiggestNumberSolution solution)
    {
        string input = "99DDAB7++0088UUQQ450 Z9Z";
        double expected = 450;

        double actual = solution.BiggestNumber(input);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BiggestNumber_NoNumbersInString_ReturnsZero(IBiggestNumberSolution solution)
    {
        string input = "abcDEF++--";
        double expected = 0;

        double actual = solution.BiggestNumber(input);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BiggestNumber_EmptyString_ReturnsZero(IBiggestNumberSolution solution)
    {
        string input = "";
        double expected = 0;

        double actual = solution.BiggestNumber(input);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BiggestNumber_FractionalNumbers_ReturnsCorrectMax(IBiggestNumberSolution solution)
    {
        string input = "12,3 text 12,34 text 5,5";
        double expected = 12.34;

        // Важно задать правильную культуру для тестирования вещественных чисел с запятой,
        // так как double.Parse зависит от текущей культуры.
        var originalCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
        try
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
            double actual = solution.BiggestNumber(input);
            Assert.Equal(expected, actual);
        }
        finally
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = originalCulture;
        }
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BiggestNumber_MultipleFractionalNumbers_HandledCorrectly(IBiggestNumberSolution solution)
    {
        string input = "100,5 200,1 50,9";
        double expected = 200.1;

        var originalCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
        try
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
            double actual = solution.BiggestNumber(input);
            Assert.Equal(expected, actual);
        }
        finally
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = originalCulture;
        }
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BiggestNumber_NumberAtTheEndOfString_IsProcessed(IBiggestNumberSolution solution)
    {
        string input = "abc999";
        double expected = 999;

        double actual = solution.BiggestNumber(input);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void BiggestNumber_MultipleCommasInSequence_RightmostCommaIsDecimalPoint(IBiggestNumberSolution solution)
    {
        // "98,456,8" разбирается как 98 (целое) и 456,8 (вещественное),
        // потому что самая правая запятая, окружённая цифрами, выступает десятичным разделителем.
        string input = "zz98,456,8mmmm123";
        double expected = 456.8;

        var originalCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
        try
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
            double actual = solution.BiggestNumber(input);
            Assert.Equal(expected, actual);
        }
        finally
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = originalCulture;
        }
    }
}
