using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Factories;
using MyGuitarShop.Common.Interfaces;
using MyGuitarShop.Common.DTOs;

namespace MyGuitarShop.Data.Ado.Repositories
{
    public class ProductRepoMongo(
        ILogger<ProductRepoMongo> logger,
        SqlConnectionFactory connectionFactory)
        : IRepository<ProductEntity, int>
    {
        public async Task<IEnumerable<ProductEntity>> GetAllAsync()
        {
            const string query = "SELECT * FROM Products;";

            var products = new List<ProductEntity>();

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                await using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var product = new ProductEntity
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
                throw;
            }
            return products;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string query = @"DELETE FROM Products WHERE ProductID = @ProductID;";

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@ProductID", id);

                return await cmd.ExecuteNonQueryAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting product");
                throw;
            }
        }

        public async Task<ProductEntity?> FindByIdAsync(int id)
        {
            const string query = @"SELECT * FROM Products WHERE ProductID = @ProductID;";

            ProductEntity? product = null;

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@ProductID", id);

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    product = new ProductEntity
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
                throw;
            }
            return product;
        }

        public async Task<bool> InsertAsync(ProductEntity dto)
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

                return await cmd.ExecuteNonQueryAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error inserting new product");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, ProductEntity dto)
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

                return await cmd.ExecuteNonQueryAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating product");
                throw;
            }
        }

        public async Task<ProductDto?> FindByUniqueAsync(string ident)
        {
            const string query = @"SELECT * FROM Products WHERE ProductCode = @ProductCode;";

            ProductDto? product = null;

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@ProductCode", ident);

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
                    logger.LogError("Specified identifier could not be found");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving product by product code");
                throw;
            }
            return product;
        }
    }
}
