using Last.Simple.App.Domain.Contracts.Queries;
using Last.Simple.App.Domain.Contracts.Repositories;
using Last.Simple.App.Domain.Views;
using Microsoft.Data.SqlClient;

namespace Last.Simple.App.Infra.Queries
{
    public class ProductQuery : IProductQuery
    {
        private readonly ISQLConnectionBuilder connectionBuilder;
        private readonly IProductRepository productRepository;

        public ProductQuery(ISQLConnectionBuilder connectionBuilder, IProductRepository productRepository)
        {
            this.connectionBuilder = connectionBuilder;
            this.productRepository = productRepository;
        }

        public async Task<ProductView?> Get(long id)
        {
            var product  = await this.productRepository.Get(id);

            if (product == null)             
                return null;

            return new ProductView(product);            
        }

        public async Task<List<ProductView>> ListAll() 
        {
            using var conn = connectionBuilder.Build();

            var cmd = new SqlCommand("SELECT * FROM Products", conn);            

            conn.Open();
            var reader = await cmd.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                var products = new List<ProductView>();
                while (await reader.ReadAsync())
                {
                    var view = new ProductView();
                    view.Id = (long)reader["id"];
                    view.Name = reader["Name"].ToString() ?? string.Empty;
                    view.Price = (decimal)reader["Price"];
                    view.Description = reader["Description"].ToString();                    

                    products.Add(view);
                }

                return products;
            }


            return new List<ProductView>();
        }
    }
}
