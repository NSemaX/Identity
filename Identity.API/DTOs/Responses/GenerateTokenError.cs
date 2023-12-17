namespace Identity.API.DTOs.Responses
{
    public class GenerateTokenError
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public string JWT { get; set; }
        public string refreshToken { get; set; }
        public string JWTExpireDate { get; set; }
        public string RefreshTokenExpireDate { get; set; }
    }
}
