using Tasks.Common;

namespace Task06.Find2DArray;

public interface IFind2DArraySolution : ISolution
{
    /// <summary>
    /// Ищет значение в 2D-массиве, отсортированном по строкам.
    /// </summary>
    /// <param name="array">Двумерный массив, элементы отсортированы по неубыванию построчно.</param>
    /// <param name="value">Искомое значение.</param>
    /// <param name="indexes">Индексы найденного элемента [row, col], либо [-1, -1].</param>
    /// <returns>true, если элемент найден.</returns>
    bool FindNumber(int[,] array, int value, out int[] indexes);
}
