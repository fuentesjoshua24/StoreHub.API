using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StoreHub.API.CommonUtility
{
    public class JwtUtil
    {
        private readonly string _secret;
        private readonly int _expiryMinutes;

        public JwtUtil(IConfiguration config)
        {
            _secret = config["Jwt:Secret"];
            _expiryMinutes = int.Parse(config["Jwt:ExpiryMinutes"] ?? "60");
        }

        public string GenerateToken(int userId, string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secret);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, email),
        new Claim("userId", userId.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // unique ID
        new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
    };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_expiryMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = "BettingHub.API",
                Audience = "BettingHub.Client"
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
