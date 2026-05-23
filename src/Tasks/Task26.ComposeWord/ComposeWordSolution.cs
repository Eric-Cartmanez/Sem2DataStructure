using Tasks.Common;

namespace Task26.ComposeWord;

[Task(26, "Составить слово")]
public class ComposeWordSolution : IComposeWordSolution
{
    public void Run()
    {
        Console.WriteLine("Введите текст:");
        string text = Console.ReadLine()!;
        Console.WriteLine("Введите слово:");
        string word = Console.ReadLine()!;
        bool result = CanComposeWord(text, word);
        Console.WriteLine(result);
    }

    public bool CanComposeWord(string text, string word)
    {
        Dictionary<char, int> letterCounts = new Dictionary<char, int>();
        foreach (char letter in text)
        {
            char upperLetter = char.ToUpper(letter);
            if (letterCounts.ContainsKey(upperLetter))
            {
                letterCounts[upperLetter]++;
            }
            else
            {
                letterCounts[upperLetter] = 1;
            }
        }

        foreach (char letter in word)
        {
            char upperLetter = char.ToUpper(letter);
            if (letterCounts.ContainsKey(upperLetter))
            {
                letterCounts[upperLetter]--;
            }
            else
            {
                return false;
            }
        }

        foreach (int count in letterCounts.Values)
        {
            if (count < 0) return false;
        }

        return true;
    }
}
