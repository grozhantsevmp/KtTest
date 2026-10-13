
using Xunit;
using Microsoft.AspNetCore.Mvc;
using BudgetCalculator.Api.Controllers;
using BudgetCalculator.Api.Models;

public class BudgetControllerTests
{
    private static decimal GetTotal(IActionResult result)
    {
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, ok.StatusCode);

        var property = ok.Value?.GetType()
            .GetProperty("TotalAmount");

        Assert.NotNull(property);

        return Assert.IsType<decimal>(
            property.GetValue(ok.Value));
    }

    // Тест 1: Обычный расчет
    [Fact]
    public void Calculate_WithoutInsurance_Returns500()
    {
        var controller = new BudgetController();

        var result = controller.Calculate(new BudgetRequest
        {
            DailyCost = 100m,
            Days = 5,
            IncludeInsurance = false
        });

        Assert.Equal(500m, GetTotal(result));
    }

    // Тест 2: Расчет со страховкой
    [Fact]
    public void Calculate_WithInsurance_Returns525()
    {
        var controller = new BudgetController();

        var result = controller.Calculate(new BudgetRequest
        {
            DailyCost = 100m,
            Days = 5,
            IncludeInsurance = true
        });

        Assert.Equal(525m, GetTotal(result));
    }

    // Тест 3: Отрицательное количество дней
    [Fact]
    public void Calculate_NegativeDays_Returns400()
    {
        var controller = new BudgetController();

        var result = controller.Calculate(new BudgetRequest
        {
            DailyCost = 100m,
            Days = -1,
            IncludeInsurance = false
        });

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, bad.StatusCode);
    }

    // Тест 4: Нулевая стоимость
    [Fact]
    public void Calculate_ZeroCost_ReturnsZero()
    {
        var controller = new BudgetController();

        var result = controller.Calculate(new BudgetRequest
        {
            DailyCost = 0m,
            Days = 5,
            IncludeInsurance = false
        });

        Assert.Equal(0m, GetTotal(result));
    }

    // Тест 5: Дробная стоимость
    [Fact]
    public void Calculate_DecimalCost_Returns301_50()
    {
        var controller = new BudgetController();

        var result = controller.Calculate(new BudgetRequest
        {
            DailyCost = 100.50m,
            Days = 3,
            IncludeInsurance = false
        });

        Assert.Equal(301.50m, GetTotal(result));
    }
}
