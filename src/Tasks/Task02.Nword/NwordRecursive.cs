using Tasks.Common;

namespace Task02.Nword;

[Task(2, "N-буквенные слова", "Рекурсивный")]
public class NwordRecursive : INwordSolution
{
    private const string Alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
    private const string ErrorMessage = "Некорректный ввод. Введите число от 1 до 33";

    public void Run()
    {
        Console.WriteLine("Введите длину слова (N):");
        if (!TryReadValue(out int n)) { Console.WriteLine(ErrorMessage); return; }

        Console.WriteLine("Введите размер алфавита (M, не более 33):");
        if (!TryReadValue(out int m)) { Console.WriteLine(ErrorMessage); return; }

        foreach (var word in GenerateWords(n, Alphabet[..m]))
            Console.WriteLine(word);
    }

    public IEnumerable<string> GenerateWords(int wordLength, string alphabet)
    {
        return Generate(wordLength, alphabet, "");
    }

    private static IEnumerable<string> Generate(int remaining, string alphabet, string current)
    {
        if (remaining == 0)
        {
            yield return current;
            yield break;
        }
        foreach (char c in alphabet)
            foreach (var word in Generate(remaining - 1, alphabet, current + c))
                yield return word;
    }

    private static bool TryReadValue(out int value)
    {
        if (int.TryParse(Console.ReadLine(), out value) && value > 0 && value <= 33)
            return true;
        value = -1;
        return false;
    }
}
