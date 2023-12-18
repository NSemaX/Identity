using Product.API.Models;
namespace Product.API.Infrastructure.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product.API.Models.Product>> GetAll();
    }
}
