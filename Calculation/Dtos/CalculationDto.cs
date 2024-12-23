using Domain.Entities;

namespace Calculation.Dtos;

public class CalculationDto
{
    public string Expression { get; set; } = string.Empty;

    public CalculationEntity ToModel()
    {
        var entity = new CalculationEntity();
        entity.Expression = Expression;
        return entity;
    }
}