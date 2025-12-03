using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Common.Interfaces;

namespace MyGuitarShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(
        ILogger<OrderController> logger,
        IRepository<OrderDto, int> repo)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var orders = await repo.GetAllAsync();

                return Ok(orders.Select(p => p.OrderID));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error fetching orders");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var order = await repo.FindByIdAsync(id);

                if (order == null)
                {
                    return NotFound();
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving order with ID {OrderID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync(OrderDto newOrder)
        {
            try
            {
                var numberOrdersCreated = await repo.InsertAsync(newOrder);

                return Ok($"{numberOrdersCreated} rows affected");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error adding new order with ID {OrderID}", newOrder.OrderID);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderAsync(int id, OrderDto updatedOrder)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Order with id {id} not found");

                var numberOrdersUpdated = await repo.UpdateAsync(id, updatedOrder);

                return Ok($"{numberOrdersUpdated} orders updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating order with ID {OrderID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Order with id {id} not found");

                var numberOrdersDeleted = await repo.DeleteAsync(id);

                return Ok($"{numberOrdersDeleted} orders deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting order with ID {OrderID}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
