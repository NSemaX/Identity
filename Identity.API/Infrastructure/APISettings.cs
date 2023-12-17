namespace Identity.API.Infrastructure
{
    public class APISettings
    {
        public int JWT_ExpireMins { get; set; }
        public int RefreshTokenExpireDays { get; set; }
    }
}
