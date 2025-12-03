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
    public class AddressRepo(
        ILogger<AddressRepo> logger,
        SqlConnectionFactory connectionFactory)
        : IRepository<AddressDto, int>
    {
        public async Task<bool> DeleteAsync(int id)
        {
            const string query = @"DELETE FROM Addresses WHERE AddressID = @AddressID;";

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@AddressID", id);

                return await cmd.ExecuteNonQueryAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting address");
                throw;
            }
        }

        public async Task<AddressDto?> FindByIdAsync(int id)
        {
            const string query = @"SELECT * FROM Addresses WHERE AddressID = @AddressID;";

            AddressDto? address = null;

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@AddressID", id);

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    address = new AddressDto
                    {
                        AddressID = reader.GetInt32(reader.GetOrdinal("AddressID")),
                        CustomerID = reader.IsDBNull(reader.GetOrdinal("CustomerID")) ? null : reader.GetInt32(reader.GetOrdinal("CustomerID")),
                        Line1 = reader.GetString(reader.GetOrdinal("Line1")),
                        Line2 = reader.IsDBNull(reader.GetOrdinal("Line2")) ? null : reader.GetString(reader.GetOrdinal("Line2")),
                        City = reader.GetString(reader.GetOrdinal("City")),
                        State = reader.GetString(reader.GetOrdinal("State")),
                        ZipCode = reader.GetString(reader.GetOrdinal("ZipCode")),
                        Phone = reader.GetString(reader.GetOrdinal("Phone")),
                        Disabled = reader.GetInt32(reader.GetOrdinal("Disabled"))
                    };
                }
                else
                {
                    logger.LogError("Specified ID could not be found");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving address by ID");
                throw;
            }
            return address;
        }

        public async Task<IEnumerable<AddressDto>> GetAllAsync()
        {
            const string query = @"SELECT * FROM Addresses;";

            var addresses = new List<AddressDto>();

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                await using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var address = new AddressDto
                    {
                        AddressID = reader.GetInt32(reader.GetOrdinal("AddressID")),
                        CustomerID = reader.IsDBNull(reader.GetOrdinal("CustomerID")) ? null : reader.GetInt32(reader.GetOrdinal("CustomerID")),
                        Line1 = reader.GetString(reader.GetOrdinal("Line1")),
                        Line2 = reader.IsDBNull(reader.GetOrdinal("Line2")) ? null : reader.GetString(reader.GetOrdinal("Line2")),
                        City = reader.GetString(reader.GetOrdinal("City")),
                        State = reader.GetString(reader.GetOrdinal("State")),
                        ZipCode = reader.GetString(reader.GetOrdinal("ZipCode")),
                        Phone = reader.GetString(reader.GetOrdinal("Phone")),
                        Disabled = reader.GetInt32(reader.GetOrdinal("Disabled"))
                    };
                    addresses.Add(address);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving address list");
                throw;
            }
            return addresses;
        }

        public async Task<bool> InsertAsync(AddressDto dto)
        {
            const string query = @"INSERT INTO Addresses 
                    (CustomerID, Line1, Line2, City, State, ZipCode, Phone, Disabled) VALUES
                    (@CustomerID, @Line1, @Line2, @City, @State, @ZipCode, @Phone, @Disabled);";

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@CustomerID", dto.CustomerID);
                cmd.Parameters.AddWithValue("@Line1", dto.Line1);
                cmd.Parameters.AddWithValue("@Line2", dto.Line2);
                cmd.Parameters.AddWithValue("@City", dto.City);
                cmd.Parameters.AddWithValue("@State", dto.State);
                cmd.Parameters.AddWithValue("@ZipCode", dto.ZipCode);
                cmd.Parameters.AddWithValue("@Phone", dto.Phone);
                cmd.Parameters.AddWithValue("@Disabled", dto.Disabled);

                return await cmd.ExecuteNonQueryAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error inserting new address");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, AddressDto dto)
        {
            const string query = @"UPDATE Addresses 
                                    SET Line1 = @Line1, Line2 = @Line2, City = @City, State = @State, ZipCode = @ZipCode, Phone = @Phone, Disabled = @Disabled
                                    WHERE AddressID = @AddressID;";

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@AddressID", id);
                cmd.Parameters.AddWithValue("@Line1", dto.Line1);
                cmd.Parameters.AddWithValue("@Line2", dto.Line2);
                cmd.Parameters.AddWithValue("@City", dto.City);
                cmd.Parameters.AddWithValue("@State", dto.State);
                cmd.Parameters.AddWithValue("@ZipCode", dto.ZipCode);
                cmd.Parameters.AddWithValue("@Phone", dto.Phone);
                cmd.Parameters.AddWithValue("@Disabled", dto.Disabled);

                return await cmd.ExecuteNonQueryAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating address");
                throw;
            }

            throw new NotImplementedException();
        }
    }
}
