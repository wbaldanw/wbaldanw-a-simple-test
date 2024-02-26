using Last.Simple.App.Domain.Contracts.Queries;
using Last.Simple.App.Domain.Contracts.UseCases.Products;
using Last.Simple.App.Domain.Models.Products;
using Last.Simple.App.Domain.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Last.Simple.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ICreateProductUC createProductUC;
        private readonly IUpdateProductUC updateProductUC;
        private readonly IProductQuery productQuery;

        public ProductController(ICreateProductUC createProductUC, IUpdateProductUC updateProductUC, IProductQuery productQuery)
        {
            this.createProductUC = createProductUC;
            this.updateProductUC = updateProductUC;
            this.productQuery = productQuery;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<long>> CreateProduct([FromBody] CreateProductRequest request)
        {
            var id = await createProductUC.Create(request);

            return Created(string.Empty, new { id });
        }

        [Authorize]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute]long id, [FromBody] UpdateProductRequest request)
        {
            await updateProductUC.Update(id, request);

            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ProductView>> GetProduct([FromRoute]long id)
        {
            var product = await productQuery.Get(id);

            if (product is null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductView>>> ListAll()
        {
            var products = await productQuery.ListAll();

            return Ok(products);
        }
    }
}
