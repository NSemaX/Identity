using Identity.API.DTOs.Requests;
using Identity.API.DTOs.Responses;
using Identity.API.Infrastructure.Repositories;
using Identity.API.Infrastructure;
using Identity.API.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Identity.API.Infrastructure.ErrorHandling;
using Identity.API.Infrastructure.Utils;

namespace Identity.API.Application
{
    public class IdentityOperations : IIdentityOperations
    {
        private readonly IOptions<JwtAuthentication> _jwtAuthentication;
        private readonly ITokenRepository _tokenRepository;
        private readonly IOptions<APISettings> _apisettings;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public IdentityOperations(IOptions<JwtAuthentication> jwtAuthentication, ITokenRepository tokenRepository,
            IOptions<APISettings> apisettings, IHttpContextAccessor httpContextAccessor)
        {
            _jwtAuthentication = jwtAuthentication ?? throw new ArgumentNullException(nameof(jwtAuthentication));
            _tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
            _apisettings = apisettings ?? throw new ArgumentNullException(nameof(apisettings));
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<GenerateTokenResponse> GenerateToken(GenerateTokenRequest request)
        {
            if (!await _tokenRepository.ValidateUser(request.Username, request.Password))
                throw new CustomException("Username or password is invalid");

            APIToken available_token = await _tokenRepository.GetTokenByUsername(request.Username);

            if (available_token != null)
                return (new GenerateTokenResponse
                {
                    JWT = null,//"This user already has a token, please use your refresh token to get a new JWT Token.", to avoid double record for token on DB.
                    refreshToken = available_token.Refreshtoken,
                    JWTExpireDate = null,//available_token.JWT_ExpireDate.ToString("dd.MM.yyyy HH:mm:ss"),
                    RefreshTokenExpireDate = available_token.RT_ExpireDate.ToString("dd.MM.yyyy HH:mm:ss")
                });

            // TODO get user roles and set
            string UserRoles = "User";
            var someClaims = new Claim[]{
                new Claim(JwtRegisteredClaimNames.Sub, request.Username),
                new Claim(ClaimTypes.Role, UserRoles),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            DateTime jwt_expiredatetime = DateTime.Now.AddMinutes(_apisettings.Value.JWT_ExpireMins);
            

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthentication.Value.SecurityKey));
            var token = new JwtSecurityToken(
                issuer: _jwtAuthentication.Value.ValidIssuer,
                audience: _jwtAuthentication.Value.ValidAudience,
                claims: someClaims,
                expires: jwt_expiredatetime,
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );



            var _tokenObj = new APIToken
            {
                Username = request.Username,
                Roles = UserRoles,
                Refreshtoken = Guid.NewGuid().ToString(),
                JWT = new JwtSecurityTokenHandler().WriteToken(token),
                JWT_ExpireDate = jwt_expiredatetime,
                RT_ExpireDate = DateTime.Now.AddDays(_apisettings.Value.RefreshTokenExpireDays),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Revoked = 0,
                IP = Helpers.GetUserIP(_httpContextAccessor)
            };
            await _tokenRepository.Add(_tokenObj);


            return (new GenerateTokenResponse
            {
                JWT = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = _tokenObj.Refreshtoken,
                JWTExpireDate = _tokenObj.JWT_ExpireDate.ToString("dd.MM.yyyy HH:mm:ss"),
                RefreshTokenExpireDate = _tokenObj.RT_ExpireDate.ToString("dd.MM.yyyy HH:mm:ss")
            });
        }

        public async Task<GenerateTokenResponse> RefreshToken(string refreshToken)
        {
            //check refresh token
            APIToken _apiToken = await _tokenRepository.GetByRefreshToken(refreshToken);

            if (_apiToken == null)
                throw new CustomException("Refresh token not found");


            var someClaims = new Claim[]{
                new Claim(JwtRegisteredClaimNames.Sub, _apiToken.Username),
                new Claim(ClaimTypes.Role, _apiToken.Roles),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            DateTime jwt_expiredatetime = DateTime.Now.AddMinutes(_apisettings.Value.JWT_ExpireMins);
            
            //generate new token
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthentication.Value.SecurityKey));
            var token = new JwtSecurityToken(
                issuer: _jwtAuthentication.Value.ValidIssuer,
                audience: _jwtAuthentication.Value.ValidAudience,
                claims: someClaims,
                expires: jwt_expiredatetime,
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            if (_apiToken.RT_ExpireDate <= DateTime.Now) //if refresh token expired create a new one
            {
                _apiToken.Refreshtoken = Guid.NewGuid().ToString();
                _apiToken.RT_ExpireDate = DateTime.Now.AddDays(_apisettings.Value.RefreshTokenExpireDays);
            }
            else
            {
                _apiToken.Refreshtoken = _apiToken.Refreshtoken;
            }

            _apiToken.JWT = new JwtSecurityTokenHandler().WriteToken(token);
            _apiToken.JWT_ExpireDate = jwt_expiredatetime;
            _apiToken.UpdatedDate = DateTime.Now;
            _apiToken.IP = Helpers.GetUserIP(_httpContextAccessor);

            await _tokenRepository.Update(_apiToken);

            return (new GenerateTokenResponse
            {
                JWT = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = _apiToken.Refreshtoken,
                JWTExpireDate = _apiToken.JWT_ExpireDate.ToString("dd.MM.yyyy HH:mm:ss"),
                RefreshTokenExpireDate = _apiToken.RT_ExpireDate.ToString("dd.MM.yyyy HH:mm:ss")
            });
        }

        public async Task RevokeToken(string refreshToken)
        {
            //check refresh token
            APIToken _apiToken = await _tokenRepository.GetByRefreshToken(refreshToken);

            if (_apiToken == null)
                throw new CustomException("Refresh token not found");

            _apiToken.Revoked = 1;
            _apiToken.UpdatedDate = DateTime.Now;

            // revoke token and update
            await _tokenRepository.Update(_apiToken);
        }
    }
}
