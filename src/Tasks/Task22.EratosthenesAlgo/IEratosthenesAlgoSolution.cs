using Tasks.Common;

namespace Task22.EratosthenesAlgo;

public interface IEratosthenesAlgoSolution : ISolution
{
   public List<int> FindPrimesNumbers(int max);
}
