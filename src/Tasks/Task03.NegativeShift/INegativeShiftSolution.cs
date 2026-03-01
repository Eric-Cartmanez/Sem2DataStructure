using Tasks.Common;

namespace Task03.NegativeShift;

public interface INegativeShiftSolution : ISolution
{
    /// <summary>
    /// Переставляет элементы массива in-place: сначала все отрицательные, потом неотрицательные.
    /// Порядок внутри групп не гарантируется.
    /// </summary>
    void ShiftNegativeLeft(int[] array);
}
