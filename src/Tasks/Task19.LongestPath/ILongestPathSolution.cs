using Tasks.Common;

namespace Task19.LongestPath;

public interface ILongestPathSolution : ISolution
{
    public Point[] FindLongestPath(int[,] arr, out int length);
}
