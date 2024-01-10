using Identity.API.Infrastructure.ErrorHandling;
using Identity.API.Models;
using System.Net;

namespace Identity.API.Infrastructure.Utils
{
    public static class Helpers
    {
        public static string GetUserIP(IHttpContextAccessor httpContextAccessor)
        {
            string UserIP = "";
            try
            {
                string header = (httpContextAccessor.HttpContext.Request.Headers["CF-Connecting-IP"].FirstOrDefault() ?? httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault());
                if (IPAddress.TryParse(header, out IPAddress ip))
                {
                    UserIP = ip.MapToIPv4().ToString();
                }
                UserIP = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
            catch (Exception ex)
            {
                throw new CustomException($"Exception: {ex.Message}");
            }
            return UserIP;
        }

        public static bool isRevoked(APIToken apiToken)
        {
            bool isRevoked = false;
            if (apiToken != null)
            {
                isRevoked = Convert.ToBoolean(apiToken.Revoked);
            }
            return isRevoked;
        }
    }

}
