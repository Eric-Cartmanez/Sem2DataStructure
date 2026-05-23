using Tasks.Common;

namespace Task22.EratosthenesAlgo;

[Task(22, "Алгоритм Эратосфена")]
public class EratosthenesAlgoSolution : IEratosthenesAlgoSolution
{
    public void Run()
    {

        Console.WriteLine("Введите максимальное число");
        int max = int.Parse(Console.ReadLine());

        List<int> primes = FindPrimesNumbers(max);
        Console.WriteLine(string.Join(" ", primes));
    }

    public List<int> FindPrimesNumbers(int max)
    {
        if (max < 2)
            throw new ArgumentException("Max должен быть больше 1");

        // Решето: индекс — число, значение true = "вычеркнуто как составное".
        bool[] possibleCompositeNumbers = new bool[max + 1];

        // Достаточно перебирать p только до sqrt(max): любое составное число n
        // имеет хотя бы один делитель <= sqrt(n) и уже будет вычеркнуто.
        // (long) защищает p*p от переполнения int.
        for (int p = 2; (long)p * p <= max; p++)
        {
            // Если p уже помечено — оно составное, его кратные вычеркнуты меньшим простым.
            if (possibleCompositeNumbers[p]) continue;

            // Начинаем с p*p: все меньшие кратные (2p, 3p, ..., (p-1)p)
            // уже вычеркнуты простыми, меньшими p.
            for (int j = p * p; j <= max; j += p)
                possibleCompositeNumbers[j] = true;
        }

        // Собираем не вычеркнутые числа в порядке возрастания.
        List<int> primes = new List<int>();
        for (int i = 2; i <= max; i++)
            if (!possibleCompositeNumbers[i])
                primes.Add(i);

        return primes;
    }
}
