using Microsoft.AspNetCore.Mvc;
using APBD07.Services;

namespace APBD07.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly IDatabaseService _databaseService;

    public TripsController(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    [HttpGet]
    public IActionResult GetTrips()
    {
        var trips = _databaseService.GetTrips();
        return Ok(trips);
    }
}