using Tasks.Common;

namespace Task06.Find2DArray;

[Task(6, "Поиск в 2D-массиве")]
public class Find2DArraySolution : IFind2DArraySolution
{
    public void Run()
    {
        int[,] array = {
            { 2,  6,  7,  9,  9, 14},
            {18, 20, 26, 26, 29, 40},
            {44, 47, 50, 51, 55, 62}
        };

        int rows = array.GetLength(0);
        int cols = array.GetLength(1);
        Console.WriteLine($"Массив {rows}×{cols} (отсортирован по строкам):");
        for (int i = 0; i < rows; i++)
        {
            var row = Enumerable.Range(0, cols).Select(j => array[i, j]);
            Console.WriteLine("  [ " + string.Join(", ", row.Select(v => $"{v,2}")) + " ]");
        }
        Console.WriteLine();
        Console.WriteLine("Задача: бинарный поиск в 2D-массиве");
        Console.WriteLine();

        foreach (int value in new[] { 2, 18, 29, 62, 42, 1 })
        {
            if (FindNumber(array, value, out int[] idx))
                Console.WriteLine($"  Ищем {value,3} → найдено по индексам [{idx[0]}, {idx[1]}]");
            else
                Console.WriteLine($"  Ищем {value,3} → не найдено");
        }
    }

    // Представляем 2D-массив как плоский 1D и применяем бинарный поиск.
    // Индекс mid → (mid / cols, mid % cols).
    // O(log(N×M)) по времени, O(1) по памяти.
    public bool FindNumber(int[,] array, int value, out int[] indexes)
    {
        int cols = array.GetLength(1);
        int left = 0;
        int right = array.GetLength(0) * cols - 1;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            int element = array[mid / cols, mid % cols];

            if (element == value)
            {
                indexes = [mid / cols, mid % cols];
                return true;
            }

            if (element < value) left = mid + 1;
            else right = mid - 1;
        }

        indexes = [-1, -1];
        return false;
    }
}
