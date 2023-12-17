using Dapper;
using Identity.API.Infrastructure.ErrorHandling;
using Identity.API.Models;
using System.Data;

namespace Identity.API.Infrastructure.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly string _conString;
        private readonly ILogger _logger;


        public TokenRepository(string conString)
        {
            _conString = conString ?? throw new ArgumentNullException(nameof(conString));
            //_logger = logger;
        }

        public async Task<APIToken> GetByRefreshToken(string refreshtoken)
        {
            try
            {
                using (IDbConnection conn = GetConnection.GetConnectionDB(_conString))
                {
                    string sQuery = "SELECT * FROM Tokens WHERE Refreshtoken = @refreshtoken";
                    var result = await conn.QueryAsync<APIToken>(sQuery, new { refreshtoken = refreshtoken });
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message + " " + ex.StackTrace);
                throw new CustomException($"Exception: {ex.Message}");
            }

        }

        public async Task<APIToken> GetTokenByUsername(string username)
        {
            try
            {
                using (IDbConnection conn = GetConnection.GetConnectionDB(_conString))
                {
                    string sQuery = "SELECT * FROM Tokens WHERE username = @username";
                    var result = await conn.QueryAsync<APIToken>(sQuery, new { username = username });
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message + " " + ex.StackTrace);
                throw new CustomException($"Exception: {ex.Message}");
            }
        }

        public async Task<List<APIToken>> GetAll()
        {
            try
            {
                using (IDbConnection conn = GetConnection.GetConnectionDB(_conString))
                {
                    string sQuery = "SELECT * FROM Tokens";
                    var result = await conn.QueryAsync<APIToken>(sQuery);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message + " " + ex.StackTrace);
                throw new CustomException($"Exception: {ex.Message}");
            }

        }

        public async Task Add(APIToken token)
        {
            try
            {
                using (IDbConnection conn = GetConnection.GetConnectionDB(_conString))
                {

                    var sqlStatement = @"
                            INSERT INTO Tokens 
                            (Username
                            ,Roles
                            ,Refreshtoken
                            ,JWT
                            ,JWT_ExpireDate
                            ,RT_ExpireDate
                            ,CreatedDate
                            ,UpdatedDate
                            ,IP
                            ,Revoked)
                            VALUES (
                             @Username
                            ,@Roles
                            ,@Refreshtoken
                            ,@JWT
                            ,@JWT_ExpireDate
                            ,@RT_ExpireDate
                            ,@CreatedDate
                            ,@UpdatedDate
                            ,@IP
                            ,@Revoked)";
                    await conn.ExecuteAsync(sqlStatement, token);
                }

                // _logger.LogDebug("token added. {@Customer}", refreshtoken);

            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message + " " + ex.StackTrace);
                throw new CustomException($"Exception: {ex.Message}");
            }

        }

        public async Task Update(APIToken token)
        {
            try
            {
                using (IDbConnection conn = GetConnection.GetConnectionDB(_conString))
                {

                    var sqlStatement = @"
                            UPDATE Tokens 
                            SET  Username = @Username
                            ,Roles = @Roles
                            ,Refreshtoken = @Refreshtoken 
                            ,JWT = @JWT 
                            ,JWT_ExpireDate= @JWT_ExpireDate 
                            ,RT_ExpireDate= @RT_ExpireDate 
                            ,CreatedDate = @CreatedDate 
                            ,UpdatedDate = @UpdatedDate 
                            ,IP = @IP
                            ,Revoked = @Revoked
                            WHERE ID = @ID";
                    await conn.ExecuteAsync(sqlStatement, token);
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message + " " + ex.StackTrace);
                throw new CustomException($"Exception: {ex.Message}");
            }

        }


        public async Task Delete(string refreshtoken)
        {
            try
            {
                using (IDbConnection conn = GetConnection.GetConnectionDB(_conString))
                {
                    string sqlStatement = "DELETE Tokens WHERE Refreshtoken = @refreshtoken";
                    await conn.QueryAsync<APIToken>(sqlStatement, new { refreshtoken = refreshtoken });
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message + " " + ex.StackTrace);
                throw new CustomException($"Exception: {ex.Message}");
            }

        }

        public async Task<bool> ValidateUser(string username, string password)
        {
            bool response = false;
            try
            {
                using (IDbConnection conn = GetConnection.GetConnectionDB(_conString))
                {
                    string sQuery = "select * from Users where Username = @username and Password= @password";
                    var result = await conn.QueryAsync<User>(sQuery, new { Username = username, Password = password });
                    if (result != null && result.Count() > 0) 
                    {
                        response = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message + " " + ex.StackTrace);
                throw new CustomException($"Exception: {ex.Message}");
            }

            return response;
        }

    }
}
