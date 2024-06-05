using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KFHRBackEnd.Models.Entites;
using KFHRBackEnd.Models.Entites.Request.Department;
using KFHRBackEnd.Models.Entites.Request;
using System.Security.Claims;
using KFHRBackEnd.Models;

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

        // Get all employee attendances
        [HttpGet("GetAttendances")]
        [ProducesResponseType(typeof(IEnumerable<Attendance>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAttendances()
        {
            try
            {
                var attendances = await _context.Attendances.ToListAsync();
                return Ok(attendances);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpGet("GetAttendance/{id}")]
        [ProducesResponseType(typeof(Attendance), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAttendance(int id)
        {
            try
            {
                var attendance = await _context.Attendances.FindAsync(id);
                if (attendance == null)
                {
                    return NotFound("Attendance record not found.");
                }
                return Ok(attendance);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpGet("GetAttendancesByEmployee/{employeeId}")]
        [ProducesResponseType(typeof(IEnumerable<Attendance>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAttendancesByEmployee(int employeeId)
        {
            try
            {
                var attendances = await _context.Attendances
                    .Where(a => a.EmployeeId == employeeId)
                    .ToListAsync();
                return Ok(attendances);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    

    // Get all leave requests
    [HttpGet("GetLeaves")]
        [ProducesResponseType(typeof(IEnumerable<Leave>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetLeaves()
        {
            try
            {
                var leaves = await _context.Leaves.ToListAsync();
                return Ok(leaves);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // Update leave status
        [HttpPut("UpdateLeaveStatus/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateLeaveStatus(int id, [FromBody] UpdateLeaveStatus leaveStatusDto)
        {
            if (leaveStatusDto == null)
            {
                return BadRequest("Leave status data is null.");
            }

            try
            {
                var existingLeave = await _context.Leaves.FindAsync(id);
                if (existingLeave == null)
                {
                    return NotFound("Leave not found.");
                }

                existingLeave.Status = leaveStatusDto.Status;

                _context.Leaves.Update(existingLeave);
                await _context.SaveChangesAsync();
                return Ok(existingLeave);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    

    [HttpPost("AddCertificate")]
        [ProducesResponseType(typeof(Certificate), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddCertificate([FromBody] Certificate certificateDto)
        {
            if (certificateDto == null)
            {
                return BadRequest("Certificate data is null.");
            }

            try
            {
                var certificate = new Certificate
                {
                    EmployeeId = certificateDto.EmployeeId, // Admin can specify the EmployeeId
                    CertificateName = certificateDto.CertificateName,
                    IssueDate = certificateDto.IssueDate,
                    ExpirationDate = certificateDto.ExpirationDate,
                    VerificationURL = certificateDto.VerificationURL
                };

                await _context.Certificates.AddAsync(certificate);
                await _context.SaveChangesAsync();
                return Created(nameof(AddCertificate), new { Id = certificate.ID });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetCertificates")]
        [ProducesResponseType(typeof(IEnumerable<Certificate>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCertificates()
        {
            try
            {
                var certificates = await _context.Certificates.ToListAsync();
                return Ok(certificates);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("UpdateCertificate/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateCertificate(int id, [FromBody] AddCertificate certificateDto)
        {
            if (certificateDto == null)
            {
                return BadRequest("Certificate data is null.");
            }

            try
            {
                var existingCertificate = await _context.Certificates.FindAsync(id);
                if (existingCertificate == null)
                {
                    return NotFound("Certificate not found.");
                }

                existingCertificate.CertificateName = certificateDto.CertificateName;
                existingCertificate.IssueDate = certificateDto.IssueDate;
                existingCertificate.ExpirationDate = certificateDto.ExpirationDate;
                existingCertificate.VerificationURL = certificateDto.VerificationURL;

                _context.Certificates.Update(existingCertificate);
                await _context.SaveChangesAsync();
                return Ok(existingCertificate);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("DeleteCertificate/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteCertificate(int id)
        {
            try
            {
                var existingCertificate = await _context.Certificates.FindAsync(id);
                if (existingCertificate == null)
                {
                    return NotFound("Certificate not found.");
                }

                _context.Certificates.Remove(existingCertificate);
                await _context.SaveChangesAsync();
                return Ok("Certificate deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    

    [HttpPost("GivePoints")]
        [ProducesResponseType(typeof(Employee), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GivePoints([FromBody] GivePointsRequest request)
        {
            if (request == null || request.Points < 0)
            {
                return BadRequest("Invalid points request.");
            }

            try
            {
                var existingEmployee = await _context.Employees.FindAsync(request.EmployeeId);
                if (existingEmployee == null)
                {
                    return NotFound("Employee not found.");
                }

                existingEmployee.PointEarned = (existingEmployee.PointEarned ?? 0) + request.Points;

                _context.Employees.Update(existingEmployee);
                await _context.SaveChangesAsync();
                return Ok(existingEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("AddRecommendedCertificate")]
        [ProducesResponseType(typeof(RecommendedCertificate), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddCertificate([FromBody] RecommendedCertificate certificate)
        {
            if (certificate == null)
            {
                return BadRequest("Certificate data is null.");
            }

            try
            {
                await _context.RecommendedCertificates.AddAsync(certificate);
                await _context.SaveChangesAsync();
                return Ok(certificate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

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
                var department = _context.Departments.FirstOrDefault(d => d.ID == employeeDto.DepartmentId);

                var employee = new Employee
                {
                    Name = employeeDto.Name,
                    Email = employeeDto.Email,
                    Password = hashedPassword,
                    DOB = employeeDto.DOB,
                    Gender = employeeDto.Gender,
                    ProfilePicURL = employeeDto.ProfilePicURL,
                    NFCIdNumber = employeeDto.NFCIdNumber,
                    PositionName = employeeDto.PositionName,
                    DepartmentId = department,
                    PointEarned = employeeDto.PointEarned,
                    // Allow the admin to specify if the new employee is an admin
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

        [HttpPut("EditEmployee/{id}")]
        [ProducesResponseType(typeof(Employee), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> EditEmployee(int id, [FromBody] EditEmployee employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest("Employee data is null.");
            }

            try
            {
                var existingEmployee = await _context.Employees.FindAsync(id);
                if (existingEmployee == null)
                {
                    return NotFound("Employee not found.");
                }

                existingEmployee.Name = employeeDto.Name;
                existingEmployee.Email = employeeDto.Email;
                existingEmployee.DOB = employeeDto.DOB;
                existingEmployee.Gender = employeeDto.Gender;
                var department = _context.Departments.FirstOrDefault(d => d.ID == employeeDto.DepartmentId);

                if (!string.IsNullOrEmpty(employeeDto.ProfilePicURL))
                {
                    existingEmployee.ProfilePicURL = employeeDto.ProfilePicURL;
                }

                existingEmployee.NFCIdNumber = employeeDto.NFCIdNumber;
                existingEmployee.PositionName = employeeDto.PositionName;
                existingEmployee.DepartmentId = department;
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

        [HttpGet("GetDepartments")]
        [ProducesResponseType(typeof(IEnumerable<Department>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetDepartments()
        {
            try
            {
                var departments = await _context.Departments.ToListAsync();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("EditDepartment/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> EditDepartment(int id, EditDepartmentRequest editDepartmentRequest)
        {
            try
            {
                var department = await _context.Departments.FindAsync(id);
                if (department == null)
                {
                    return NotFound("Department not found.");
                }

                department.DepartmentName = editDepartmentRequest.DepartmentName;
                _context.Departments.Update(department);
                await _context.SaveChangesAsync();

                return Ok("Department updated.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("AddDepartment")]
        [ProducesResponseType(typeof(IActionResult), 201)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddDepartment(DepartmentRequest addDepartmentRequest)
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

        [HttpGet("Employees")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEmployees(int? employeeId = null)
        {
            try
            {
                if (employeeId.HasValue)
                {
                    var employee = await _context.Employees.Include(x=>x.DepartmentId).FirstOrDefaultAsync(x => x.Id == employeeId.Value);
                    if (employee == null)
                    {
                        return NotFound("Employee not found.");
                    }
                    return Ok(employee);
                }
                else
                {
                    var employees = await _context.Employees.Include(x => x.DepartmentId).ToListAsync();
                    return Ok(employees);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetAttendance")]
        [ProducesResponseType(typeof(IEnumerable<Attendance>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAttendance()
        {
            try
            {
                var employeeId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (employeeId == null)
                {
                    return Unauthorized();
                }

                var attendances = await _context.Attendances
                    .Where(a => a.EmployeeId == int.Parse(employeeId))
                    .ToListAsync();
                return Ok(attendances);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

}
