using Calculation.Dtos;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Calculation.Controllers;

[ApiController]
[Route("[controller]")]
public class CalculateController(
    ICalculationService calculationService
) : ControllerBase
{
    [HttpPost("calc")]
    public async Task<ActionResult<double>> Calculate(CalculationDto calculationDto)
    {
        var result = await calculationService.Calculate(calculationDto.ToModel());
        
        
        return Ok(result.Result);
    }
}