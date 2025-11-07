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
    public class AdministratorRepo(
        ILogger<AdministratorRepo> logger,
        SqlConnectionFactory connectionFactory)
        : IRepository<AdministratorDto>
    {
        public async Task<int> DeleteAsync(int id)
        {
            const string query = @"DELETE FROM Administrators WHERE AdminID = @AdminID;";

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@AdminID", id);

                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting administrator");
                throw;
            }
        }

        public async Task<AdministratorDto?> FindByIdAsync(int id)
        {
            const string query = @"SELECT * FROM Administrators WHERE AdminID = @AdminID;";

            AdministratorDto ? admin = null;

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@AdminID", id);

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    admin = new AdministratorDto
                    {
                        AdminID = reader.GetInt32(reader.GetOrdinal("AdminID")),
                        EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress")),
                        Password = reader.GetString(reader.GetOrdinal("Password")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName"))
                    };
                }
                else
                {
                    logger.LogError("Specified ID could not be found");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving administrator by ID");
                throw;
            }
            return admin;
        }

        public async Task<IEnumerable<AdministratorDto>> GetAllAsync()
        {
            const string query = @"SELECT * FROM Administrators;";

            var adminList = new List<AdministratorDto>();

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                await using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var admin = new AdministratorDto
                    {
                        AdminID = reader.GetInt32(reader.GetOrdinal("AdminID")),
                        EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress")),
                        Password = reader.GetString(reader.GetOrdinal("Password")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName"))
                    };
                    adminList.Add(admin);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving administrator list");
                throw;
            }
            return adminList;
        }

        public async Task<int> InsertAsync(AdministratorDto dto)
        {
            const string query = @"INSERT INTO Administrators 
                    (EmailAddress, Password, FirstName, LastName) VALUES
                    (@EmailAddress, @Password, @FirstName, @LastName);";

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@AdminID", dto.AdminID);
                cmd.Parameters.AddWithValue("@EmailAddress", dto.EmailAddress);
                cmd.Parameters.AddWithValue("@Password", dto.Password);
                cmd.Parameters.AddWithValue("@FirstName", dto.FirstName);
                cmd.Parameters.AddWithValue("@LastName", dto.LastName);

                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error inserting new administrator");
                throw;
            }
        }

        public async Task<int> UpdateAsync(int id, AdministratorDto dto)
        {
            const string query = @"UPDATE Administrators 
                                    SET EmailAddress = @EmailAddress, EmailAddress = @Password, FirstName = @FirstName, LastName = @LastName
                                    WHERE AdminID = @AdminID;";

            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@AdminID", dto.AdminID);
                cmd.Parameters.AddWithValue("@EmailAddress", dto.EmailAddress);
                cmd.Parameters.AddWithValue("@Password", dto.Password);
                cmd.Parameters.AddWithValue("@FirstName", dto.FirstName);
                cmd.Parameters.AddWithValue("@LastName", dto.LastName);

                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating administrator");
                throw;
            }

            throw new NotImplementedException();
        }
    }
}
