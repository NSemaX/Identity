using Product.API.Infrastructure.Repositories;
namespace Product.API.Application
{
    public class ProductOperations : IProductOperations
    {
        private readonly IProductRepository _productRepository;

        public ProductOperations(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }
        public async Task<List<Product.API.Models.Product>> GetProducts()
        {
            return await _productRepository.GetAll();
        }
    }
}
