using System.Reflection;
using Tasks.Common;

var executableDir = AppContext.BaseDirectory;
var allTypes = new List<Type>();

foreach (var dllPath in Directory.GetFiles(executableDir, "*.dll"))
{
    try
    {
        var assembly = Assembly.LoadFrom(dllPath);
        allTypes.AddRange(
            assembly.GetTypes()
                    .Where(t => typeof(ISolution).IsAssignableFrom(t)
                             && !t.IsAbstract
                             && !t.IsInterface
                             && t.GetCustomAttribute<TaskAttribute>() != null));
    }
    catch { }
}

// Группируем по номеру задачи, сортируем по возрастанию
var taskGroups = allTypes
    .GroupBy(t => t.GetCustomAttribute<TaskAttribute>()!.Number)
    .OrderBy(g => g.Key)
    .Select(g => g.ToList())
    .ToList();

if (taskGroups.Count == 0)
{
    Console.WriteLine("Задачи не найдены.");
    return;
}

while (true)
{
    Console.Clear();
    Console.WriteLine("╔══════════════════════════════════════╗");
    Console.WriteLine("║        Sem2DataStructure             ║");
    Console.WriteLine("╚══════════════════════════════════════╝");
    Console.WriteLine();
    Console.WriteLine("Выберите задачу (0 — выход):");
    Console.WriteLine();

    for (int i = 0; i < taskGroups.Count; i++)
    {
        var group = taskGroups[i];
        var attr = group[0].GetCustomAttribute<TaskAttribute>()!;
        string suffix = group.Count > 1 ? $" [{group.Count} решения]" : "";
        Console.WriteLine($"  {i + 1,2}. [{attr.Number:D2}] {attr.TaskName}{suffix}");
    }

    Console.WriteLine();
    Console.Write("Задача: ");

    if (!int.TryParse(Console.ReadLine(), out int taskChoice) || taskChoice == 0)
        break;

    if (taskChoice < 1 || taskChoice > taskGroups.Count)
        continue;

    var selectedGroup = taskGroups[taskChoice - 1];
    Type selectedType;

    if (selectedGroup.Count == 1)
    {
        selectedType = selectedGroup[0];
    }
    else
    {
        Console.Clear();
        var taskAttr = selectedGroup[0].GetCustomAttribute<TaskAttribute>()!;
        Console.WriteLine($"=== [{taskAttr.Number:D2}] {taskAttr.TaskName} ===");
        Console.WriteLine("Выберите решение (0 — назад):");
        Console.WriteLine();

        for (int i = 0; i < selectedGroup.Count; i++)
        {
            var sAttr = selectedGroup[i].GetCustomAttribute<TaskAttribute>()!;
            Console.WriteLine($"  {i + 1}. {sAttr.SolutionName}");
        }

        Console.WriteLine();
        Console.Write("Решение: ");

        if (!int.TryParse(Console.ReadLine(), out int solutionChoice) || solutionChoice == 0)
            continue;

        if (solutionChoice < 1 || solutionChoice > selectedGroup.Count)
            continue;

        selectedType = selectedGroup[solutionChoice - 1];
    }

    Console.Clear();
    var solution = (ISolution)Activator.CreateInstance(selectedType)!;

    try
    {
        solution.Run();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Ошибка: {ex.Message}");
        Console.ResetColor();
    }

    Console.WriteLine();
    Console.WriteLine("Нажмите Enter для продолжения...");
    Console.ReadLine();
}
