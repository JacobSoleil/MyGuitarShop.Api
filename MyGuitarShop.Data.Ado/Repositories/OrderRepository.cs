using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.Ado.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Repositories
{
    public class OrderRepo(
        ILogger<OrderRepo> logger,
        SqlConnectionFactory connectionFactory)
        : IRepository<OrderDto>
    {
        public async Task<int> DeleteAsync(int id)
        {
            const string query = @"DELETE FROM Orders WHERE OrderID = @OrderID;";

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@OrderID", id);

                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting order");
                throw;
            }
        }

        public async Task<OrderDto?> FindByIdAsync(int id)
        {
            const string query = @"SELECT * FROM Orders WHERE OrderID = @OrderID;";

            OrderDto? order = null;

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@OrderID", id);

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    order = new OrderDto
                    {
                        OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                        CustomerID = reader.IsDBNull(reader.GetOrdinal("CustomerID")) ? null : reader.GetInt32(reader.GetOrdinal("CustomerID")),
                        OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                        ShipAmount = reader.GetDecimal(reader.GetOrdinal("ShipAmount")),
                        TaxAmount = reader.GetDecimal(reader.GetOrdinal("TaxAmount")),
                        ShipDate = reader.IsDBNull(reader.GetOrdinal("ShipDate")) ? null : reader.GetDateTime(reader.GetOrdinal("ShipDate")),
                        ShipAddressID = reader.GetInt32(reader.GetOrdinal("ShipAddressID")),
                        CardType = reader.GetString(reader.GetOrdinal("CardType")),
                        CardNumber = reader.GetString(reader.GetOrdinal("CardNumber")),
                        CardExpires = reader.GetString(reader.GetOrdinal("CardExpires")),
                        BillingAddressID = reader.GetInt32(reader.GetOrdinal("BillingAddressID"))
                    };
                }
                else
                {
                    logger.LogError("Specified ID could not be found");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving order by ID");
                throw;
            }
            return order;
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            const string query = @"SELECT * FROM Orders;";

            var orders = new List<OrderDto>();

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                await using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var order = new OrderDto
                    {
                        OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                        CustomerID = reader.IsDBNull(reader.GetOrdinal("CustomerID")) ? null : reader.GetInt32(reader.GetOrdinal("CustomerID")),
                        OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                        ShipAmount = reader.GetDecimal(reader.GetOrdinal("ShipAmount")),
                        TaxAmount = reader.GetDecimal(reader.GetOrdinal("TaxAmount")),
                        ShipDate = reader.IsDBNull(reader.GetOrdinal("ShipDate")) ? null : reader.GetDateTime(reader.GetOrdinal("ShipDate")),
                        ShipAddressID = reader.GetInt32(reader.GetOrdinal("ShipAddressID")),
                        CardType = reader.GetString(reader.GetOrdinal("CardType")),
                        CardNumber = reader.GetString(reader.GetOrdinal("CardNumber")),
                        CardExpires = reader.GetString(reader.GetOrdinal("CardExpires")),
                        BillingAddressID = reader.GetInt32(reader.GetOrdinal("BillingAddressID"))
                    };
                    orders.Add(order);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving order list");
                throw;
            }
            return orders;
        }

        public async Task<int> InsertAsync(OrderDto dto)
        {
            const string query = @"INSERT INTO Orders 
                    (CustomerID, OrderDate, ShipAmount, TaxAmount, ShipDate, ShipAddressID, CardType, CardNumber, CardExpires, BillingAddressID) VALUES
                    (@CustomerID, @OrderDate, @ShipAmount, @TaxAmount, @ShipDate, @ShipAddressID, @CardType, @CardNumber, @CardExpires, @BillingAddressID);";

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@CustomerID", dto.CustomerID);
                cmd.Parameters.AddWithValue("@OrderDate", dto.OrderDate);
                cmd.Parameters.AddWithValue("@ShipAmount", dto.ShipAmount);
                cmd.Parameters.AddWithValue("@TaxAmount", dto.TaxAmount);
                cmd.Parameters.AddWithValue("@ShipDate", dto.ShipDate);
                cmd.Parameters.AddWithValue("@ShipAddressID", dto.ShipAddressID);
                cmd.Parameters.AddWithValue("@CardType", dto.CardType);
                cmd.Parameters.AddWithValue("@CardNumber", dto.CardNumber);
                cmd.Parameters.AddWithValue("@CardExpires", dto.CardExpires);
                cmd.Parameters.AddWithValue("@BillingAddressID", dto.BillingAddressID);

                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error inserting new order");
                throw;
            }
        }

        public async Task<int> UpdateAsync(int id, OrderDto dto)
        {
            const string query = @"UPDATE Orders 
                                    SET CustomerID = @CustomerID, OrderDate = @OrderDate, ShipAmount = @ShipAmount, TaxAmount = @TaxAmount, ShipDate = @ShipDate, ShipAddressID = @ShipAddressID, CardType = @CardType, CardNumber = @CardNumber, CardExpires = @CardExpires, BillingAddressID = @BillingAddressID
                                    WHERE OrderID = @OrderID;";

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@OrderID", id);
                cmd.Parameters.AddWithValue("@CustomerID", dto.CustomerID);
                cmd.Parameters.AddWithValue("@OrderDate", dto.OrderDate);
                cmd.Parameters.AddWithValue("@ShipAmount", dto.ShipAmount);
                cmd.Parameters.AddWithValue("@TaxAmount", dto.TaxAmount);
                cmd.Parameters.AddWithValue("@ShipDate", dto.ShipDate);
                cmd.Parameters.AddWithValue("@ShipAddressID", dto.ShipAddressID);
                cmd.Parameters.AddWithValue("@CardType", dto.CardType);
                cmd.Parameters.AddWithValue("@CardNumber", dto.CardNumber);
                cmd.Parameters.AddWithValue("@CardExpires", dto.CardExpires);
                cmd.Parameters.AddWithValue("@BillingAddressID", dto.BillingAddressID);

                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating order");
                throw;
            }

            throw new NotImplementedException();
        }
    }
}
