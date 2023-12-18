namespace Product.API.Application
{
    public interface IProductOperations
    {
        Task<List<Product.API.Models.Product>> GetProducts();
    }
}
