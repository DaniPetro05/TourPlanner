using Microsoft.AspNetCore.Mvc;
using TourPlanner.DTOs;
using TourPlanner.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace TourPlanner.Controllers;

[ApiController]
[Route("api/tourlogs")]
[Authorize]
public class TourLogsController : ControllerBase
{
    private readonly ITourLogService _service;

    public TourLogsController(ITourLogService service)
    {
        _service = service;
    }

    [HttpGet("{tourId}")]
    public async Task<IActionResult> GetByTour(int tourId)
        => Ok(await _service.GetByTourIdAsync(tourId));


    [HttpPost]
    public async Task<IActionResult> Create(CreateTourLogDto dto)
    {
        try
        {
            return Ok(await _service.CreateAsync(dto));
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        return ok ? Ok() : NotFound();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateTourLogDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (updated == null)
            return NotFound();

        return Ok(updated);
    }
}