using Tasks.Common;

namespace Task04.CombArray;

[Task(4, "Комбинированный массив")]
public class CombArraySolution : ICombArraySolution
{
    public void Run()
    {
        Console.WriteLine("Введите размер массива:");
        int size = int.Parse(Console.ReadLine()!);
        var array = new int[size];
        for (int i = 0; i < size; i++)
            array[i] = int.Parse(Console.ReadLine()!);

        foreach (int element in FilterNonDecreasing(array))
            Console.WriteLine(element);
    }

    // O(n) по времени, O(n) по памяти.
    // Сохраняем первый элемент, затем каждый следующий — только если >= последнего сохранённого.
    public int[] FilterNonDecreasing(int[] array)
    {
        if (array.Length == 0)
            return [];

        var result = new int[array.Length];
        result[0] = array[0];
        int resultIndex = 0;

        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] >= result[resultIndex])
                result[++resultIndex] = array[i];
        }

        return result[..(resultIndex + 1)];
    }
}
