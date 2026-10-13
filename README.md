# Калькулятор бюджета — ASP.NET Core + xUnit

Готовый учебный проект: REST API и 5 модульных тестов.

## Требования

- .NET 8 SDK или новее с поддержкой сборки net8.0
- VS Code и расширение C# Dev Kit

## Как открыть

1. Распакуйте архив.
2. В VS Code: File → Open Folder → папка `BudgetCalculator`.
3. Откройте терминал (Ctrl+`).

## Запуск API

```powershell
dotnet run --project BudgetCalculator.Api
```

Адрес API: `http://localhost:5080/api/budget/calculate` (профиль http).

Проверка из второго терминала PowerShell:

```powershell
Invoke-RestMethod -Uri "http://localhost:5080/api/budget/calculate" -Method Post -ContentType "application/json" -Body '{"dailyCost":100,"days":5,"includeInsurance":true}'
```

Ожидаемая сумма: 525.

## Запуск тестов

```powershell
dotnet test BudgetCalculator.Tests/BudgetCalculator.Tests.csproj
```

Все пять тестов должны пройти. Для покрытия:

```powershell
dotnet test BudgetCalculator.Tests/BudgetCalculator.Tests.csproj --collect:"XPlat Code Coverage"
```

Отчёт появится в `BudgetCalculator.Tests/TestResults/.../coverage.cobertura.xml`.

## Как продемонстрировать обнаружение бага

1. Откройте `BudgetCalculator.Api/Controllers/BudgetController.cs`.
2. Временно замените `total *= 1.05m;` на `total *= 5;`.
3. Запустите `dotnet test` — тест `Calculate_WithInsurance_Returns525` упадёт: ожидается 525, получено 2500.
4. Верните `total *= 1.05m;` и запустите тесты снова.

В архиве баг **уже исправлен**, чтобы проект работал сразу.

## Примечание

Это unit-тесты контроллера: они вызывают метод напрямую, без HTTP-сервера. `decimal` используется для денежных значений. Для округления до копеек в сценариях с более чем двумя знаками потребуется отдельно согласовать правило округления.
