using Task27.FindPalindrome;

namespace Tasks.Tests;

public class Task27FindPalindromeTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new FindPalindromeSolution()];
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPalindrome_FromTaskDescription_ReturnsLongestPalindrome(IFindPalindromeSolution solution)
    {
        // Arrange
        string input = "АБААБГДГБАБЕГ";
        string expected = "АБГДГБА";

        // Act
        string actual = solution.FindPalindrome(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPalindrome_EmptyString_ReturnsEmpty(IFindPalindromeSolution solution)
    {
        // Arrange
        string input = "";
        string expected = "";

        // Act
        string actual = solution.FindPalindrome(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPalindrome_SingleChar_ReturnsItself(IFindPalindromeSolution solution)
    {
        // Arrange
        string input = "А";
        string expected = "А";

        // Act
        string actual = solution.FindPalindrome(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPalindrome_TwoSameChars_ReturnsWholeString(IFindPalindromeSolution solution)
    {
        // Arrange
        string input = "АА";
        string expected = "АА";

        // Act
        string actual = solution.FindPalindrome(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPalindrome_TwoDifferentChars_ReturnsSingleChar(IFindPalindromeSolution solution)
    {
        // Arrange
        string input = "АБ";

        // Act
        string actual = solution.FindPalindrome(input);

        // Assert
        Assert.Equal("А", actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPalindrome_WholeStringIsOddPalindrome_ReturnsWholeString(IFindPalindromeSolution solution)
    {
        // Arrange
        string input = "ШАЛАШ";
        string expected = "ШАЛАШ";

        // Act
        string actual = solution.FindPalindrome(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPalindrome_WholeStringIsEvenPalindrome_ReturnsWholeString(IFindPalindromeSolution solution)
    {
        // Arrange
        string input = "АББА";
        string expected = "АББА";

        // Act
        string actual = solution.FindPalindrome(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPalindrome_OddPalindromeInside_ReturnsIt(IFindPalindromeSolution solution)
    {
        // Arrange
        string input = "ХАБВБАХ";
        string expected = "ХАБВБАХ";

        // Act
        string actual = solution.FindPalindrome(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPalindrome_EvenPalindromeInside_ReturnsIt(IFindPalindromeSolution solution)
    {
        // Arrange
        string input = "ВАББАГ";
        string expected = "АББА";

        // Act
        string actual = solution.FindPalindrome(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPalindrome_NoPalindromesLongerThanOne_ReturnsFirstChar(IFindPalindromeSolution solution)
    {
        // Arrange
        string input = "АБВГД";

        // Act
        string actual = solution.FindPalindrome(input);

        // Assert
        Assert.Equal("А", actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPalindrome_PalindromeAtStart_ReturnsIt(IFindPalindromeSolution solution)
    {
        // Arrange
        string input = "АБВБАГДЕ";
        string expected = "АБВБА";

        // Act
        string actual = solution.FindPalindrome(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPalindrome_PalindromeAtEnd_ReturnsIt(IFindPalindromeSolution solution)
    {
        // Arrange
        string input = "ГДЕАБВБА";
        string expected = "АБВБА";

        // Act
        string actual = solution.FindPalindrome(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPalindrome_LongerEvenBeatsShorterOdd_ReturnsLongest(IFindPalindromeSolution solution)
    {
        // Arrange
        string input = "АБАВГДДГВЕ";
        string expected = "ВГДДГВ";

        // Act
        string actual = solution.FindPalindrome(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPalindrome_LongerOddBeatsShorterEven_ReturnsLongest(IFindPalindromeSolution solution)
    {
        // Arrange
        string input = "ХАББАЫШАЛАШЪ";
        string expected = "ШАЛАШ";

        // Act
        string actual = solution.FindPalindrome(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindPalindrome_AllSameChars_ReturnsWholeString(IFindPalindromeSolution solution)
    {
        // Arrange
        string input = "ААААА";
        string expected = "ААААА";

        // Act
        string actual = solution.FindPalindrome(input);

        // Assert
        Assert.Equal(expected, actual);
    }
}
