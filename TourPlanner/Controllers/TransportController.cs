using Microsoft.AspNetCore.Mvc;
using TourPlanner.Constants;

namespace TourPlanner.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransportController : ControllerBase
{
    [HttpGet]
    public IActionResult GetTransportTypes()
    {
        return Ok(TransportTypes.All);
    }
}