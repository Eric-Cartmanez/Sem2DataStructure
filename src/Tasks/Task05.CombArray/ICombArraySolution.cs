using Tasks.Common;

namespace Task05.CombArray;

public interface ICombArraySolution : ISolution
{
    /// <summary>
    /// Из исходного массива возвращает новый, содержащий только элементы,
    /// которые не нарушают порядок неубывания (один проход, сравнение с соседом).
    /// </summary>
    int[] FilterNonDecreasing(int[] array);
}
