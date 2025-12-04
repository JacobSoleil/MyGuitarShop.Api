using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.MongoDb.Models;
using MyGuitarShop.Data.MongoDb.Services;

namespace MyGuitarShop.Api.MongoControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsMongoController(
        ILogger<ProductsMongoController> logger,
        MongoProductService productService) 
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var products = await productService.GetAllAsync();
                if (products.Count() != 0)
                    return Ok(products);
                return NotFound("No items found");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving products");

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            try
            {
                var product = await productService.FindByIdAsync(id);
                if (product == null)
                {
                    return NotFound($"Product with id {id} not found in MongoDB");
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
                var model = new ProductModel()
                {
                    CategoryID = newProduct.CategoryID, 
                    ProductCode = newProduct.ProductCode,
                    ProductName = newProduct.ProductName,
                    Description = newProduct.Description,
                    ListPrice = newProduct.ListPrice,
                    DiscountPercent = newProduct.DiscountPercent,
                    DateAdded = default
                };

                if (await productService.InsertAsync(model))
                    return Ok($"Product inserted");
                throw new Exception($"Unable to insert new product");
            }
            catch (Exception ex)
            {
                logger.LogError("Error adding new product.\n\nError: {message}", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(string id, ProductDto updatedProduct)
        {
            try
            {
                if (await productService.FindByIdAsync(id) == null)
                    return NotFound($"ProductID {id} not found");

                var entity = new ProductModel()
                {
                    _id = id,
                    ProductCode = updatedProduct.ProductCode,
                    ProductName = updatedProduct.ProductName,
                    Description = updatedProduct.Description,
                    ListPrice = updatedProduct.ListPrice,
                    DiscountPercent = updatedProduct.DiscountPercent,
                    DateAdded = default
                };

                var numberProductsUpdated = await productService.UpdateAsync(id, entity);

                return Ok($"{numberProductsUpdated} products updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating product with ID {ProductID}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(string id)
        {
            try
            {
                if (await productService.FindByIdAsync(id) == null)
                    return NotFound($"ProductID {id} not found");

                if (await productService.DeleteAsync(id))
                    return Ok($"ProductID {id} deleted");

                throw new Exception($"Unable to delete ProductID {id}");
            }
            catch (Exception ex)
            {
                logger.LogError("Error deleting ProductID {ProductID}\n\nError: {message}", id, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
