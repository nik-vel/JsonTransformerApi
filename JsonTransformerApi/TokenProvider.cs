using JsonTransformerApi.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JsonTransformerApi
{
    public class TokenProvider : ITokenProvider
    {
        private readonly JwtOptions _jwtOptions;
        private readonly ILogger<ITokenProvider> _logger;

        public TokenProvider(IOptions<JwtOptions> jwtOptions, ILogger<ITokenProvider> logger)
        {
            _jwtOptions = jwtOptions.Value;
            _logger = logger;
        }

        /// <inheritdoc />
        public (bool isSuccess, string token) GetToken(string password)
        {
            if (!_jwtOptions.AllowedPasswords.Contains(password))
            {
                return (false, string.Empty);
            }
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = _jwtOptions.Issuer,
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim("Password", password)
                }),
                    Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.TtlMinutes),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Key)),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return (true, tokenHandler.WriteToken(token));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return (false, string.Empty);
            }
            
        }
    }
}
