using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.Ado.Repositories;

namespace MyGuitarShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(
        ILogger<CategoryController> logger,
        IRepository<CategoryDto> repo)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var categories = await repo.GetAllAsync();

                return Ok(categories.Select(p => p.CategoryID));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error fetching categories");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var category = await repo.FindByIdAsync(id);

                if (category == null)
                {
                    return NotFound();
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving category with ID {CategoryID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync(CategoryDto newCategory)
        {
            try
            {
                var numberCategoriesCreated = await repo.InsertAsync(newCategory);

                return Ok($"{numberCategoriesCreated} categories created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error adding new category with ID {CategoryID}", newCategory.CategoryID);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryAsync(int id, CategoryDto updatedCategory)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Category with id {id} not found");

                var numberCategoriesUpdated = await repo.UpdateAsync(id, updatedCategory);

                return Ok($"{numberCategoriesUpdated} categories updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating category with ID {CategoryID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Category with id {id} not found");

                var numberCategoriesDeleted = await repo.DeleteAsync(id);

                return Ok($"{numberCategoriesDeleted} categories deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting category with ID {CategoryID}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
