using Domain.Entities;
using Services.Interfaces;

namespace Services.Implementations;

public class CalculationService : ICalculationService
{
    public async Task<ResultEntity> Calculate(ResultEntity calculator)
    {
        var calc = calculator as CalculationEntity;
        try
        {
            var (result, error) = RPN.Rpn.Calculate(calc.Expression);

            if (error != null)
            {
                return error;
            }

            if (double.IsInfinity(result.Value) || double.IsNaN(result.Value))
            { 
                return new ErrorEntity("Результат слишком большой", 400);
            }

            calc.Result = result.Value;
        }
        catch (Exception ex)
        {
            return new ErrorEntity($"Произошла ошибка при вычислении: {ex.Message}", 500);
        }

        return await Task.FromResult(calculator);
    }
}