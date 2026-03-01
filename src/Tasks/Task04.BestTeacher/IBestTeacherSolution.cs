using Tasks.Common;

namespace Task04.BestTeacher;

public interface IBestTeacherSolution : ISolution
{
    /// <summary>
    /// Находит лучшего преподавателя: строку массива с максимальным усечённым средним
    /// (без учёта одного максимума и одного минимума в каждой строке).
    /// </summary>
    /// <param name="marks">Массив оценок [преподаватели, студенты]. Минимум 3 студента.</param>
    /// <param name="index">Индекс лучшего преподавателя. -1 при некорректных данных.</param>
    /// <param name="average">Усечённое среднее лучшего преподавателя.</param>
    /// <returns>true, если данные корректны; иначе false.</returns>
    bool FindBestTeacher(double[,] marks, out int index, out double average);
}
