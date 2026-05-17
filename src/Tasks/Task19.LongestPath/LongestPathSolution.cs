using Tasks.Common;

namespace Task19.LongestPath;

public struct Point
{
    public Point(int r, int c)
    {

        if (r < 0 || c < 0)
            throw new ArgumentException("Row and column must be non-negative");

        Row = r;
        Column = c;
    }

    public int Row { get; }
    public int Column { get; }
}

[Task(19, "Самый длинный путь")]
public class LongestPathSolution : ILongestPathSolution
{
    public void Run()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Задача 19: Самый длинный путь возрастания");
        Console.ResetColor();

        int[,] arr =
        {
            { 2, 5, 1, 0 },
            { 3, 3, 1, 9 },
            { 4, 4, 7, 8 },
        };

        Console.WriteLine();
        Console.WriteLine("Исходный массив:");
        PrintArray(arr);

        Point[] path = FindLongestPath(arr, out int length);

        Console.WriteLine();
        Console.Write("Длина пути: ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(length);
        Console.ResetColor();

        Console.Write("Маршрут по ячейкам: ");
        for (int i = 0; i < path.Length; i++)
        {
            Console.Write($"({path[i].Row},{path[i].Column})");
            if (i < path.Length - 1) Console.Write(" — ");
        }
        Console.WriteLine();

        Console.Write("Маршрут по значениям: ");
        for (int i = 0; i < path.Length; i++)
        {
            Console.Write(arr[path[i].Row, path[i].Column]);
            if (i < path.Length - 1) Console.Write('-');
        }
        Console.WriteLine();
    }

    private static void PrintArray(int[,] arr)
    {
        int rows = arr.GetLength(0);
        int cols = arr.GetLength(1);
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Console.Write(arr[r, c]);
                if (c < cols - 1) Console.Write(' ');
            }
            Console.WriteLine();
        }
    }

    public Point[] FindLongestPath(int[,] arr, out int length)
    {
        int rows = arr.GetLength(0);
        int cols = arr.GetLength(1);

        if (rows == 0 || cols == 0)
        {
            length = 0;
            return [];
        }

        int[,] memo = new int[rows, cols];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                LongestFrom(arr, r, c, memo);
            }
        }

        int startR = 0;
        int startC = 0;
        length = memo[0, 0];
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (memo[r, c] > length)
                {
                    length = memo[r, c];
                    startR = r;
                    startC = c;
                }
            }
        }

        Point[] path = new Point[length];
        path[0] = new Point(startR, startC);
        int curR = startR;
        int curC = startC;

        for (int i = 1; i < length; i++)
        {
            int nextR = -1;
            int nextC = -1;

            if (curR - 1 >= 0
                && arr[curR - 1, curC] > arr[curR, curC]
                && memo[curR - 1, curC] == memo[curR, curC] - 1)
            {
                nextR = curR - 1; nextC = curC;
            }
            else if (curC - 1 >= 0
                && arr[curR, curC - 1] > arr[curR, curC]
                && memo[curR, curC - 1] == memo[curR, curC] - 1)
            {
                nextR = curR; nextC = curC - 1;
            }
            else if (curC + 1 < cols
                && arr[curR, curC + 1] > arr[curR, curC]
                && memo[curR, curC + 1] == memo[curR, curC] - 1)
            {
                nextR = curR; nextC = curC + 1;
            }
            else if (curR + 1 < rows
                && arr[curR + 1, curC] > arr[curR, curC]
                && memo[curR + 1, curC] == memo[curR, curC] - 1)
            {
                nextR = curR + 1; nextC = curC;
            }

            path[i] = new Point(nextR, nextC);
            curR = nextR;
            curC = nextC;
        }

        return path;
    }

    private int LongestFrom(int[,] arr, int r, int c, int[,] memo)
    {
        if (memo[r, c] != 0) return memo[r, c];

        int[] directionsPaths = new int[4];
        if (r - 1 >= 0 && arr[r - 1, c] > arr[r, c])
            directionsPaths[0] = LongestFrom(arr, r - 1, c, memo);
        if (c - 1 >= 0 && arr[r, c - 1] > arr[r, c])
            directionsPaths[1] = LongestFrom(arr, r, c - 1, memo);
        if (c + 1 < arr.GetLength(1) && arr[r, c + 1] > arr[r, c])
            directionsPaths[2] = LongestFrom(arr, r, c + 1, memo);
        if (r + 1 < arr.GetLength(0) && arr[r + 1, c] > arr[r, c])
            directionsPaths[3] = LongestFrom(arr, r + 1, c, memo);

        int path = directionsPaths.Max() + 1;
        memo[r, c] = path;
        return path;
    }
}
