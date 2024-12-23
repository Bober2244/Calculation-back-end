namespace Domain.Entities;

public class CalculationEntity
{
    public string Expression { get; set; } = string.Empty;
    
    public double Result { get; set; }
    
    public ErrorEntity? Error { get; set; }
}