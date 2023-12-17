

using Swashbuckle.AspNetCore.Filters;

namespace Identity.API.DTOs.Responses
{
    public class GenerateTokenResponse
    {
        public string JWT { get; set; }
        public string refreshToken { get; set; }
        public string JWTExpireDate { get; set; }
        public string RefreshTokenExpireDate { get; set; }
    }

    #region Example

    public class GenerateTokenResponseExample : IExamplesProvider<GenerateTokenResponse>
    {
        public GenerateTokenResponse GetExamples()
        {
            return new GenerateTokenResponse
            {
                JWT = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c",
                refreshToken = "f41c92bf-cc20-491b-9e4c-efc1b253e43a",
                JWTExpireDate = "10.03.2020 16:05:00",
                RefreshTokenExpireDate = "09.04.2020 15:04:31"
            };
        }
    }

    #endregion
}
