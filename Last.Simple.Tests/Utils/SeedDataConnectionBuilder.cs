using Last.Simple.App.Infra;
using Microsoft.Data.SqlClient;

namespace Last.Simple.Tests.Utils
{
    public class SeedDataConnectionBuilder : ISQLConnectionBuilder
    {
        private readonly string connectionString;

        public SeedDataConnectionBuilder(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public SqlConnection Build() => new SqlConnection(connectionString);
    }
}
