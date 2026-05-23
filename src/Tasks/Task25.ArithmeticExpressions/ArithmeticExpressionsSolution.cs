using Tasks.Common;
using System.Globalization;

namespace Task25.ArithmeticExpressions;

[Task(25, "Разбор арифметических выражений")]
public class ArithmeticExpressionsSolution : IArithmeticExpressionsSolution
{
    public void Run()
    {
        string input = Console.ReadLine()!;
        double result = CalculateExpression(input);
        Console.WriteLine(result.ToString(CultureInfo.GetCultureInfo("ru-RU")));
    }

    // Матрица переходов алгоритма Бауэра-Замельзона.
    // Строка — символ на вершине T (или $, если T пуст).
    // Столбец — текущий символ из строки (или $, если строка кончилась).
    // Значение — номер функции f1..f6, которую надо выполнить.
    private static readonly int[,] table = new int[6, 7]
    {
        //      $  (  +  -  *  /  )
        /* $ */ {6, 1, 1, 1, 1, 1, 5},
        /* ( */ {5, 1, 1, 1, 1, 1, 3},
        /* + */ {4, 1, 2, 2, 1, 1, 4},
        /* - */ {4, 1, 2, 2, 1, 1, 4},
        /* * */ {4, 1, 4, 4, 2, 2, 4},
        /* / */ {4, 1, 4, 4, 2, 2, 4},
    };

    // Индексы столбцов матрицы для текущего символа из входной строки.
    private static readonly Dictionary<char, int> colIndex = new Dictionary<char, int>
    {
        {'$', 0}, {'(', 1}, {'+', 2}, {'-', 3},
        {'*', 4}, {'/', 5}, {')', 6}
    };

    // Индексы строк матрицы для символа на вершине T. ')' тут нет — она в T никогда не попадает.
    private static readonly Dictionary<char, int> rowIndex = new Dictionary<char, int>
    {
        {'$', 0}, {'(', 1}, {'+', 2}, {'-', 3},
        {'*', 4}, {'/', 5}
    };

    public double CalculateExpression(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
            throw new InvalidOperationException("Пустое выражение");

        MyStackChar T = new MyStackChar();
        MyStackDouble E = new MyStackDouble();
        int pos = 0;

        while (true)
        {
            // Пропускаем пробелы.
            while (pos < expression.Length && char.IsWhiteSpace(expression[pos]))
            {
                pos++;
            }

            // Если символы кончились — играем роль $.
            char currentChar;
            if (pos >= expression.Length)
            {
                currentChar = '$';
            }
            else
            {
                currentChar = expression[pos];
            }

            // Числа в матрицу не идут — сразу в стек E.
            if (currentChar != '$' && (char.IsDigit(currentChar) || currentChar == ','))
            {
                int start = pos;
                while (pos < expression.Length && (char.IsDigit(expression[pos]) || expression[pos] == ','))
                {
                    pos++;
                }
                string numStr = expression.Substring(start, pos - start).Replace(',', '.');
                double number = double.Parse(numStr, CultureInfo.InvariantCulture);
                E.Push(number);
                continue;
            }

            // Операция или скобка — должна быть в словаре столбцов.
            if (!colIndex.ContainsKey(currentChar))
                throw new InvalidOperationException($"Invalid character: {currentChar}");

            // Если T пуст — подставляем $ как индекс строки 0.
            char topT = T.Count() > 0 ? T.Peek() : '$';
            int row = rowIndex.ContainsKey(topT) ? rowIndex[topT] : 0;
            int col = colIndex[currentChar];

            int f = table[row, col];
            switch (f)
            {
                // f1: положить операцию в T и шагнуть дальше.
                case 1:
                    T.Push(currentChar);
                    pos++;
                    break;

                // f2: посчитать тройку, положить новую операцию в T и шагнуть дальше.
                case 2:
                    CalculateTriple(T, E);
                    T.Push(currentChar);
                    pos++;
                    break;

                // f3: убрать символ с вершины T (закрытие скобки) и шагнуть дальше.
                case 3:
                    T.Pop();
                    pos++;
                    break;

                // f4: посчитать тройку и снова посмотреть в матрицу с тем же символом.
                case 4:
                    CalculateTriple(T, E);
                    break;

                // f5: ошибочная комбинация (например, лишняя ')').
                case 5:
                    throw new InvalidOperationException("Ошибка в выражении");

                // f6: всё посчитано, результат на вершине E.
                case 6:
                    return E.Peek();
            }
        }
    }

    // Берёт операцию с T и два операнда с E, выполняет, кладёт результат в E.
    // Порядок Pop'ов важен: первое — правый операнд, второе — левый.
    private void CalculateTriple(MyStackChar T, MyStackDouble E)
    {
        double b = E.Pop();
        double a = E.Pop();
        char op = T.Pop();

        switch (op)
        {
            case '+': E.Push(a + b); break;
            case '-': E.Push(a - b); break;
            case '*': E.Push(a * b); break;
            case '/': E.Push(a / b); break;
            default: throw new InvalidOperationException($"Invalid operation: {op}");
        }
    }
}
