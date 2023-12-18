using Dapper;
using Product.API.Infrastructure.ErrorHandling;
using Product.API.Models;
using System.Data;

namespace Product.API.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _conString;
        public ProductRepository(string conString)
        {
            _conString = conString ?? throw new ArgumentNullException(nameof(conString));
        }
        public async Task<List<Product.API.Models.Product>> GetAll()
        {
            try
            {
                using (IDbConnection conn = GetConnection.GetConnectionDB(_conString))
                {
                    string sQuery = "SELECT * FROM Products";
                    var result = await conn.QueryAsync< Product.API.Models.Product> (sQuery);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new CustomException($"Exception: {ex.Message}");
            }

        }

    }
}
