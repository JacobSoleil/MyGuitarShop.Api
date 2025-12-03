using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.Ado.Factories;
using MyGuitarShop.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Repositories
{
    public class OrderItemRepo(
        ILogger<OrderItemRepo> logger,
        SqlConnectionFactory connectionFactory)
        : IRepository<OrderItemDto, int>
    {
        public async Task<bool> DeleteAsync(int id)
        {
            const string query = @"DELETE FROM OrderItems WHERE ItemID = @ItemID;";

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@ItemID", id);

                return await cmd.ExecuteNonQueryAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting order item");
                throw;
            }
        }

        public async Task<OrderItemDto?> FindByIdAsync(int id)
        {
            const string query = @"SELECT * FROM OrderItems WHERE ItemID = @ItemID;";

            OrderItemDto? item = null;

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@ItemID", id);

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    item = new OrderItemDto
                    {
                        ItemID = reader.GetInt32(reader.GetOrdinal("ItemID")),
                        OrderID = reader.IsDBNull(reader.GetOrdinal("OrderID")) ? null : reader.GetInt32(reader.GetOrdinal("OrderID")),
                        ProductID = reader.IsDBNull(reader.GetOrdinal("ProductID")) ? null : reader.GetInt32(reader.GetOrdinal("ProductID")),
                        ItemPrice = reader.GetDecimal(reader.GetOrdinal("ItemPrice")),
                        DiscountAmount = reader.GetDecimal(reader.GetOrdinal("DiscountAmount")),
                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                    };
                }
                else
                {
                    logger.LogError("Specified ID could not be found");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving order item by ID");
                throw;
            }
            return item;
        }

        public async Task<IEnumerable<OrderItemDto>> GetAllAsync()
        {
            const string query = @"SELECT * FROM OrderItems;";

            var items = new List<OrderItemDto>();

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                await using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var item = new OrderItemDto
                    {
                        ItemID = reader.GetInt32(reader.GetOrdinal("ItemID")),
                        OrderID = reader.IsDBNull(reader.GetOrdinal("OrderID")) ? null : reader.GetInt32(reader.GetOrdinal("OrderID")),
                        ProductID = reader.IsDBNull(reader.GetOrdinal("ProductID")) ? null : reader.GetInt32(reader.GetOrdinal("ProductID")),
                        ItemPrice = reader.GetDecimal(reader.GetOrdinal("ItemPrice")),
                        DiscountAmount = reader.GetDecimal(reader.GetOrdinal("DiscountAmount")),
                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                    };
                    items.Add(item);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving order item list");
                throw;
            }
            return items;
        }

        public async Task<bool> InsertAsync(OrderItemDto dto)
        {
            const string query = @"INSERT INTO OrderItems 
                    (OrderID, ProductID, ItemPrice, DiscountAmount, Quantity) VALUES
                    (@OrderID, @ProductID, @ItemPrice, @DiscountAmount, @Quantity);";

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@OrderID", dto.OrderID);
                cmd.Parameters.AddWithValue("@ProductID", dto.ProductID);
                cmd.Parameters.AddWithValue("@ItemPrice", dto.ItemPrice);
                cmd.Parameters.AddWithValue("@DiscountAmount", dto.DiscountAmount);
                cmd.Parameters.AddWithValue("@Quantity", dto.Quantity);

                return await cmd.ExecuteNonQueryAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error inserting new order item");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, OrderItemDto dto)
        {
            const string query = @"UPDATE OrderItems 
                                    SET OrderID = @OrderID, ProductID = @ProductID, ItemPrice = @ItemPrice, DiscountAmount = @DiscountAmount, Quantity = @Quantity
                                    WHERE ItemID = @ItemID;";

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@ItemID", id);
                cmd.Parameters.AddWithValue("@OrderID", dto.OrderID);
                cmd.Parameters.AddWithValue("@ProductID", dto.ProductID);
                cmd.Parameters.AddWithValue("@ItemPrice", dto.ItemPrice);
                cmd.Parameters.AddWithValue("@DiscountAmount", dto.DiscountAmount);
                cmd.Parameters.AddWithValue("@Quantity", dto.Quantity);

                return await cmd.ExecuteNonQueryAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating order item");
                throw;
            }

            throw new NotImplementedException();
        }
    }
}
