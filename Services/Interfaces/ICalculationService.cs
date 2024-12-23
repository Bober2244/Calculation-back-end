using Domain.Entities;

namespace Services.Interfaces;

public interface ICalculationService
{
    Task<CalculationEntity> Calculate(CalculationEntity calculator);
}