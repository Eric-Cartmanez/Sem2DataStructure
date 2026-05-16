using Tasks.Common;

namespace Task18.HanoiTowers;

[Task(18, "Ханойские башни")]
public class HanoiTowersSolution : IHanoiTowersSolution
{
    public struct Tower
    {
        private List<int> _disks;

        public Tower(string name)
        {
            Name = name;
            _disks = new List<int>();
        }

        public string Name { get; }

        public int Count
        {
            get { return _disks.Count; }
        }

        public int GetDisk(int level)
        {
            return _disks[level];
        }

        public void Push(int disk)
        {
            _disks.Add(disk);
        }

        public int Pop()
        {
            int top = _disks[_disks.Count - 1];
            _disks.RemoveAt(_disks.Count - 1);
            return top;
        }
    }

    private int _step;
    private int _totalDisks;

    public void Run()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("ИГРА Ханойские башни");
        Console.ResetColor();

        Console.Write("Введите количество колец: ");

        if (!int.TryParse(Console.ReadLine(), out int count) || count < 0)
        {
            Console.WriteLine("Некорректный ввод");
            return;
        }

        var t1 = new Tower("A");
        var t2 = new Tower("B");
        var t3 = new Tower("C");

        for (int i = count; i > 0; i--)
        {
            t1.Push(i);
        }

        _totalDisks = count;
        _step = 0;

        PrintState(t1, t2, t3);
        Move(t1, t2, t3, count);

        Console.WriteLine("ИГРА окончена");
    }

    public void Move(Tower from, Tower to, Tower temp, int count)
    {
        if (count <= 0) return;

        Move(from, temp, to, count - 1);

        to.Push(from.Pop());
        PrintState(from, to, temp);

        Move(temp, to, from, count - 1);
    }

    private void PrintState(Tower from, Tower to, Tower temp)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Шаг №{_step}");
        Console.ResetColor();

        var towers = new[] { from, to, temp };
        Array.Sort(towers, (x, y) => string.CompareOrdinal(x.Name, y.Name));

        for (int level = _totalDisks - 1; level >= 0; level--)
        {
            var values = new int[3];
            for (int i = 0; i < 3; i++)
            {
                if (level < towers[i].Count)
                {
                    values[i] = towers[i].GetDisk(level);
                }
                else
                {
                    values[i] = 0;
                }
            }
            Console.WriteLine(string.Join("   ", values));
        }

        _step++;
    }
}
