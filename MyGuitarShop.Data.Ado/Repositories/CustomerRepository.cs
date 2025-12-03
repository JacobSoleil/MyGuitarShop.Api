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
    public class CustomerRepo(
        ILogger<CustomerRepo> logger,
        SqlConnectionFactory connectionFactory)
        : IUniqueRepository<CustomerDto>
    {
        public async Task<bool> DeleteAsync(int id)
        {
            const string query = @"DELETE FROM Customers WHERE CustomerID = @CustomerID;";

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@CustomerID", id);

                return await cmd.ExecuteNonQueryAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting customer");
                throw;
            }
        }

        public async Task<CustomerDto?> FindByIdAsync(int id)
        {
            const string query = @"SELECT * FROM Customers WHERE CustomerID = @CustomerID;";

            CustomerDto? customer = null;

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@CustomerID", id);

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    customer = new CustomerDto
                    {
                        CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                        EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress")),
                        Password = reader.GetString(reader.GetOrdinal("Password")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        ShippingAddressID = reader.IsDBNull(reader.GetOrdinal("ShippingAddressID")) ? null : reader.GetInt32(reader.GetOrdinal("ShippingAddressID")),
                        BillingAddressID = reader.IsDBNull(reader.GetOrdinal("BillingAddressID")) ? null : reader.GetInt32(reader.GetOrdinal("BillingAddressID"))
                    };
                }
                else
                {
                    logger.LogError("Specified ID could not be found");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving customer by ID");
                throw;
            }
            return customer;
        }

        public async Task<CustomerDto?> FindByUniqueAsync(string ident)
        {
            const string query = @"SELECT * FROM Customers WHERE EmailAddress = @EmailAddress;";

            CustomerDto? customer = null;

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@EmailAddress", ident);

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    customer = new CustomerDto
                    {
                        CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                        EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress")),
                        Password = reader.GetString(reader.GetOrdinal("Password")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        ShippingAddressID = reader.IsDBNull(reader.GetOrdinal("ShippingAddressID")) ? null : reader.GetInt32(reader.GetOrdinal("ShippingAddressID")),
                        BillingAddressID = reader.IsDBNull(reader.GetOrdinal("BillingAddressID")) ? null : reader.GetInt32(reader.GetOrdinal("BillingAddressID"))
                    };
                }
                else
                {
                    logger.LogError("Specified identifier could not be found");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving customer by email address");
                throw;
            }
            return customer;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            const string query = @"SELECT * FROM Customers;";

            var customers = new List<CustomerDto>();

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                await using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var customer = new CustomerDto
                    {
                        CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                        EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress")),
                        Password = reader.GetString(reader.GetOrdinal("Password")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        ShippingAddressID = reader.IsDBNull(reader.GetOrdinal("ShippingAddressID")) ? null : reader.GetInt32(reader.GetOrdinal("ShippingAddressID")),
                        BillingAddressID = reader.IsDBNull(reader.GetOrdinal("BillingAddressID")) ? null : reader.GetInt32(reader.GetOrdinal("BillingAddressID"))
                    };
                    customers.Add(customer);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving customer list");
                throw;
            }
            return customers;
        }

        public async Task<bool> InsertAsync(CustomerDto dto)
        {
            const string query = @"INSERT INTO Customers 
                    (EmailAddress, Password, FirstName, LastName, ShippingAddressID, BillingAddressID) VALUES
                    (@EmailAddress, @Password, @FirstName, @LastName, @ShippingAddressID, @BillingAddressID);";

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@EmailAddress", dto.EmailAddress);
                cmd.Parameters.AddWithValue("@Password", dto.Password);
                cmd.Parameters.AddWithValue("@FirstName", dto.FirstName);
                cmd.Parameters.AddWithValue("@LastName", dto.LastName);
                cmd.Parameters.AddWithValue("@ShippingAddressID", dto.ShippingAddressID);
                cmd.Parameters.AddWithValue("@BillingAddressID", dto.BillingAddressID);

                return await cmd.ExecuteNonQueryAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error inserting new customer");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, CustomerDto dto)
        {
            const string query = @"UPDATE Customers 
                                    SET EmailAddress = @EmailAddress, Password = @Password, FirstName = @FirstName, LastName = @LastName, ShippingAddressID = @ShippingAddressID, BillingAddressID = @BillingAddressID
                                    WHERE CustomerID = @CustomerID;";

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@CustomerID", id);
                cmd.Parameters.AddWithValue("@EmailAddress", dto.EmailAddress);
                cmd.Parameters.AddWithValue("@Password", dto.Password);
                cmd.Parameters.AddWithValue("@FirstName", dto.FirstName);
                cmd.Parameters.AddWithValue("@LastName", dto.LastName);
                cmd.Parameters.AddWithValue("@ShippingAddressID", dto.ShippingAddressID);
                cmd.Parameters.AddWithValue("@BillingAddressID", dto.BillingAddressID);

                return await cmd.ExecuteNonQueryAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating customer");
                throw;
            }

            throw new NotImplementedException();
        }
    }
}
