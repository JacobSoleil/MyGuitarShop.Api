using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Common.Interfaces;

namespace MyGuitarShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(
        ILogger<CustomerController> logger,
        IUniqueRepository<CustomerDto> repo)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var customers = await repo.GetAllAsync();

                return Ok(customers.Select(p => p.CustomerID));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error fetching customers");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var customer = await repo.FindByIdAsync(id);

                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving customer with ID {CustomerID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("emailAddress/{ident}")]
        public async Task<IActionResult> GetByEmailAddressAsync(string ident)
        {
            try
            {
                var customer = await repo.FindByUniqueAsync(ident);

                if (customer == null)
                {
                    return NotFound($"Customer with identifier {ident} not found");
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving customer with identifier {EmailAddress}", ident);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CustomerDto newCustomer)
        {
            try
            {
                var numberCustomersCreated = await repo.InsertAsync(newCustomer);

                return Ok($"{numberCustomersCreated} customers created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error adding new customer with ID {CustomerID}", newCustomer.CustomerID);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerAsync(int id, CustomerDto updatedCustomer)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Customer with id {id} not found");

                var numberCustomersUpdated = await repo.UpdateAsync(id, updatedCustomer);

                return Ok($"{numberCustomersUpdated} customers updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating customer with ID {CustomerID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Customer with id {id} not found");

                var numberCustomersDeleted = await repo.DeleteAsync(id);

                return Ok($"{numberCustomersDeleted} customers deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting customer with ID {CustomerID}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
