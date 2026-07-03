using Microsoft.AspNetCore.Mvc;
using TourPlanner.DTOs;
using TourPlanner.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace TourPlanner.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ToursController : ControllerBase
{
    private readonly ITourService _service;

    public ToursController(ITourService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tour = await _service.GetByIdAsync(id);
        return tour == null ? NotFound() : Ok(tour);
    }

    /*[HttpPost]
    public async Task<IActionResult> Create(CreateTourDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return Ok(created);
    }*/

    [HttpPost]
    public async Task<IActionResult> Create(CreateTourDto dto)
    {
        try
        {
            var created = await _service.CreateAsync(dto);
            return Ok(created);
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                error = ex.Message
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        return ok ? Ok() : NotFound();
    }

    /*[HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateTourDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated ? Ok() : NotFound();
    }*/

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateTourDto dto)
    {
        try
        {
            var updated = await _service.UpdateAsync(id, dto);

            return updated
                ? Ok()
                : NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                error = ex.Message
            });
        }
    }
}