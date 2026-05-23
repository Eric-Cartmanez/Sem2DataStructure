using Task26.ComposeWord;

namespace Tasks.Tests;

public class Task26ComposeWordTests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new ComposeWordSolution()];
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void CanComposeWord_Example_Salo_ReturnsTrue(IComposeWordSolution solution)
    {
        // Arrange
        string text = "колбаса";
        string word = "сало";

        // Act
        bool actual = solution.CanComposeWord(text, word);

        // Assert
        Assert.True(actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void CanComposeWord_Example_Oblaka_ReturnsTrue(IComposeWordSolution solution)
    {
        // Arrange
        string text = "колбаса";
        string word = "облака";

        // Act
        bool actual = solution.CanComposeWord(text, word);

        // Assert
        Assert.True(actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void CanComposeWord_Example_Barak_ReturnsFalse(IComposeWordSolution solution)
    {
        // Arrange
        string text = "колбаса";
        string word = "барак";

        // Act
        bool actual = solution.CanComposeWord(text, word);

        // Assert
        Assert.False(actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void CanComposeWord_Example_Lasso_ReturnsFalse(IComposeWordSolution solution)
    {
        // Arrange
        string text = "колбаса";
        string word = "лассо";

        // Act
        bool actual = solution.CanComposeWord(text, word);

        // Assert
        Assert.False(actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void CanComposeWord_CaseInsensitive_UpperWord_ReturnsTrue(IComposeWordSolution solution)
    {
        // Arrange
        string text = "колбаса";
        string word = "САЛО";

        // Act
        bool actual = solution.CanComposeWord(text, word);

        // Assert
        Assert.True(actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void CanComposeWord_CaseInsensitive_MixedCase_ReturnsTrue(IComposeWordSolution solution)
    {
        // Arrange
        string text = "КоЛбАсА";
        string word = "СаЛо";

        // Act
        bool actual = solution.CanComposeWord(text, word);

        // Assert
        Assert.True(actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void CanComposeWord_EmptyWord_ReturnsTrue(IComposeWordSolution solution)
    {
        // Arrange
        string text = "колбаса";
        string word = "";

        // Act
        bool actual = solution.CanComposeWord(text, word);

        // Assert
        Assert.True(actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void CanComposeWord_EmptyTextAndWord_ReturnsTrue(IComposeWordSolution solution)
    {
        // Arrange
        string text = "";
        string word = "";

        // Act
        bool actual = solution.CanComposeWord(text, word);

        // Assert
        Assert.True(actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void CanComposeWord_EmptyText_NonEmptyWord_ReturnsFalse(IComposeWordSolution solution)
    {
        // Arrange
        string text = "";
        string word = "abc";

        // Act
        bool actual = solution.CanComposeWord(text, word);

        // Assert
        Assert.False(actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void CanComposeWord_SameStrings_ReturnsTrue(IComposeWordSolution solution)
    {
        // Arrange
        string text = "abc";
        string word = "abc";

        // Act
        bool actual = solution.CanComposeWord(text, word);

        // Assert
        Assert.True(actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void CanComposeWord_WordHasExtraRepeat_ReturnsFalse(IComposeWordSolution solution)
    {
        // Arrange
        string text = "abc";
        string word = "abcc";

        // Act
        bool actual = solution.CanComposeWord(text, word);

        // Assert
        Assert.False(actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void CanComposeWord_WordHasMissingChar_ReturnsFalse(IComposeWordSolution solution)
    {
        // Arrange
        string text = "abc";
        string word = "d";

        // Act
        bool actual = solution.CanComposeWord(text, word);

        // Assert
        Assert.False(actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void CanComposeWord_RepeatedLettersWithinBudget_ReturnsTrue(IComposeWordSolution solution)
    {
        // Arrange
        string text = "aabbcc";
        string word = "abc";

        // Act
        bool actual = solution.CanComposeWord(text, word);

        // Assert
        Assert.True(actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void CanComposeWord_RepeatedLettersExceedingBudget_ReturnsFalse(IComposeWordSolution solution)
    {
        // Arrange
        string text = "abc";
        string word = "aa";

        // Act
        bool actual = solution.CanComposeWord(text, word);

        // Assert
        Assert.False(actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void CanComposeWord_TextLongerThanWord_AllLettersAvailable_ReturnsTrue(IComposeWordSolution solution)
    {
        // Arrange
        string text = "программирование";
        string word = "рама";

        // Act
        bool actual = solution.CanComposeWord(text, word);

        // Assert
        Assert.True(actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void CanComposeWord_SpacesAreCharacters_RespectsCount(IComposeWordSolution solution)
    {
        // Arrange
        string text = "a b c";
        string word = "ab";

        // Act
        bool actual = solution.CanComposeWord(text, word);

        // Assert
        Assert.True(actual);
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void CanComposeWord_RequiredSpaceMissing_ReturnsFalse(IComposeWordSolution solution)
    {
        // Arrange
        string text = "abc";
        string word = "a b";

        // Act
        bool actual = solution.CanComposeWord(text, word);

        // Assert
        Assert.False(actual);
    }
}
