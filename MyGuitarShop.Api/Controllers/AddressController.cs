using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Data.Ado.Repositories;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Common.DTOs;

namespace MyGuitarShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController(
        ILogger<AddressController> logger,
        IRepository<AddressDto> repo)
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var product = await repo.FindByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving address with ID {AddressID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddressAsync(AddressDto newAddress)
        {
            try
            {
                var numberAddressessCreated = await repo.InsertAsync(newAddress);

                return Ok($"{numberAddressessCreated} addresses created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding new address with ID {AddressID}", newAddress.AddressID);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddressAsync(int id, AddressDto updatedAddress)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Address with id {id} not found");

                var numberAddressesCreated = await repo.UpdateAsync(id, updatedAddress);

                return Ok($"{numberAddressesCreated} products created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating address with ID {AddressID}", updatedAddress.AddressID);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }

            throw new NotImplementedException();
        }
    }
}
