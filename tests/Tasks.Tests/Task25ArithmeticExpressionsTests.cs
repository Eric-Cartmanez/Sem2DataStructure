using Task25.ArithmeticExpressions;

namespace Tasks.Tests;

public class Task25ArithmeticExpressionsTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new ArithmeticExpressionsSolution()];
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Example1_TaskMd(ArithmeticExpressionsSolution s)
    {
        Assert.Equal(9.9, s.CalculateExpression("9-(2-11)/10"), 6);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Example2_TaskMd(ArithmeticExpressionsSolution s)
    {
        Assert.Equal(-17, s.CalculateExpression("(5+10,5-24)*2"), 6);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Priority_MulOverAdd(ArithmeticExpressionsSolution s)
    {
        Assert.Equal(14, s.CalculateExpression("2+3*4"), 6);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Priority_MulFirst(ArithmeticExpressionsSolution s)
    {
        Assert.Equal(10, s.CalculateExpression("2*3+4"), 6);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Brackets_OverridePriority(ArithmeticExpressionsSolution s)
    {
        Assert.Equal(20, s.CalculateExpression("(2+3)*4"), 6);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void DivThenAdd(ArithmeticExpressionsSolution s)
    {
        Assert.Equal(8, s.CalculateExpression("10/2+3"), 6);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void LongChain(ArithmeticExpressionsSolution s)
    {
        Assert.Equal(10, s.CalculateExpression("1+2+3+4"), 6);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void Whitespace(ArithmeticExpressionsSolution s)
    {
        Assert.Equal(14, s.CalculateExpression(" 2 + 3 * 4 "), 6);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void DecimalComma(ArithmeticExpressionsSolution s)
    {
        Assert.Equal(7.5, s.CalculateExpression("2,5*3"), 6);
    }
}
