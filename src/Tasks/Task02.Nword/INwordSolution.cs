using Tasks.Common;

namespace Task02.Nword;

public interface INwordSolution : ISolution
{
    /// <summary>
    /// Возвращает все N-буквенные слова из первых M букв алфавита.
    /// </summary>
    IEnumerable<string> GenerateWords(int wordLength, string alphabet);
}
