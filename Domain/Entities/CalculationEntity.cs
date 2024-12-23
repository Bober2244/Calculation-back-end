namespace Domain.Entities;

public class CalculationEntity : ResultEntity
{
    public string Expression { get; set; } = string.Empty;
    
    public double Result { get; set; }
}