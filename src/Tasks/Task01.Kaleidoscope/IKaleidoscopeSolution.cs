using Tasks.Common;

namespace Task01.Kaleidoscope;

public interface IKaleidoscopeSolution : ISolution
{
    /// <summary>
    /// Генерирует матрицу калейдоскопа (2N × 2N) с 8-кратной симметрией.
    /// Каждая ячейка содержит индекс цвета из набора ConsoleColor.
    /// </summary>
    int[,] GenerateKaleidoscope(int halfSize);

    /// <summary>
    /// Выводит матрицу в консоль, используя цвета фона ConsoleColor.
    /// </summary>
    void PrintKaleidoscope(int[,] matrix);
}
