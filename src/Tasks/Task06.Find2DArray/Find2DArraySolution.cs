using Tasks.Common;

namespace Task06.Find2DArray;

[Task(6, "Поиск в 2D-массиве")]
public class Find2DArraySolution : IFind2DArraySolution
{
    public void Run()
    {
        Console.WriteLine("Введите размеры массива (строки столбцы):");
        var dims = Console.ReadLine()!.Split();
        int rows = int.Parse(dims[0]);
        int cols = int.Parse(dims[1]);

        var array = new int[rows, cols];
        Console.WriteLine("Введите элементы (отсортированные по неубыванию построчно):");
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                array[i, j] = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Введите искомое значение:");
        int value = int.Parse(Console.ReadLine()!);

        if (FindNumber(array, value, out int[] idx))
            Console.WriteLine($"Найдено: [{idx[0]}, {idx[1]}]");
        else
            Console.WriteLine("Не найдено.");
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
