using Domain.Entities;

namespace Services.Interfaces;

public interface ICalculationService
{
    Task<ResultEntity> Calculate(ResultEntity calculator);
}