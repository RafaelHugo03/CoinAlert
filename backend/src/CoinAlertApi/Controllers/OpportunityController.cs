using CoinAlertApi.Application.DTOs;
using CoinAlertApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoinAlertApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OpportunityController(IOpportunityService opportunityService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<OpportunityDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var opportunities = await opportunityService.GetAllAsync();
        return Ok(opportunities);
    }


    [HttpPost]
    [ProducesResponseType(typeof(OpportunityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateOpportunityDto dto)
    {
        var opportunity = await opportunityService.CreateAsync(dto);
        return Ok(opportunity);
    }
}
