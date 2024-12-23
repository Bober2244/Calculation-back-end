namespace Domain.Entities;

public class ErrorEntity : ResultEntity
{
    public string Message { get; set; } = string.Empty;
    
    public int StatusCode { get; set; }

    public ErrorEntity()
    {
    }
    
    public ErrorEntity(string message, int statusCode)
    {
        StatusCode = statusCode;
        Message = message;
    }
}