using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using KFHRBackEnd.Models;
using KFHRBackEnd.Models.Entites;
using KFHRBackEnd.Models.Entites.Request;
using Microsoft.Extensions.Configuration;

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

  [HttpPost("Register")]
public async Task<ActionResult<Employee>> Register(RegisterRequest request)
{
    var employee = new Employee
    {
        Email = request.Email,
        Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
        Name = request.Name,
        DOB = request.DOB,
        Gender = request.Gender,
        ProfilePicURL = request.ProfilePicURL,
        IsAdmin = request.IsAdmin
    };

    _context.Employees.Add(employee);
    await _context.SaveChangesAsync();
    return StatusCode(201); // Created
}


    [HttpPost("Login")]
    public async Task<ActionResult<string>> Login(LoginRequest login)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(u => u.Email == login.EmployeeID);
        if (employee == null || !BCrypt.Net.BCrypt.Verify(login.Password, employee.Password))
        {
            return BadRequest("Invalid credentials.");
        }

        var token = GenerateJwtToken(employee);
        return Ok(token);
    }

    private string GenerateJwtToken(Employee employee)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
            new Claim(ClaimTypes.Email, employee.Email),
            // Add other claims as necessary
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
