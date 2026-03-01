#!/usr/bin/env bash
# Создаёт заготовку новой задачи и подключает её к решению.
#
# Использование:
#   ./makeTask.sh --number 8 --name MyNewTask
#
# Что создаётся:
#   src/Tasks/Task{NN}.{Name}/
#     Task{NN}.{Name}.csproj
#     I{Name}Solution.cs        (интерфейс задачи)
#     {Name}Solution.cs         (класс решения)
#     Task.md                   (описание задачи)
#   tests/Tasks.Tests/Task{NN}{Name}Tests.cs
#
# Проект автоматически добавляется в:
#   - Sem2DataStructure.slnx
#   - src/Runner/Runner.csproj
#   - tests/Tasks.Tests/Tasks.Tests.csproj

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"

NUMBER=""
NAME=""

while [[ "$#" -gt 0 ]]; do
    case "$1" in
        --number) NUMBER="$2"; shift ;;
        --name)   NAME="$2";   shift ;;
        *) echo "Неизвестный параметр: $1" >&2; exit 1 ;;
    esac
    shift
done

if [[ -z "$NUMBER" || -z "$NAME" ]]; then
    echo "Использование: $0 --number <номер> --name <имя>" >&2
    exit 1
fi

if ! [[ "$NUMBER" =~ ^[0-9]+$ ]]; then
    echo "Номер задачи должен быть целым числом." >&2
    exit 1
fi

PADDED="$(printf '%02d' "$NUMBER")"
PROJECT_NAME="Task${PADDED}.${NAME}"
PROJECT_DIR="${SCRIPT_DIR}/src/Tasks/${PROJECT_NAME}"
PROJECT_FILE="${PROJECT_DIR}/${PROJECT_NAME}.csproj"

if [[ -d "$PROJECT_DIR" ]]; then
    echo "Задача ${PROJECT_NAME} уже существует." >&2
    exit 1
fi

echo "Создаю задачу: ${PROJECT_NAME}"

# ── Создать проект ────────────────────────────────────────────────────────────
mkdir -p "$PROJECT_DIR"

cat > "$PROJECT_FILE" <<EOF
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\\..\\Tasks.Common\\Tasks.Common.csproj" />
  </ItemGroup>

</Project>
EOF

# ── Интерфейс задачи ──────────────────────────────────────────────────────────
cat > "${PROJECT_DIR}/I${NAME}Solution.cs" <<EOF
using Tasks.Common;

namespace ${PROJECT_NAME};

public interface I${NAME}Solution : ISolution
{
    // TODO: добавить методы с логикой задачи
}
EOF

# ── Класс решения ─────────────────────────────────────────────────────────────
cat > "${PROJECT_DIR}/${NAME}Solution.cs" <<EOF
using Tasks.Common;

namespace ${PROJECT_NAME};

[Task(${NUMBER}, "TODO: название задачи")]
public class ${NAME}Solution : I${NAME}Solution
{
    public void Run()
    {
        // TODO: реализовать Run() — ввод/вывод через Console
        throw new NotImplementedException();
    }
}
EOF

# ── Описание задачи ───────────────────────────────────────────────────────────
cat > "${PROJECT_DIR}/Task.md" <<EOF
# Задача ${PADDED}: ${NAME}

TODO: описание задачи.
EOF

# ── Файл тестов ───────────────────────────────────────────────────────────────
TEST_FILE="${SCRIPT_DIR}/tests/Tasks.Tests/Task${PADDED}${NAME}Tests.cs"

cat > "$TEST_FILE" <<EOF
using ${PROJECT_NAME};

namespace Tasks.Tests;

public class Task${PADDED}${NAME}Tests
{
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new ${NAME}Solution()];
    }

    // TODO: добавить тесты
    // [Theory]
    // [MemberData(nameof(GetSolutions))]
    // public void MyTest(I${NAME}Solution s) { ... }
}
EOF

# ── Подключить к решению ──────────────────────────────────────────────────────
cd "$SCRIPT_DIR"
dotnet sln add "$PROJECT_FILE"

# ── Подключить к Runner ───────────────────────────────────────────────────────
RELATIVE_FROM_RUNNER="..\\Tasks\\${PROJECT_NAME}\\${PROJECT_NAME}.csproj"
dotnet add src/Runner/Runner.csproj reference "$PROJECT_FILE"

# ── Подключить к тестам ───────────────────────────────────────────────────────
dotnet add tests/Tasks.Tests/Tasks.Tests.csproj reference "$PROJECT_FILE"

echo ""
echo "✓ Задача ${PROJECT_NAME} создана."
echo ""
echo "Следующие шаги:"
echo "  1. Заполнить Task.md — описание задачи"
echo "  2. Добавить методы в I${NAME}Solution.cs"
echo "  3. Реализовать ${NAME}Solution.cs"
echo "  4. Написать тесты в Task${PADDED}${NAME}Tests.cs"
