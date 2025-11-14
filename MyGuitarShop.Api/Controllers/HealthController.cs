using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyGuitarShop.Data.Ado.Factories;
using MyGuitarShop.Data.EFCore.Context;
using System.Threading.Tasks;

namespace MyGuitarShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController
        (ILogger<HealthController> logger,
        SqlConnectionFactory sqlConnectionFactory,
        MyGuitarShopContext dbContext)
        : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok("Healthy");
            }
            catch (Exception)
            {
                logger.LogWarning("Health check failed unreasonably.");

                return StatusCode(503, "Unhealthy");
            }
        }

        [HttpGet("db/ado")]
        public IActionResult GetDbHealth() 
        {
            try
            {
                using var connection = sqlConnectionFactory.OpenSqlConnection();

                return Ok(new { Message = "Connection successful!", connection.Database });
            }
            catch (Exception e)
            {
                logger.LogCritical("Database health check failed with error message:\n"+e.Message);

                return StatusCode(503, "Database Unhealthy");
            }
        }

        [HttpGet("db/efcore")]
        public async Task<IActionResult> GetDbContextHealthAsync()
        {
            try
            {
                if (!await dbContext.Database.CanConnectAsync())
                    throw new Exception("Cannot conncect to database via EF Core DbContext.");

                return Ok(new { Message = "Connection successful!", dbContext.Database.GetDbConnection().Database });
            }
            catch (Exception e)
            {
                logger.LogCritical("Database health check failed with error message:\n" + e.Message);

                return StatusCode(503, "Database Unhealthy");
            }
        }
    }
}
