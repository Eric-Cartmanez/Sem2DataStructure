using Tasks.Common;

namespace Task18.HanoiTowers;

[Task(18, "Ханойские башни", "alt")]
public class HanoiTowersSolutionAlt : IHanoiTowersSolution
{
    private int _count;
    private int _stepCount;
    private Tower[] _towers = [];

    public void Run()
    {
        _count = ReadCount();
        Tower sourceTower = new Tower();
        Tower destTower = new Tower();
        Tower tempTower = new Tower();

        _towers = [sourceTower, destTower, tempTower];

        for (int i = _count; i > 0; i--)
            sourceTower.AddRing(i);

        Print();
        Move(sourceTower, destTower, tempTower, _count);
    }

    private int ReadCount()
    {
        Console.WriteLine("Введите кол-во колец для игры (число >= 0)");

        int input;
        while (!int.TryParse(Console.ReadLine(), out input) || input <= 0)
            Console.WriteLine("Неверный ввод, попробуйте еще раз");

        return input;
    }

    private void Print()
    {
        Console.WriteLine($"Шаг {_stepCount}");
        for (int i = _count - 1; i >= 0; i--)
        {
            foreach (Tower tower in _towers)
            {
                int? ring = tower.GetRing(i);
                Console.Write(ring.HasValue ? $"{ring.Value} " : $"{0} ");
            }
            Console.Write("\n");
        }
        Console.WriteLine();
    }

    public void Move(Tower sourceTower, Tower destTower, Tower tempTower, int count)
    {
        if (count <= 0)
            return;

        Move(sourceTower, tempTower, destTower, count - 1);

        int ring = sourceTower.RemoveRing();
        destTower.AddRing(ring);
        _stepCount++;
        Print();

        Move(tempTower, destTower, sourceTower, count - 1);
    }
}

/// <summary>
/// Для Tower используется класс т.к состояние _rings постоянно меняется
/// и чтобы избежать непонятностей, когда структура копируется, а ссылка на _rings остается общая
/// </summary>
public class Tower
{
    private readonly Stack<int> _rings = [];

    public void AddRing(int size)
    {
        if (_rings.Count > 0 && _rings.Peek() < size)
            throw new InvalidOperationException("Нельзя положить большее кольцо на меньшее");

        _rings.Push(size);
    }

    public int RemoveRing()
    {
        if (_rings.Count == 0)
            throw new InvalidOperationException("Нельзя снять кольцо, башня пуста");

        return _rings.Pop();
    }

    public int? GetRing(int level)
    {
        if (level < 0 || level >= _rings.Count)
            return null;

        // инвертируем индекс, чтобы 0 считался дном
        // по дефолту в Stack 0 - верхний элемент стека
        int levelIdx = _rings.Count - 1 - level;
        return _rings.ElementAt(levelIdx);
    }
}
