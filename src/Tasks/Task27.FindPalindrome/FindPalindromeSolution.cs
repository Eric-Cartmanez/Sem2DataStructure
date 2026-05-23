using Tasks.Common;

namespace Task27.FindPalindrome;

[Task(27, "Поиск палиндрома")]
public class FindPalindromeSolution : IFindPalindromeSolution
{
    public void Run()
    {
        Console.WriteLine("Введите строку:");
        string str = Console.ReadLine()!;
        string result = FindPalindrome(str);
        Console.WriteLine(result);
    }

    public string FindPalindrome(string str)
    {
        if (str.Length < 2)
            return str;

        string result = str[0].ToString();

        for (int i = 0; i < str.Length; i++)
        {
            int left = i;
            int right = i;
            while (str[left] == str[right])
            {
                string current = str.Substring(left, right - left + 1);
                if (current.Length > result.Length)
                    result = current;
                left--;
                right++;

                if (left < 0 || right >= str.Length)
                    break;
            }
        }


        for (int i = 0; i < str.Length - 1; i++)
        {
            int left = i;
            int right = i + 1;
            while (str[left] == str[right])
            {
                string current = str.Substring(left, right - left + 1);
                if (current.Length > result.Length)
                    result = current;
                left--;
                right++;

                if (left < 0 || right >= str.Length)
                    break;
            }
        }

        return result;
    }
}
