using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Factories;
using MyGuitarShop.Common.DTOs;

namespace MyGuitarShop.Data.Ado.Repositories
{
    public class ProductRepo(
        ILogger<ProductRepo> logger,
        SqlConnectionFactory connectionFactory)
        : IRepository<ProductDto>
    {
        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = new List<ProductDto>();

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand("SELECT * FROM Products", conn);

                await using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var product = new ProductDto
                    {
                        ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                        CategoryID = reader.IsDBNull(reader.GetOrdinal("CategoryID")) ? null : reader.GetInt32(reader.GetOrdinal("ProductID")),
                        ProductCode = reader.GetString(reader.GetOrdinal("ProductCode")),
                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        ListPrice = reader.GetDecimal(reader.GetOrdinal("ListPrice")),
                        DiscountPercent = reader.GetDecimal(reader.GetOrdinal("DiscountPercent")),
                        DateAdded = reader.IsDBNull(reader.GetOrdinal("DateAdded")) ? null : reader.GetDateTime(reader.GetOrdinal("DateAdded"))
                    };
                    products.Add(product);
                }
            }
            catch (Exception ex) 
            {
                logger.LogError(ex.Message, "Error retrieving product list");
            }
            return products;
        }

        public async Task<int> DeleteAsync(int id)
        {
            const string query = @"DELETE FROM Products WHERE ProductID = @ProductID;";

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@ProductID", id);

                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting product");
                return 0;
            }
        }

        public async Task<ProductDto?> FindByIdAsync(int id)
        {
            ProductDto? product = null;

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand("SELECT * FROM Products WHERE ProductID = " + id.ToString() + ";", conn);

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    product = new ProductDto
                    {
                        ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                        CategoryID = reader.IsDBNull(reader.GetOrdinal("CategoryID")) ? null : reader.GetInt32(reader.GetOrdinal("CategoryID")),
                        ProductCode = reader.GetString(reader.GetOrdinal("ProductCode")),
                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        ListPrice = reader.GetDecimal(reader.GetOrdinal("ListPrice")),
                        DiscountPercent = reader.GetDecimal(reader.GetOrdinal("DiscountPercent")),
                        DateAdded = reader.IsDBNull(reader.GetOrdinal("DateAdded")) ? null : reader.GetDateTime(reader.GetOrdinal("DateAdded"))
                    };
                }
                else
                {
                    logger.LogError("Specified ID could not be found");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving product by ID");
            }
            return product;
        }

        public async Task<int> InsertAsync(ProductDto dto)
        {
            const string query = @"INSERT INTO Products 
                    (CategoryID, ProductCode, ProductName, Description, ListPrice, DiscountPercent, DateAdded) VALUES
                    (@CategoryID, @ProductCode, @ProductName, @Description, @ListPrice, @DiscountPercent, GETDATE());";

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@CategoryID", dto.CategoryID);
                cmd.Parameters.AddWithValue("@ProductCode", dto.ProductCode);
                cmd.Parameters.AddWithValue("@ProductName", dto.ProductName);
                cmd.Parameters.AddWithValue("@Description", dto.Description);
                cmd.Parameters.AddWithValue("@ListPrice", dto.ListPrice);
                cmd.Parameters.AddWithValue("@DiscountPercent", dto.DiscountPercent);

                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error inserting new product");
                return 0;
            }
        }

        public async Task<int> UpdateAsync(int id, ProductDto dto)
        {
            const string query = @"UPDATE Products 
                                    SET CategoryID = @CategoryID, ProductCode = @ProductCode, ProductName = @ProductName, Description = @Description, ListPrice = @ListPrice, DiscountPercent = @DiscountPercent
                                    WHERE ProductID = @ProductID;";

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@ProductID", id);
                cmd.Parameters.AddWithValue("@CategoryID", dto.CategoryID);
                cmd.Parameters.AddWithValue("@ProductCode", dto.ProductCode);
                cmd.Parameters.AddWithValue("@ProductName", dto.ProductName);
                cmd.Parameters.AddWithValue("@Description", dto.Description);
                cmd.Parameters.AddWithValue("@ListPrice", dto.ListPrice);
                cmd.Parameters.AddWithValue("@DiscountPercent", dto.DiscountPercent);

                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating product");
                return 0;
            }
        }
    }
}
