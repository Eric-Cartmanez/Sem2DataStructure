using Tasks.Common;

namespace Task08.ColorfulHouses;

[Task(8, "Цветные дома")]
public class ColorfulHousesSolution : IColorfulHousesSolution
{
    public void Run()
    {
        int height = int.Parse(Console.ReadLine()!);
        int width = int.Parse(Console.ReadLine()!);
        int colors = int.Parse(Console.ReadLine()!);

        foreach (int count in Solve(height, width, colors))
            Console.WriteLine(count);
    }

    public int[] Solve(int height, int width, int colorsCount)
    {
        int[] result = new int[colorsCount];
        int total = height * width;
        for (int i = 0; i < total; i++)
            result[i % colorsCount]++;
        return result;
    }
}
