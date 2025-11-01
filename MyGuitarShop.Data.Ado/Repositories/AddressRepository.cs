using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Factories;

namespace MyGuitarShop.Data.Ado.Repositories
{
    public class AddressRepo(
        ILogger<AddressRepo> logger,
        SqlConnectionFactory connectionFactory)
        : IRepository<AddressEntity>
    {
        public async Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<AddressEntity?> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AddressEntity>> GetAllAsync()
        {
            var addresses = new List<AddressEntity>();

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand("SELECT * FROM Addresses", conn);

                await using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var address = new AddressEntity
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
                logger.LogError(ex.Message, "Error retrieving product list");
            }
            return addresses;
        }

        public async Task<int> InsertAsync(AddressEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAsync(int id, AddressEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
