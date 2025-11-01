using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Data.Ado.Repositories;
using MyGuitarShop.Data.Ado.Entities;

namespace MyGuitarShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController(
        ILogger<AddressController> logger,
        AddressRepo repo)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var addresses = await repo.GetAllAsync();

                return Ok(addresses.Select(p => p.AddressID));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching Addresses");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
