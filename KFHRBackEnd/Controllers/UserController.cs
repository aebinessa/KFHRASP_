using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KFHRBackEnd.Models.Entites;
using KFHRBackEnd.Models.Entites.Request.LeaveReq;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KFHRBackEnd.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DBContextApp _context;

        public UserController(DBContextApp context)
        {
            _context = context;
        }


        [HttpGet("[action]/{id}")]
        [ProducesResponseType(typeof(IActionResult), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Employee> GetProfile(int id)
        {
            return _context.Employees.Find(id);
        }

        [HttpPost("CheckInEmployee")]
        [ProducesResponseType(typeof(IActionResult), 201)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CheckInEmployee()
        {
            try
            {
                var employeeId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (employeeId == null)
                {
                    return Unauthorized();
                }

                var checkInEmployee = new Attendance()
                {
                    EmployeeId = int.Parse(employeeId), // get employee ID from token 
                    CheckInTime = DateTime.Now
                };

                _context.Attendances.Add(checkInEmployee);
                _context.SaveChanges();
                return Created(nameof(CheckInEmployee), new { Id = checkInEmployee.ID });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("LeavesResponse")]
        [ProducesResponseType(typeof(IActionResult), 201)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult LeavesResponse(LeaveRequest leavesResponse)
        {
            try
            {
                var employeeId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (employeeId == null)
                {
                    return Unauthorized();
                }

                var leavesRequest = new Leave()
                {
                    EmployeeId = int.Parse(employeeId),
                    LeaveType = leavesResponse.LeaveTypes.ToString(),
                    StartDate = leavesResponse.StartDate,
                    EndDate = leavesResponse.EndDate,
                    Notes = leavesResponse.Notes,
                    Status = "Pending"
                };
                _context.Leaves.Add(leavesRequest);
                _context.SaveChanges();
                return Created(nameof(leavesRequest), new { Id = leavesRequest.ID });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
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

        [HttpGet("GetLeave")]
        [ProducesResponseType(typeof(IEnumerable<Leave>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetLeave()
        {
            try
            {
                var employeeId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (employeeId == null)
                {
                    return Unauthorized();
                }

                var leaves = await _context.Leaves
                    .Where(l => l.EmployeeId == int.Parse(employeeId))
                    .ToListAsync();
                return Ok(leaves);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

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
    }
}
