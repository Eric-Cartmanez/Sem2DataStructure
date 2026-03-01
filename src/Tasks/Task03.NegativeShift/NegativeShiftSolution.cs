using Tasks.Common;

namespace Task03.NegativeShift;

[Task(3, "Перестановка отрицательных")]
public class NegativeShiftSolution : INegativeShiftSolution
{
    public void Run()
    {
        // Пример 1: смешанный массив
        int[] a1 = { -3, 1, -2, 4, -1, 5 };
        Console.WriteLine("Пример 1:");
        Console.WriteLine("  Вход:  " + string.Join(", ", a1));
        Console.WriteLine("  Задача: переставить отрицательные элементы в начало");
        ShiftNegativeLeft(a1);
        Console.WriteLine("  Выход: " + string.Join(", ", a1));

        Console.WriteLine();

        // Пример 2: чередующийся массив
        int[] a2 = { -1, 2, -3, 4, -5, 6, -7, 8 };
        Console.WriteLine("Пример 2:");
        Console.WriteLine("  Вход:  " + string.Join(", ", a2));
        Console.WriteLine("  Задача: переставить отрицательные элементы в начало");
        ShiftNegativeLeft(a2);
        Console.WriteLine("  Выход: " + string.Join(", ", a2));

        Console.WriteLine();

        // Пример 3: уже отсортировано
        int[] a3 = { -5, -3, -1, 0, 2, 4 };
        Console.WriteLine("Пример 3 (уже разбито):");
        Console.WriteLine("  Вход:  " + string.Join(", ", a3));
        ShiftNegativeLeft(a3);
        Console.WriteLine("  Выход: " + string.Join(", ", a3));
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
