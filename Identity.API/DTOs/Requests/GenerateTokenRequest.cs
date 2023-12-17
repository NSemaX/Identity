using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTOs.Requests
{
    public class GenerateTokenRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

    #region Example

    public class GenerateTokenRequestExample : IExamplesProvider<GenerateTokenRequest>
    {
        public GenerateTokenRequest GetExamples()
        {
            return new GenerateTokenRequest
            {
                Username = "skudu",
                Password = "SNDLDSBK"
            };
        }
    }

    #endregion
}
