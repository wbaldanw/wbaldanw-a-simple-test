using Last.Simple.App.Infra;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Last.Simple.Tests.Utils
{
    public class SqlConnectionBuilderForTests : ISQLConnectionBuilder
    {
        public SqlConnectionBuilderForTests()
        {
            var dbName = Guid.NewGuid().ToString();
            ConnectionString = $"Server=(localdb)\\mssqllocaldb;Database={dbName};Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False";

            ConfigurationMock = new Mock<IConfiguration>();

            var mockConfSection = new Mock<IConfigurationSection>();
            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "SimpleDBConnection")]).Returns(ConnectionString);
            ConfigurationMock.Setup(a => a.GetSection(It.Is<string>(s => s == "ConnectionStrings"))).Returns(mockConfSection.Object);
        }

        public string ConnectionString { get; private set; }

        public Mock<IConfiguration> ConfigurationMock { get; private set; }

        public SqlConnection Build()
        {
            var dbBuilder = new DatabaseBuilder(ConfigurationMock.Object);
            Task.WaitAll(dbBuilder.Build());

            Task.WaitAll(SeedData(ConnectionString));

            return new SqlConnection(ConnectionString);
        }

        private async Task SeedData(string connectionString)
        {
            var seedData = new SeedData(connectionString);
            await seedData.Seed();            
        }
    }
}
