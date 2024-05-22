using KFHRBackEnd.Models.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace KFHRBackEnd.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly DBContext _context;
      
        public AdminController(ILogger<AdminController> logger, DBContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }


        [HttpPost("add-employee")]
        [ProducesResponseType(typeof(IActionResult), 201)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddEmployee(AddEmployees employee)
        {
            try
            {
                var employees = new Employee()
                { 
                    DepartmentId = employee.DepartmentId,
                    Name = employee.Name,
                    Password = employee.Password,
                    Email = employee.Email,
                    Role = employee.Role,
                    Gender = employee.Gender,
                    DOB = employee.DOB,
                    PointEarned = employee.PointEarned
                };
                _context.Employees.Add(employees);
                _context.SaveChanges();
                return Created(nameof(AddEmployee), new { Id = employees.Id });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpDelete("delete-employee/{id}")]
        [ProducesResponseType(typeof(IActionResult), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var userdelete = _context.Employees.Find(id);
                if (userdelete == null)
                {
                    return NotFound();
                }

                _context.Employees.Remove(userdelete);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
