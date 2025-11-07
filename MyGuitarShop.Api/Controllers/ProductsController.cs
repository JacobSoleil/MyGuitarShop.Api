using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Data.Ado.Repositories;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.Ado.DTOMappers;

namespace MyGuitarShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(
        ILogger<ProductsController> logger,
        IRepository<ProductDto> repo)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var products = await repo.GetAllAsync();

                return Ok(products.Select(p => p.ProductName));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching Products");
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
                    return NotFound($"Product with id {id} not found");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving product with ID {ProductID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync(ProductDto newProduct)
        {
            try
            {
                var numberProductsCreated = await repo.InsertAsync(newProduct);

                return Ok($"{numberProductsCreated} products created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding new product with ID {ProductID}", newProduct.ProductID);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(int id, ProductDto updatedProduct)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Product with id {id} not found");

                var numberProductsCreated = await repo.UpdateAsync(id, updatedProduct);

                return Ok($"{numberProductsCreated} products created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating product with ID {ProductID}", updatedProduct.ProductID);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }

            throw new NotImplementedException();
        }
    }
}
