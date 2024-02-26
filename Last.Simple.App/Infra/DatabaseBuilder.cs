using Microsoft.Data.SqlClient;

namespace Last.Simple.App.Infra
{
    public class DatabaseBuilder
    {
        private readonly IConfiguration configuration;

        public DatabaseBuilder(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task Build()
        {
            var dbConnectionString = configuration.GetConnectionString("SimpleDBConnection");            

            var builder = new SqlConnectionStringBuilder(dbConnectionString);

            var connectionString = $"Server={builder.DataSource};User Id={builder.UserID};Password={builder.Password};";
            if (builder.IntegratedSecurity)
                connectionString = $"Server={builder.DataSource};Integrated Security=true;;TrustServerCertificate=true";

            using var sqlConnection = new SqlConnection(connectionString);
            using var cmd = new SqlCommand($"SELECT database_id FROM sys.databases WHERE Name = '{builder.InitialCatalog}'", sqlConnection);

            sqlConnection.Open();

            var databaseId = cmd.ExecuteScalar();
            if (databaseId == null)
            {
                using var createDbCmd = new SqlCommand($"CREATE DATABASE [{builder.InitialCatalog}]", sqlConnection);
                createDbCmd.ExecuteNonQuery();

                await CreateTables();                
            }
        }        

        private async Task CreateTables()
        {
            using var sqlConnection = new SqlConnection(configuration.GetConnectionString("SimpleDBConnection"));
            using var cmd = new SqlCommand(GetRawSql(), sqlConnection);

            sqlConnection.Open();

            await cmd.ExecuteNonQueryAsync();
        }

        public static string GetRawSql()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Infra", "DB.sql");            
            var sqlText = File.ReadAllText(path);

            return sqlText;
        }
    }
}
