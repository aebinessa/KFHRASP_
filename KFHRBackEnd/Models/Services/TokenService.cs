using KFHRBackEnd.Models.Entites;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace KFHRBackEnd.Models.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private readonly DBContextApp _context;

        public TokenService(IConfiguration configuration, DBContextApp context)
        {
            _configuration = configuration;
            _context = context;
        }

        public (bool IsValid, string Token) GenerateToken(string email, string password)
        {
            var userAccount = _context.Employees.SingleOrDefault(r => r.Email == email);
            if (userAccount == null)
            {
                return (false, "");
            }

            var validPassword = BCrypt.Net.BCrypt.Verify(password, userAccount.Password);
            if (!validPassword)
            {
                return (false, "");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userAccount.Id.ToString()),
                new Claim(ClaimTypes.Email, userAccount.Email),
                new Claim(ClaimTypes.Role, userAccount.IsAdmin ? "Admin" : "User")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);
            var generatedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return (true, generatedToken);
        }
    }
}
