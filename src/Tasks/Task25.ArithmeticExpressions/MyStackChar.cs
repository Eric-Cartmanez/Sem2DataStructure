namespace Task25.ArithmeticExpressions;

public class MyStackChar
{
    private const int DefaultSize = 100;
    private const int Empty = -1;
    private readonly char[] items;
    private int top = Empty;

    public MyStackChar(int size = DefaultSize)
    {
        items = new char[size];
    }

    public void Push(char value)
    {
        items[++top] = value;
    }

    public char Pop()
    {
        return items[top--];
    }

    public char Peek()
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
