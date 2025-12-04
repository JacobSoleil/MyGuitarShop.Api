using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.Interfaces;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.MongoDb.Models;
using MyGuitarShop.Data.MongoDb.Services;

namespace MyGuitarShop.Api.MongoControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MongoSeedController(
        ILogger<MongoSeedController> logger,
        MongoProductService productService,
        IRepository<ProductEntity, int> productRepo) 
        : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SeedMongoFromSqlServer()
        {
            try
            {
                var allProductsFromSql = await productRepo.GetAllAsync();

                foreach (var product in allProductsFromSql)
                {
                    if (!await productService.InsertAsync(new ProductModel
                    {
                        CategoryID = product.CategoryID,
                        ProductCode = product.ProductCode,
                        ProductName = product.ProductName,
                        Description = product.Description,
                        ListPrice = product.ListPrice,
                        DiscountPercent = product.DiscountPercent,
                        DateAdded = product.DateAdded ?? DateTime.UtcNow
                    }))
                    throw new Exception($"Problems inserting\n{product}\n\nto Mongo");
                }

                return Ok("All Products inserted to MongoDB");
            }
            catch (Exception ex)
            {
                logger.LogError("Unable to insert all products to MongoDb.\n\nError {error}", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "Error inserting products to Mongo");
            }
        }
    }
}
