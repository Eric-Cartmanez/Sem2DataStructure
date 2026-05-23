using Tasks.Common;
using System.Text;

namespace Task28.NextBigNumber;

[Task(28, "Следующее большее число")]
public class NextBigNumberSolution : INextBigNumberSolution
{
    public void Run()
    {
        string[] numbers = { "12", "513", "2184", "222", "931" };

        foreach (string number in numbers)
        {
            int result = NextBigNumber(number);
            Console.WriteLine($"{number} --> {result}");
        }
    }

    // Сложность: время O(n), память O(n), где n = number.Length.
    public int NextBigNumber(string number)
    {
        if (number.Length < 2)
            return -1;

        // 1) Ищем pivot: самую правую позицию, где число ещё «можно увеличить».
        // После цикла number[i-1] < number[i], а хвост [i..n-1] невозрастает.
        int i = number.Length - 1;
        while (i > 0 && number[i - 1] >= number[i])
            i--;

        // Весь хвост невозрастает — число уже максимальная перестановка цифр.
        if (i == 0)
            return -1;

        // 2) В невозрастающем хвосте ищем самую правую цифру, большую pivot.
        // Из-за порядка хвоста она и будет наименьшей подходящей.
        char pivot = number[i - 1];
        int swapIndex = i;
        for (int j = i + 1; j < number.Length; j++)
        {
            if (number[j] > pivot)
                swapIndex = j;
        }

        // 3) Обмен через StringBuilder — string неизменяемый.
        StringBuilder newNumber = new StringBuilder(number);
        newNumber[i - 1] = number[swapIndex];
        newNumber[swapIndex] = pivot;

        // 4) Хвост по-прежнему невозрастает; переворот делает его минимальным.
        int left = i;
        int right = number.Length - 1;
        while (left < right)
        {
            (newNumber[left], newNumber[right]) = (newNumber[right], newNumber[left]);
            left++;
            right--;
        }

        return int.Parse(newNumber.ToString());
    }
}
