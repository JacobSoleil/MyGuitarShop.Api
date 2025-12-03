using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Common.Interfaces;

namespace MyGuitarShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController(
        ILogger<OrderItemController> logger,
        IRepository<OrderItemDto, int> repo)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var items = await repo.GetAllAsync();

                return Ok(items.Select(p => p.ItemID));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error fetching order items");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var item = await repo.FindByIdAsync(id);

                if (item == null)
                {
                    return NotFound();
                }
                return Ok(item);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving order item with ID {ItemID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderItemAsync(OrderItemDto newItem)
        {
            try
            {
                var numberOrderItemsCreated = await repo.InsertAsync(newItem);

                return Ok($"{numberOrderItemsCreated} order items created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error adding new order item with ID {ItemID}", newItem.ItemID);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItemAsync(int id, OrderItemDto updatedItem)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Order item with id {id} not found");

                var numberOrderItemsUpdated = await repo.UpdateAsync(id, updatedItem);

                return Ok($"{numberOrderItemsUpdated} order items updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating order item with ID {ItemID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Order item with id {id} not found");

                var numberOrderItemsDeleted = await repo.DeleteAsync(id);

                return Ok($"{numberOrderItemsDeleted} order items deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting order item with ID {ItemID}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
