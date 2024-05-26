using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KFHRBackEnd.Models.Entites;

namespace KFHRBackEnd.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly DBContextApp _context;

        public AdminController(DBContextApp context)
        {
            _context = context;
        }

        // GET: api/Admin/Employees
        [HttpGet("Employees")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var employees = await _context.Employees.ToListAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Admin/AddEmployee
        [HttpPost("AddEmployee")]
        [ProducesResponseType(typeof(Employee), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddEmployee([FromBody] AddEmployee employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest("Employee data is null.");
            }

            try
            {
                var employee = new Employee
                {
                    Name = employeeDto.Name,
                    Email = employeeDto.Email,
                    DOB = employeeDto.DOB,
                    Gender = employeeDto.Gender,
                    ProfilePicURL = employeeDto.ProfilePicURL,
                    NFCIdNumber = employeeDto.NFCIdNumber,
                    PositionId = employeeDto.PositionId,
                    DepartmentId = employeeDto.DepartmentId,
                    PointEarned = employeeDto.PointEarned,
                    IsAdmin = false // Ensure IsAdmin is not set during add operation
                };

                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Admin/EditEmployee/5
        [HttpPut("EditEmployee/{id}")]
        [ProducesResponseType(typeof(Employee), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> EditEmployee(int id, [FromBody] Employee employee)
        {
            if (employee == null || employee.Id != id)
            {
                return BadRequest("Employee data is invalid.");
            }

            try
            {
                var existingEmployee = await _context.Employees.FindAsync(id);
                if (existingEmployee == null)
                {
                    return NotFound("Employee not found.");
                }

                existingEmployee.Name = employee.Name;
                existingEmployee.Email = employee.Email;
                existingEmployee.DOB = employee.DOB;
                existingEmployee.Gender = employee.Gender;
                existingEmployee.ProfilePicURL = employee.ProfilePicURL;
                existingEmployee.NFCIdNumber = employee.NFCIdNumber;
                existingEmployee.PositionId = employee.PositionId;
                existingEmployee.DepartmentId = employee.DepartmentId;
                existingEmployee.PointEarned = employee.PointEarned;

                // Ensure IsAdmin is not set during edit operation
                existingEmployee.IsAdmin = existingEmployee.IsAdmin;

                _context.Employees.Update(existingEmployee);
                await _context.SaveChangesAsync();
                return Ok(existingEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Admin/DeleteEmployee/5
        [HttpDelete("DeleteEmployee/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound("Employee not found.");
                }

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return Ok("Employee deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("add-department")]
        [ProducesResponseType(typeof(IActionResult), 201)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddDepartment(AddDepartmentRequest addDepartmentRequest)
        {
            try
            {
                var department = new Department()
                {
                    DepartmentName = addDepartmentRequest.DepartmentName
                };
                _context.Departments.Add(department);
                _context.SaveChanges();
                return Created(nameof(AddDepartment), new { Id = department.ID });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
        [HttpDelete("DeleteDepartment/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var department = await _context.Departments.FindAsync(id);
                if (department == null)
                {
                    return NotFound("Department not found.");
                }

                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
                return Ok("Department deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
