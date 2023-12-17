using Identity.API.Application;
using Identity.API.DTOs.Requests;
using Identity.API.DTOs.Responses;
using Identity.API.Infrastructure;
using Identity.API.Infrastructure.ErrorHandling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityOperations _identityOperations;

        public IdentityController(IIdentityOperations identityOperations)
        {
            _identityOperations = identityOperations ?? throw new ArgumentNullException(nameof(identityOperations));
        }

        /// <summary>
        /// Generates JWT and RefreshToken
        /// </summary>
        ///<remarks>
        /// Generates JWT, RefreshToken and their's expire dates. Use this metod once and get jwt and refresh token, to refresh your JWT use /refresh method when your JWT expired. 
        ///</remarks>
        ///<response code="400">Validation Error</response>
        ///<response code="401">Unauthorized</response>
        /// <response code="500">Server Error</response>
        [HttpPost("token")]
        [AllowAnonymous]
        [SwaggerRequestExample(typeof(GenerateTokenRequest), typeof(GenerateTokenRequestExample))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(GenerateTokenResponseExample))]
        [ProducesResponseType(typeof(GenerateTokenResponseExample), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GenerateToken([FromBody] GenerateTokenRequest request)
        {
            try
            {
                GenerateTokenResponse response = await _identityOperations.GenerateToken(request);

                if (response.JWT == null)
                {
                    return new BadRequestObjectResult(new
                    {
                        error = new GenerateTokenError
                        {
                            Code = "20014",
                            Message = "Bad request",
                            Description = "This user already has a token, please use your refresh token to get a new JWT Token.",//_errorRepository.GetFromCache("20014", _distributedCache).Result,
                            JWT = response.JWT,
                            refreshToken = response.refreshToken,
                            JWTExpireDate = response.JWTExpireDate,
                            RefreshTokenExpireDate = response.RefreshTokenExpireDate
                        }
                    });
                }
                else
                    return Ok(response);
            }
            catch (Exception ex)
            {
                throw new CustomException($"Exception: {ex.Message}");
            }

        }

        /// <summary>
        /// Generates new JWT token if refresh token available in the system without resending Username and Password
        /// </summary>
        ///<remarks>
        ///Generates new JWT and Refreshtoken when they are expired.
        ///</remarks>
        ///<response code="400">Validation Error</response>
        ///<response code="401">Unauthorized</response>
        /// <response code="500">Server Error</response>
        [HttpPost("refresh/{refreshToken}")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(GenerateTokenResponseExample))]
        [ProducesResponseType(typeof(GenerateTokenResponseExample), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> RefreshToken([FromRoute] string refreshToken)
        {
            try
            {
                GenerateTokenResponse response = await _identityOperations.RefreshToken(refreshToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw new CustomException( $"Exception: {ex.Message}");
            }
        }

        /// <summary>
        /// Revokes token
        /// </summary>
        ///<remarks>
        ///Revokes token
        ///</remarks>
        ///<response code="400">Validation Error</response>
        ///<response code="401">Unauthorized</response>
        /// <response code="500">Server Error</response>
        [HttpPost("revoke/{refreshToken}")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(GenerateTokenResponseExample))]
        [ProducesResponseType(typeof(GenerateTokenResponseExample), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> RevokeToken([FromRoute] string refreshToken)
        {
            try
            {
                await _identityOperations.RevokeToken(refreshToken);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new CustomException($"Exception: {ex.Message}");
            }
        }

    }
}
