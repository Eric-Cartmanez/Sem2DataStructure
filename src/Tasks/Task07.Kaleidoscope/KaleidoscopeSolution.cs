using Tasks.Common;

namespace Task07.Kaleidoscope;

[Task(7, "Калейдоскоп")]
public class KaleidoscopeSolution : IKaleidoscopeSolution
{
    // Индексы цветов из ConsoleColor, расположенные по «гармоничному» кругу
    private static readonly int[] Colors = { 0, 8, 7, 15, 14, 6, 12, 4, 5, 13, 11, 3, 9, 1, 2, 10 };
    private static readonly int[] PossibleShift = { -2, -1, 1, 2 };

    public void Run()
    {
        Console.WriteLine("Введите размер калейдоскопа (половина стороны, от 3 до 20):");
        if (int.TryParse(Console.ReadLine(), out int halfSize) && halfSize >= 3 && halfSize <= 20)
        {
            var matrix = GenerateKaleidoscope(halfSize);
            PrintKaleidoscope(matrix);
        }
        else
        {
            Console.WriteLine("Некорректный ввод. Введите число от 3 до 20 включительно.");
        }
    }

    // Генерируем цвета только для верхнего треугольника левого верхнего квадранта,
    // затем заполняем остальные 7 частей симметрично.
    public int[,] GenerateKaleidoscope(int halfSize)
    {
        int full = 2 * halfSize;
        var matrix = new int[full, full];

        for (int row = 0; row < halfSize; row++)
        {
            int colorIndex = Random.Shared.Next(Colors.Length);

            for (int col = row; col < halfSize; col++)
            {
                int color = Colors[colorIndex];

                // Левый верхний квадрант (треугольник + отражение по диагонали)
                matrix[row, col] = color;
                matrix[col, row] = color;

                // Правый верхний квадрант
                matrix[row, full - 1 - col] = color;
                matrix[col, full - 1 - row] = color;

                // Правый нижний квадрант
                matrix[full - 1 - row, full - 1 - col] = color;
                matrix[full - 1 - col, full - 1 - row] = color;

                // Левый нижний квадрант
                matrix[full - 1 - col, row] = color;
                matrix[full - 1 - row, col] = color;

                int shift = PossibleShift[Random.Shared.Next(PossibleShift.Length)];
                colorIndex = (colorIndex + shift + Colors.Length) % Colors.Length;
            }
        }

        return matrix;
    }

    public void PrintKaleidoscope(int[,] matrix)
    {
        int size = matrix.GetLength(0);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Console.BackgroundColor = (ConsoleColor)matrix[i, j];
                Console.Write("  ");
            }
            Console.ResetColor();
            Console.WriteLine();
        }
        Console.ResetColor();
    }
}
