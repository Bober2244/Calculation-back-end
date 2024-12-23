using Domain.Entities;
using Services.Interfaces;

namespace Services.Implementations;

public class CalculationService : ICalculationService
{
    public async Task<CalculationEntity> Calculate(CalculationEntity calculator)
    {
        try
        {
            var (result, error) = RPN.Rpn.Calculate(calculator.Expression);

            if (error != null)
            {
                calculator.Error = error;
                return calculator;
            }

            if (double.IsInfinity(result) || double.IsNaN(result))
            {
                calculator.Error = new ErrorEntity("Результат слишком большой", 400);
                return calculator;
            }

            calculator.Result = result;
        }
        catch (Exception ex)
        {
            calculator.Error = new ErrorEntity($"Произошла ошибка при вычислении: {ex.Message}", 500);
        }

        return await Task.FromResult(calculator);
    }
}