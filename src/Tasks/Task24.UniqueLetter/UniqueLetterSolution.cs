using Tasks.Common;

namespace Task24.UniqueLetter;

[Task(24, "Уникальная буква")]
public class UniqueLetterSolution : IUniqueLetterSolution
{
    public void Run()
    {
        string input = Console.ReadLine()!;
        char result = FindUniqueLetter(input);
        Console.WriteLine(result);
    }

    public char FindUniqueLetter(string str)
    {
        Dictionary<char, int> letterCounts = new Dictionary<char, int>();
        foreach (char letter in str)
        {
            if (!char.IsLetter(letter)) continue;
            
            char upperLetter = char.ToUpper(letter);
            char lowerLetter = char.ToLower(letter);
            if (letterCounts.ContainsKey(upperLetter))
            {
                letterCounts[upperLetter]++;
            }
            else if (letterCounts.ContainsKey(lowerLetter))
            {
                letterCounts[lowerLetter]++;
            }
            else
            {
                letterCounts[letter] = 1;
            }
        }

        foreach (char letter in letterCounts.Keys)
        {
            if (letterCounts[letter] == 1)
                return letter;
        }
        
        return '.';
    }
}
