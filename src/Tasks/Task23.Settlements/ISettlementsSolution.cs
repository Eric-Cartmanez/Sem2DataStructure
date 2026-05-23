using Tasks.Common;

namespace Task23.Settlements;

public interface ISettlementsSolution : ISolution
{
    /// <summary>
    /// Строит хеш-таблицу размера 256 по принципу закрытого хеширования.
    /// При коллизиях использует циклически массив смещений {3, 5, 6}.
    /// Перед вычислением хеша каждое слово приводится к верхнему регистру.
    /// </summary>
    /// <param name="towns">Входной массив строк.</param>
    /// <param name="collisions">На выходе — общее число коллизий (каждое попадание в занятую ячейку).</param>
    /// <returns>Массив длины 256: ячейка содержит строку или null, если пуста.</returns>
    string[] BuildTable(string[] towns, out int collisions);
}
