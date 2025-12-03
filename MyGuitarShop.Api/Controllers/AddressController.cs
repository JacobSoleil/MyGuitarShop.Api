using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Data.Ado.Repositories;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Common.Interfaces;

namespace MyGuitarShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController(
        ILogger<AddressController> logger,
        IRepository<AddressDto, int> repo)
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
                logger.LogError(ex.Message, "Error fetching addresses");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var address = await repo.FindByIdAsync(id);

                if (address == null)
                {
                    return NotFound();
                }
                return Ok(address);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving address with ID {AddressID}", id);
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
                logger.LogError(ex.Message, "Error adding new address with ID {AddressID}", newAddress.AddressID);
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

                var numberAddressesUpdated = await repo.UpdateAsync(id, updatedAddress);

                return Ok($"{numberAddressesUpdated} addresses updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating address with ID {AddressID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Address with id {id} not found");

                var numberAddressesDeleted = await repo.DeleteAsync(id);

                return Ok($"{numberAddressesDeleted} addresses deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting address with ID {AddressID}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
