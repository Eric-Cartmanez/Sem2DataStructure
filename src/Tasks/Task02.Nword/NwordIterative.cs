using Tasks.Common;

namespace Task02.Nword;

[Task(2, "N-буквенные слова", "Итеративный")]
public class NwordIterative : INwordSolution
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

    // Каждое число i от 0 до M^N − 1 — это слово в M-ичной записи.
    // i / M^(N−1−j) % M даёт j-ю «цифру» — индекс буквы.
    public IEnumerable<string> GenerateWords(int wordLength, string alphabet)
    {
        int m = alphabet.Length;
        int total = (int)Math.Pow(m, wordLength);
        var chars = new char[wordLength];

        for (int i = 0; i < total; i++)
        {
            for (int j = 0; j < wordLength; j++)
                chars[j] = alphabet[i / (int)Math.Pow(m, wordLength - 1 - j) % m];
            yield return new string(chars);
        }
    }

    private static bool TryReadValue(out int value)
    {
        if (int.TryParse(Console.ReadLine(), out value) && value > 0 && value <= 33)
            return true;
        value = -1;
        return false;
    }
}
