using Tasks.Common;

namespace Task04.BestTeacher;

[Task(4, "Лучший преподаватель")]
public class BestTeacherSolution : IBestTeacherSolution
{
    public void Run()
    {
        double[,] marks = {
            {3.6, 3.1, 2.8, 1,   4,   3.3, 3.2, 3  },
            {3.5, 3.6, 4.1, 3.9, 3.5, 5,   4,   5  },
            {2.2, 2.7, 3.1, 3,   4.5, 2.2, 3.1, 3.7},
            {4.2, 3.4, 3,   4.3, 4.1, 4.6, 4.4, 4.5},
            {4.7, 4.1, 3.6, 2.1, 2.7, 2,   2.5, 2.7}
        };

        int teachers = marks.GetLength(0);
        int students = marks.GetLength(1);
        Console.WriteLine($"Оценки {teachers} преподавателей от {students} студентов:");
        for (int t = 0; t < teachers; t++)
        {
            var row = Enumerable.Range(0, students).Select(s => marks[t, s]);
            Console.WriteLine($"  Преподаватель {t}: {string.Join(", ", row)}");
        }
        Console.WriteLine();
        Console.WriteLine("Задача: найти преподавателя с максимальным усечённым средним");
        Console.WriteLine("        (без учёта одной наибольшей и одной наименьшей оценки)");
        Console.WriteLine();

        if (FindBestTeacher(marks, out int index, out double average))
            Console.WriteLine($"Лучший преподаватель: индекс {index}, усечённое среднее = {average:0.##}");
        else
            Console.WriteLine("Некорректные данные.");
    }

    // O(N×M) по времени, O(1) по памяти.
    public bool FindBestTeacher(double[,] marks, out int index, out double average)
    {
        average = -1;
        index = -1;

        int teacherCount = marks.GetLength(0);
        int markCount = marks.GetLength(1);

        if (teacherCount == 0 || markCount < 3)
            return false;

        for (int t = 0; t < teacherCount; t++)
        {
            double max = double.MinValue;
            double min = double.MaxValue;
            double sum = 0;

            for (int i = 0; i < markCount; i++)
            {
                double mark = marks[t, i];
                if (mark > max) max = mark;
                if (mark < min) min = mark;
                sum += mark;
            }

            double trimmedAverage = (sum - max - min) / (markCount - 2);
            if (trimmedAverage > average)
            {
                average = trimmedAverage;
                index = t;
            }
        }

        return true;
    }
}
