
using KFHRBackEnd.Models.Entites;
using KFHRBackEnd.Models.Entites.Request;
using KFHRBackEnd.Models.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly DBContext _context;
    private readonly IConfiguration _configuration;

    public AuthenticationController(DBContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    // POST: /Authentication/Register
    [HttpPost("Register")]
    public async Task<ActionResult<LoginRequest>> Register(LoginRequest request)
    {
        var user = new ApplicationUser()
        {
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return StatusCode(201); // Created
    }

    // POST: /Authentication/Login
    [HttpPost("Login")]
    public async Task<ActionResult<string>> Login(LoginRequest login)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
        {
            return BadRequest("Invalid credentials.");
        }

        var token = GenerateJwtToken(user);
        return Ok(token);
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
