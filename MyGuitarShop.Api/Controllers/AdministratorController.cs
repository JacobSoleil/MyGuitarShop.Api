using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Common.Interfaces; 

namespace MyGuitarShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController(
        ILogger<AdministratorController> logger,
        IRepository<AdministratorDto, int> repo)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var admins = await repo.GetAllAsync();

                return Ok(admins.Select(p => p.AdminID));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error fetching administrators");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var admin = await repo.FindByIdAsync(id);
                if (admin == null)
                {
                    return NotFound();
                }
                return Ok(admin);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving administrator with ID {AdminID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdminAsync(AdministratorDto newAdmin)
        {
            try
            {
                var numberAdminsCreated = await repo.InsertAsync(newAdmin);

                return Ok($"{numberAdminsCreated} administrators created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error adding new administrator with ID {AdminID}", newAdmin.AdminID);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdminAsync(int id, AdministratorDto updatedAdmin)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Administrator with id {id} not found");

                var numberAdminsUpdated = await repo.UpdateAsync(id, updatedAdmin);

                return Ok($"{numberAdminsUpdated} administrators updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating administrator with ID {AdminID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Administrator with id {id} not found");

                var numberAdminsDeleted = await repo.DeleteAsync(id);

                return Ok($"{numberAdminsDeleted} administrators deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting administrator with ID {AdminID}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
