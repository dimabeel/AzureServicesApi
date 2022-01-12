using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AzureServices.Models;

namespace AzureServicesAPI.Controllers;

[ApiController]
[Route("cars")]
[Authorize]
public class CarsController : ControllerBase
{
    // In-memory garage
    private static readonly Dictionary<int, Car> carsGarage = new Dictionary<int, Car>();

    private readonly ILogger<CarsController> _logger;
    private readonly IHttpContextAccessor _contextAccessor;

    public CarsController(ILogger<CarsController> logger, IHttpContextAccessor contextAccessor)
    {
        _logger = logger;
        _contextAccessor = contextAccessor;
    }

    [HttpGet("{id}")]
    public ActionResult<Car> Get(int id)
    {
        if (carsGarage.ContainsKey(id))
        {
            return Ok(carsGarage[id]);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpGet("all")]
    public ActionResult<Car[]> GetAll()
    {
        if (carsGarage.Count > 0)
        {
            return Ok(carsGarage.Select(x => x.Value).ToArray());
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost("create")]
    public ActionResult<Car> Create(Car car)
    {
        int id = carsGarage.Count + 1;
        carsGarage.Add(id, car);
        return Ok(carsGarage[id]);
    }

    [HttpPut("update/{id}")]
    public ActionResult<Car> Update(int id, Car car)
    {
        if (carsGarage.ContainsKey(id))
        {
            carsGarage[id] = car;
            return Ok(carsGarage[id]);
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        if (carsGarage.ContainsKey(id))
        {
            carsGarage.Remove(id);
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpDelete("delete-all")]
    public ActionResult DeleteAll()
    {
        if (carsGarage.Count > 0)
        {
            carsGarage.Clear();
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}
