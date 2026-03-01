namespace Tasks.Common;

/// <summary>
/// Помечает класс-решение задачи. Используется Runner'ом для формирования меню.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class TaskAttribute : Attribute
{
    /// <summary>Порядковый номер задачи. Определяет сортировку в меню.</summary>
    public int Number { get; }

    /// <summary>Название задачи. Отображается как заголовок группы в меню.</summary>
    public string TaskName { get; }

    /// <summary>
    /// Название конкретного решения. Указывается только если у задачи несколько решений.
    /// </summary>
    public string SolutionName { get; }

    public TaskAttribute(int number, string taskName, string solutionName = "")
    {
        Number = number;
        TaskName = taskName;
        SolutionName = solutionName;
    }
}
