using Tasks.Common;

namespace Task26.ComposeWord;

public interface IComposeWordSolution : ISolution
{
    public bool CanComposeWord(string text, string word);
}
