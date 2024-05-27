using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using KFHRBackEnd.Models.Entites;
using KFHRBackEnd.Models.Entites.Request;
using KFHRBackEnd.Models.Services;
using KFHRBackEnd.Models.Entites.Request.Employee;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly DBContextApp _context;
    private readonly TokenService _tokenService;

    public AuthenticationController(DBContextApp context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<RegisterRequest>> Register(RegisterRequest request)
    {
        if (await _context.Employees.AnyAsync(e => e.Email == request.Email))
        {
            return BadRequest("Email already exists.");
        }

        var employee = new Employee
        {
            Name = request.Name,
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            DOB = request.DOB,
            Gender = request.Gender,
            ProfilePicURL = request.ProfilePicURL,
            IsAdmin = request.IsAdmin
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return StatusCode(201); 
    }


    [HttpPost("Login")]
    public async Task<ActionResult<string>> Login(LoginRequest login)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(u => u.Email == login.Email);
        if (employee == null)
        {
            return BadRequest("Invalid email.");
        }

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(login.Password, employee.Password);
        if (!isPasswordValid)
        {
            return BadRequest("Invalid password.");
        }

        var (isValid, token) = _tokenService.GenerateToken(employee.Email, login.Password);
        if (!isValid)
        {
            return Unauthorized("Invalid credentials.");
        }

        return Ok(new { Token = token });
    }

}
