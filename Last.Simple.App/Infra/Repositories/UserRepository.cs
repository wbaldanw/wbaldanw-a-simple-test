using Last.Simple.App.Domain.Contracts.Repositories;
using Last.Simple.App.Domain.CoreDomain;
using Last.Simple.App.Domain.DataObjects;
using Microsoft.Data.SqlClient;

namespace Last.Simple.App.Infra.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ISQLConnectionBuilder connectionBuilder;

        public UserRepository(ISQLConnectionBuilder connectionBuilder)
        {
            this.connectionBuilder = connectionBuilder;
        }

        public async Task<long> AddUser(User user)
        {
            using var conn = connectionBuilder.Build();
            
            var cmd = new SqlCommand("INSERT INTO Users (UserName, Password) output INSERTED.ID VALUES (@UserName, @Password)", conn);
            cmd.Parameters.AddWithValue("@UserName", user.UserName);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            
            conn.Open();
            var id = await cmd.ExecuteScalarAsync();

            return (long)id;
        }

        public async Task<User?> GetByUserName(string userName)
        {
            using var conn = connectionBuilder.Build();

            var cmd = new SqlCommand("SELECT * FROM Users WHERE UserName = @UserName", conn);
            cmd.Parameters.AddWithValue("@UserName", userName);

            conn.Open();
            return await GetUserDomain(cmd);
        }

        private static async Task<User?> GetUserDomain(SqlCommand cmd)
        {
            var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return User.FromDataObject(new UserDO
                {
                    Id = reader.GetInt64(0),
                    UserName = reader.GetString(1),
                    Password = reader.GetString(2)
                });
            }

            return null;
        }
    }
}
