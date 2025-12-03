using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.Ado.Factories;
using MyGuitarShop.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Repositories
{
    public class CategoryRepo(
        ILogger<CategoryRepo> logger,
        SqlConnectionFactory connectionFactory)
        : IUniqueRepository<CategoryDto>
    {
        public async Task<bool> DeleteAsync(int id)
        {
            const string query = @"DELETE FROM Categories WHERE CategoryID = @CategoryID;";

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@CategoryID", id);

                return await cmd.ExecuteNonQueryAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting category");
                throw;
            }
        }

        public async Task<CategoryDto?> FindByIdAsync(int id)
        {
            const string query = @"SELECT * FROM Categories WHERE CategoryID = @CategoryID;";

            CategoryDto? category = null;

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@CategoryID", id);

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    category = new CategoryDto
                    {
                        CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                        CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"))
                    };
                }
                else
                {
                    logger.LogError("Specified ID could not be found");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving category by ID");
                throw;
            }
            return category;
        }

        public async Task<CategoryDto?> FindByUniqueAsync(string ident)
        {
            const string query = @"SELECT * FROM Categories WHERE CategoryName = @CategoryName;";

            CategoryDto? category = null;

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@CategoryName", ident);

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    category = new CategoryDto
                    {
                        CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                        CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"))
                    };
                }
                else
                {
                    logger.LogError("Specified identifier could not be found");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving category by category name");
                throw;
            }
            return category;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            const string query = @"SELECT * FROM Categories;";

            var categories = new List<CategoryDto>();

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                await using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var category = new CategoryDto
                    {
                        CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                        CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"))
                    };
                    categories.Add(category);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving category list");
                throw;
            }
            return categories;
        }

        public async Task<bool> InsertAsync(CategoryDto dto)
        {
            const string query = @"INSERT INTO Categories 
                    (CategoryName) VALUES
                    (@CategoryName);";

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@CategoryName", dto.CategoryName);

                return await cmd.ExecuteNonQueryAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error inserting new category");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, CategoryDto dto)
        {
            const string query = @"UPDATE Categories 
                                    SET CategoryName = @CategoryName
                                    WHERE CategoryID = @CategoryID;";

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@CategoryID", id);
                cmd.Parameters.AddWithValue("@CategoryName", dto.CategoryName);

                return await cmd.ExecuteNonQueryAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating categories");
                throw;
            }

            throw new NotImplementedException();
        }
    }
}
