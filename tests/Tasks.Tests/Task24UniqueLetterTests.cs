using Task24.UniqueLetter;
using Xunit;

namespace Tasks.Tests;

public class Task24UniqueLetterTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new UniqueLetterSolution()];
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindUniqueLetter_FromTaskDescription_ReturnsCorrectLetter(IUniqueLetterSolution solution)
    {
        // Arrange
        string input = "Если вы не видели никогда Ивлета, советую вам отыскать его в Горячей долине.";
        char expected = 'у';

        // Act
        char actual = solution.FindUniqueLetter(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindUniqueLetter_NoUniqueLetters_ReturnsDot(IUniqueLetterSolution solution)
    {
        // Arrange
        string input = "АабБвВгГ";
        char expected = '.';

        // Act
        char actual = solution.FindUniqueLetter(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindUniqueLetter_EmptyString_ReturnsDot(IUniqueLetterSolution solution)
    {
        // Arrange
        string input = "";
        char expected = '.';

        // Act
        char actual = solution.FindUniqueLetter(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindUniqueLetter_OnlyNonLetters_ReturnsDot(IUniqueLetterSolution solution)
    {
        // Arrange
        string input = "1234567890 !@#$%^&*()";
        char expected = '.';

        // Act
        char actual = solution.FindUniqueLetter(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindUniqueLetter_PreservesCase(IUniqueLetterSolution solution)
    {
        // Arrange
        string input = "aBbaC";
        char expected = 'C';

        // Act
        char actual = solution.FindUniqueLetter(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void FindUniqueLetter_AllUniqueLetters_ReturnsFirst(IUniqueLetterSolution solution)
    {
        // Arrange
        string input = "AbCdEf";
        char expected = 'A';

        // Act
        char actual = solution.FindUniqueLetter(input);

        // Assert
        Assert.Equal(expected, actual);
    }
}
