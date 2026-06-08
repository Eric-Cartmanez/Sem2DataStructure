using Tasks.Common;

namespace Task19.LongestPath;

[Task(19, "Самый длинный путь", "alt")]
public class LongestPathAltSolution : ILongestPathSolution
{
    private static readonly int[] RowSteps = [-1, 1, 0, 0];
    private static readonly int[] ColSteps = [0, 0, -1, 1];

    public void Run()
    {
        int[,] matrix = {
            { 1,  2,  3,  4,  5 },
            { 10, 9,  8,  7,  6 },
            { 11, 12, 13, 14, 15 },
            { 20, 19, 18, 17, 16 },
            { 21, 22, 23, 24, 25 }
        };
        int[,] inputData =
        {
            { 2, 5, 1, 0 },
            { 3, 3, 1, 9 },
            { 4, 4, 7, 8 },
        };

        List<(int x, int y)> longestPath = FindLongestPath(matrix);

        foreach (var coord in longestPath)
            Console.WriteLine(coord);
    }

    private List<(int x, int y)> FindLongestPath(int[,] ints)
    {
        int rows = ints.GetLength(0);
        int cols = ints.GetLength(1);
        List<(int row, int col)> longestPath = [];
        List<(int row, int col)>[,] memo = new List<(int row, int col)>[rows, cols];

        for (int row = 0; row < rows; row++)
            for (int col = 0; col < cols; col++)
            {
                List<(int row, int col)> path = GetMaxPathFrom(ints, (row, col), memo);
                if (path.Count > longestPath.Count)
                    longestPath = path;
            }

        return longestPath;
    }

    private List<(int row, int col)> GetMaxPathFrom(int[,] ints, (int row, int col) coord, List<(int row, int col)>?[,] memo)
    {
        List<(int row, int col)>? cache = memo[coord.row, coord.col];

        if (cache != null)
            return cache;

        List<(int row, int col)> maxSubPath = [];

        for (int i = 0; i < RowSteps.Length; i++)
        {
            int nextRow = coord.row + RowSteps[i];
            int nextCol = coord.col + ColSteps[i];

            if (!CanDive(ints, coord, (nextRow, nextCol)))
                continue;

            List<(int row, int col)> neighborPath = GetMaxPathFrom(ints, (nextRow, nextCol), memo);
            if (neighborPath.Count > maxSubPath.Count)
                maxSubPath = neighborPath;
        }

        List<(int row, int col)> result = [(coord.row, coord.col)];
        result.AddRange(maxSubPath);

        memo[coord.row, coord.col] = result;
        return result;
    }

    private bool CanDive(int[,] ints, (int row, int col) currCoord, (int row, int col) nextCoord)
    {
        return nextCoord.row >= 0 && nextCoord.row < ints.GetLength(0) &&
               nextCoord.col >= 0 && nextCoord.col < ints.GetLength(1) &&
               ints[currCoord.row, currCoord.col] < ints[nextCoord.row, nextCoord.col];
    }

    public Point[] FindLongestPath(int[,] arr, out int length)
    {
        var lp = FindLongestPath(arr);
        Point[] r = new Point[lp.Count];
        for (int i = 0; i < r.Length; i++)
            r[i] = new Point(lp[i].x, lp[i].y);

        length = lp.Count;
        return r;
    }
}
