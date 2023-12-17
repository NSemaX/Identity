using Identity.API.Models;

namespace Identity.API.Infrastructure.Repositories
{
    public interface ITokenRepository
    {
        Task<APIToken> GetByRefreshToken(string refreshtoken);
        Task<APIToken> GetTokenByUsername(string username);
        Task<List<APIToken>> GetAll();
        Task Add(APIToken token);
        Task Update(APIToken token);
        Task Delete(string refreshtoken);
        Task<bool> ValidateUser(string username, string password);
    }
}
