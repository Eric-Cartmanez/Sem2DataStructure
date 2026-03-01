using Tasks.Common;

namespace Task03.BestTeacher;

[Task(3, "Лучший преподаватель")]
public class BestTeacherSolution : IBestTeacherSolution
{
    public void Run()
    {
        Console.WriteLine("Введите количество преподавателей (N):");
        int n = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Введите количество студентов (M, не менее 3):");
        int m = int.Parse(Console.ReadLine()!);

        var marks = new double[n, m];
        Console.WriteLine("Введите оценки (по строкам, через Enter):");
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
                marks[i, j] = double.Parse(Console.ReadLine()!);

        if (FindBestTeacher(marks, out int index, out double average))
            Console.WriteLine($"Лучший преподаватель: индекс {index}, среднее {average:0.##}");
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
