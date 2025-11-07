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
    public class AddressRepo(
        ILogger<AddressRepo> logger,
        SqlConnectionFactory connectionFactory)
        : IRepository<AddressDto>
    {
        public async Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<AddressDto?> FindByIdAsync(int id)
        {
            AddressDto? address = null;

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand("SELECT * FROM Addresses WHERE AddressID = " + id.ToString() + ";", conn);

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
            }
            return address;
        }

        public async Task<IEnumerable<AddressDto>> GetAllAsync()
        {
            var addresses = new List<AddressDto>();

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand("SELECT * FROM Addresses", conn);

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
            }
            return addresses;
        }

        public async Task<int> InsertAsync(AddressDto dto)
        {
            const string query = @"INSERT INTO Addresses 
                    (CustomerID, Line1, Line2, City, State, ZipCode, Phone, Disabled) VALUES
                    (@CustomerID, @Line1, @Line2, @City, @State, @ZipCode, @Phone, @Disabled)";

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

                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error inserting new address");
                return 0;
            }
        }

        public async Task<int> UpdateAsync(int id, AddressDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
