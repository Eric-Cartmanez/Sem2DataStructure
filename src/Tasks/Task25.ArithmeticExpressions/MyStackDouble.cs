namespace Task25.ArithmeticExpressions;

public class MyStackDouble
{
    private const int DefaultSize = 100;
    private const int Empty = -1;
    private readonly double[] items;
    private int top = Empty;

    public MyStackDouble(int size = DefaultSize)
    {
        items = new double[size];
    }

    public void Push(double value)
    {
        items[++top] = value;
    }

    public double Pop()
    {
        return items[top--];
    }

    public double Peek()
    {
        return items[top];
    }

    public void Clear()
    {
        top = Empty;
    }

    public int Count()
    {
        return top + 1;
    }
}
