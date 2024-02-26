using Last.Simple.App.Domain.Contracts.Repositories;
using Last.Simple.App.Domain.CoreDomain;
using Last.Simple.App.Domain.DataObjects;
using Microsoft.Data.SqlClient;

namespace Last.Simple.App.Infra.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ISQLConnectionBuilder connectionBuilder;

        public ProductRepository(ISQLConnectionBuilder connectionBuilder)
        {
            this.connectionBuilder = connectionBuilder;
        }

        public async Task<long> AddProduct(Product product)
        {
            using var conn = connectionBuilder.Build();
            
            var cmd = new SqlCommand("INSERT INTO Products (Name, Price, Description, CreatedBy) output INSERTED.ID VALUES (@Name, @Price, @Description, @CreatedById)", conn);
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@Description", product.Description);
            cmd.Parameters.AddWithValue("@CreatedById", product.CreatedById);
            
            conn.Open();
            var id = await cmd.ExecuteScalarAsync();

            return (long)id;
        }

        public async Task<Product?> Get(long id) 
        {
            using var conn = connectionBuilder.Build();

            var cmd = new SqlCommand("SELECT * FROM Products WHERE ID = @ID", conn);
            cmd.Parameters.AddWithValue("@ID", id);

            conn.Open();
            return await GetProductDomain(cmd);
        }

        public async Task Update(Product product)
        {
            using var conn = connectionBuilder.Build();

            var cmd = new SqlCommand("UPDATE Products SET Price = @Price, Description = @Description WHERE ID = @ID", conn);
            cmd.Parameters.AddWithValue("@ID", product.Id);            
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@Description", product.Description);

            conn.Open();
            await cmd.ExecuteNonQueryAsync();
        }

        private async Task<Product?> GetProductDomain(SqlCommand cmd)
        {
            var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var dataObject = new ProductDO()
                {
                    Id = (long)reader["id"],
                    Name = reader["Name"].ToString() ?? string.Empty,
                    Price = (decimal)reader["Price"],
                    Description = reader["Description"].ToString(),
                    CreatedById = (long)reader["CreatedBy"]
                };

                return Product.FromDataObject(dataObject);
            }

            return null;
        }
    }
}
