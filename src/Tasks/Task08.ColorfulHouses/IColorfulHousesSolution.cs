using Tasks.Common;

namespace Task08.ColorfulHouses;

public interface IColorfulHousesSolution : ISolution
{
    /// <summary>
    /// Распределяет height*width клеток по colorsCount цветам (round-robin).
    /// Возвращает массив длиной colorsCount, где result[i] — количество клеток i-го цвета.
    /// </summary>
    int[] Solve(int height, int width, int colorsCount);
}
