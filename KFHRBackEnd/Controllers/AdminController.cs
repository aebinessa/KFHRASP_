using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KFHRBackEnd.Models.Entites;
using KFHRBackEnd.Models.Entites.Request.Employee;
using KFHRBackEnd.Migrations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KFHRBackEnd.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly DBContextApp _context;

        public AdminController(DBContextApp context)
        {
            _context = context;
        }

        [HttpPost("idResponse")]
        [ProducesResponseType(typeof(IActionResult), 201)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CheckInEmployee(EmployeeIdResponse employeeIdResponse)
        {
            try
            {
                var checkInEmployee = new Attendance()
                {
                    EmployeeId = employeeIdResponse.EmployeeId,
                    CheckInTime = DateTime.Now
                };
                if (checkInEmployee.CheckInTime == DateTime.Today.AddHours(8))
                {
                    checkInEmployee.CheckInTime = DateTime.Now;
                }
                if (checkInEmployee.CheckOutTime == DateTime.Today.AddHours(16))
                {
                    checkInEmployee.CheckOutTime = DateTime.Now;
                }
                else
                {
                    return BadRequest("The Employee Didn't Attend");
                }
                _context.Attendances.Add(checkInEmployee);
                _context.SaveChanges();
                return Created(nameof(CheckInEmployee), new { Id = employeeIdResponse.EmployeeId });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }


        [HttpPost("leavesResponse")]
        [ProducesResponseType(typeof(IActionResult), 201)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult LeavesResponse(Leave leavesResponse)
        {
            try
            {
                var leavesResponses = new Leave()
                {
                    ID = leavesResponse.ID,
                   EmployeeId = leavesResponse.EmployeeId,
                   LeaveType = leavesResponse.LeaveType,
                   StartDate = leavesResponse.StartDate,
                   EndDate = leavesResponse.EndDate,
                   Notes = leavesResponse.Notes,
                   Status = leavesResponse.Status
                };
                _context.Leaves.Add(leavesResponses);
                _context.SaveChanges();
                return Created(nameof(CheckInEmployee), new { Id = leavesResponse.EmployeeId });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }



        [HttpGet("getAttendance")]
        [ProducesResponseType(typeof(IEnumerable<Attendance>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAttendance()
        {
            try
            {
                var attendances = await _context.Attendances.ToListAsync();
                return Ok(attendances);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getLeave")]
        [ProducesResponseType(typeof(IEnumerable<Leave>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetLeave()
        {
            try
            {
                var leaves = await _context.Leaves.ToListAsync();
                return Ok(leaves);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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
        // post 
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
                var defaultPassword = "123";
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(defaultPassword);

                var employee = new Employee
                {
                    Name = employeeDto.Name,
                    Email = employeeDto.Email,
                    Password = hashedPassword, // Set the hashed default password
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


        // edit (Put)
        [HttpPut("EditEmployee")]
        [ProducesResponseType(typeof(Employee), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> EditEmployee([FromBody] EditEmployee employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest("Employee data is null.");
            }

            try
            {
                var existingEmployee = await _context.Employees.FindAsync(employeeDto.Id);
                if (existingEmployee == null)
                {
                    return NotFound("Employee not found.");
                }

                existingEmployee.Name = employeeDto.Name;
                existingEmployee.Email = employeeDto.Email;
                existingEmployee.DOB = employeeDto.DOB;
                existingEmployee.Gender = employeeDto.Gender;
                existingEmployee.ProfilePicURL = employeeDto.ProfilePicURL;
                existingEmployee.NFCIdNumber = employeeDto.NFCIdNumber;
                existingEmployee.PointEarned = employeeDto.PointEarned;

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
