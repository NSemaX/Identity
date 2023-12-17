using Identity.API.DTOs.Requests;
using Identity.API.DTOs.Responses;

namespace Identity.API.Application
{
    public interface IIdentityOperations
    {
        Task<GenerateTokenResponse> GenerateToken(GenerateTokenRequest request);
        Task<GenerateTokenResponse> RefreshToken(string refreshToken);
        Task RevokeToken(string refreshToken);
    }
}
