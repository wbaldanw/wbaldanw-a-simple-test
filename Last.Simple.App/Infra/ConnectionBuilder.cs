using Microsoft.Data.SqlClient;

namespace Last.Simple.App.Infra
{
    public class SQLConnectionBuilder : ISQLConnectionBuilder
    {
        private readonly IConfiguration configuration;

        public SQLConnectionBuilder(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public SqlConnection Build() => new SqlConnection(configuration.GetConnectionString("SimpleDBConnection"));
    }

    public interface ISQLConnectionBuilder
    {
        SqlConnection Build();
    }
}
