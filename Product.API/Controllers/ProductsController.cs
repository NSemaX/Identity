using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product.API.Application;
using System.Net;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductOperations _productOperations;

        public ProductsController(IProductOperations productOperations)
        {
            _productOperations = productOperations ?? throw new ArgumentNullException(nameof(productOperations));
        }
        /// <summary>
        /// Get All Products
        /// </summary>
        ///<remarks>
        /// Gets all products.
        ///</remarks>
        ///<response code="400">Validation Error</response>
        ///<response code="401">Unauthorized</response>
        /// <response code="404">Products not found</response>
        [Authorize]
        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(List<Product.API.Models.Product>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetProductsAsync()
        {
            var products = await _productOperations.GetProducts();
            if (products == null)
                return NotFound();
           
            return Ok(products);
        }
    }
}
