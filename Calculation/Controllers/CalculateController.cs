using Calculation.Dtos;
using Domain.Entities;
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

        if (result is ErrorEntity)
        {
            return BadRequest(result as ErrorEntity);
        }
        else
        {
            return Ok((result as CalculationEntity).Result);
        }
        
    }
}