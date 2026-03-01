using Tasks.Common;

namespace Task05.NegativeShift;

[Task(5, "Перестановка отрицательных")]
public class NegativeShiftSolution : INegativeShiftSolution
{
    public void Run()
    {
        Console.WriteLine("Введите размер массива:");
        int size = int.Parse(Console.ReadLine()!);
        var array = new int[size];
        for (int i = 0; i < size; i++)
            array[i] = int.Parse(Console.ReadLine()!);

        ShiftNegativeLeft(array);

        Console.WriteLine("Результат:");
        Console.WriteLine(string.Join(" ", array));
    }

    // O(n) по времени, O(1) по памяти.
    // Два указателя: левый ищет место для отрицательного, правый перебирает элементы.
    public void ShiftNegativeLeft(int[] array)
    {
        int left = 0;
        for (int right = 0; right < array.Length; right++)
        {
            if (array[right] < 0)
            {
                if (left != right)
                    (array[left], array[right]) = (array[right], array[left]);
                left++;
            }
        }
    }
}
