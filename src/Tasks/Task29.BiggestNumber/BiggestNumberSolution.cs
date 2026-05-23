using Tasks.Common;
using System.Globalization;

namespace Task29.BiggestNumber;

[Task(29, "Самое большое число")]
public class BiggestNumberSolution : IBiggestNumberSolution
{
    public void Run()
    {
        string input = Console.ReadLine() ?? "";
        double result = BiggestNumber(input);
        Console.WriteLine(result);
    }

    public double BiggestNumber(string str)
    {
        if (string.IsNullOrEmpty(str))
            return 0;

        double result = 0;
        int i = 0;

        while (i < str.Length)
        {
            if (!char.IsDigit(str[i]) && str[i] != ',')
            {
                i++;
                continue;
            }

            int start = i;
            while (i < str.Length && (char.IsDigit(str[i]) || str[i] == ','))
                i++;

            ProcessRun(str.Substring(start, i - start), ref result);
        }

        return result;
    }

    private static void ProcessRun(string run, ref double result)
    {
        int decIdx = -1;
        for (int k = run.Length - 1; k > 0; k--)
        {
            if (run[k] == ',' && k + 1 < run.Length
                && char.IsDigit(run[k - 1]) && char.IsDigit(run[k + 1]))
            {
                decIdx = k;
                break;
            }
        }

        string remaining;
        if (decIdx >= 0)
        {
            int intStart = decIdx;
            while (intStart > 0 && char.IsDigit(run[intStart - 1]))
                intStart--;

            int fracEnd = decIdx + 1;
            while (fracEnd < run.Length && char.IsDigit(run[fracEnd]))
                fracEnd++;

            string decStr = run.Substring(intStart, fracEnd - intStart);
            TryUpdateMax(decStr, ref result);

            string before = run.Substring(0, intStart);
            string after = fracEnd < run.Length ? run.Substring(fracEnd) : "";
            remaining = before + "," + after;
        }
        else
        {
            remaining = run;
        }

        foreach (var part in remaining.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            TryUpdateMax(part, ref result);
        }
    }

    private static void TryUpdateMax(string s, ref double result)
    {
        s = s.Replace(',', '.');
        if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double v))
            result = Math.Max(result, v);
    }
}
