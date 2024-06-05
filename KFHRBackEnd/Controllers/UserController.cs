using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KFHRBackEnd.Models.Entites;
using KFHRBackEnd.Models.Entites.Request.LeaveReq;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KFHRBackEnd.Models.Entites.Request;

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

        [HttpGet("GetCertificates")]
        [ProducesResponseType(typeof(IEnumerable<Certificate>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCertificates()
        {
            try
            {
                var employeeId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (employeeId == null)
                {
                    return Unauthorized();
                }

                var certificates = await _context.Certificates
                    .Where(c => c.EmployeeId == int.Parse(employeeId))
                    .ToListAsync();

                return Ok(certificates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetPoints")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPoints()
        {
            try
            {
                var employeeId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (employeeId == null)
                {
                    return Unauthorized();
                }

                var existingEmployee = await _context.Employees.FindAsync(int.Parse(employeeId));
                if (existingEmployee == null)
                {
                    return NotFound("Employee not found.");
                }

                return Ok(existingEmployee.PointEarned ?? 0);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetRecommendedCertificates")]
        [ProducesResponseType(typeof(IEnumerable<RecommendedCertificate>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRecommendedCertificates()
        {
            try
            {
                var certificates = await _context.RecommendedCertificates.ToListAsync();
                return Ok(certificates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("[action]/{id}")]
        [ProducesResponseType(typeof(IActionResult), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Employee> GetProfile(int id)
        {
            return _context.Employees.Find(id);
        }

        [HttpGet("GetLateMinutes")]
        [ProducesResponseType(typeof(LateMinutesLeft), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetLateMinutes()
        {
            try
            {
                var employeeId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (employeeId == null)
                {
                    return Unauthorized();
                }

                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;

                var lateMinutes = await _context.LateMinutesLeft
                    .FirstOrDefaultAsync(l => l.EmployeeId == int.Parse(employeeId) && l.Month.Month == currentMonth && l.Month.Year == currentYear);

                return Ok(lateMinutes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPost("CheckInEmployee")]
        [ProducesResponseType(typeof(IActionResult), 201)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CheckInEmployee()
        {
            try
            {
                var employeeId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (employeeId == null)
                {
                    return Unauthorized();
                }

                var today = DateTime.Now;
                var todayAttendance = new Attendance()
                {
                    EmployeeId = int.Parse(employeeId), // get employee ID from token 
                    CheckInTime = today
                };

                await _context.Attendances.AddAsync(todayAttendance);

                var scheduledStartTime = new TimeOnly(5, 0); //9:00 AM 
                int lateMinutes = 0;
                // if the user is late 
                if (today.TimeOfDay > scheduledStartTime.ToTimeSpan())
                {
                    lateMinutes = (int)((today.TimeOfDay  - scheduledStartTime.ToTimeSpan()).TotalMinutes);

                }

                var currentMonth = today.Month;
                var currentYear = today.Year;

                var lateMinutesRecord = await _context.LateMinutesLeft
                        .FirstOrDefaultAsync(l => l.EmployeeId == int.Parse(employeeId) && l.Month.Month == currentMonth && l.Month.Year == currentYear);

                if (lateMinutesRecord == null)
                {
                    lateMinutesRecord = new LateMinutesLeft
                    {
                        EmployeeId = int.Parse(employeeId),
                        MinutesLeft = 30 - lateMinutes,
                        Time = new TimeOnly(),
                        Month = new DateTime(currentYear, currentMonth, 1)
                    };
                    
                    await _context.LateMinutesLeft.AddAsync(lateMinutesRecord);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    lateMinutesRecord.MinutesLeft -= lateMinutes;
                }

                if (lateMinutesRecord.MinutesLeft < 0)
                {
                    lateMinutesRecord.MinutesLeft = 0;
                }
                _context.LateMinutesLeft.Update(lateMinutesRecord);
              
                await _context.SaveChangesAsync();

                return Created(nameof(GetLateMinutes), lateMinutesRecord);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPost("CheckOutEmployee")]
        [ProducesResponseType(typeof(IActionResult), 201)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CheckOutEmployee()
        {
            try
            {
                var employeeId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (employeeId == null)
                {
                    return Unauthorized();
                }

                var date = DateTime.Now;
                var todayAttendance = await _context.Attendances
                    .FirstOrDefaultAsync(attendance =>
                        attendance.EmployeeId == int.Parse(employeeId) &&
                        attendance.CheckInTime.Date == date.Date &&
                        attendance.CheckOutTime == null);

                if (todayAttendance == null)
                {
                    return BadRequest("No check-in record found for today.");
                }

                todayAttendance.CheckOutTime = date;

                _context.Attendances.Update(todayAttendance);
                await _context.SaveChangesAsync();
                return Ok(new { Id = todayAttendance.ID });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("Leave")]
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
        [HttpGet("GetTodayAttendance")]
        [ProducesResponseType(typeof(IEnumerable<Attendance>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTodayAttendance()
        {
            try
            {
                var employeeId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (employeeId == null)
                {
                    return Unauthorized();
                }

                var today = DateTime.Now.Date;

                var todayAttendances = await _context.Attendances
                    .Where(a => a.EmployeeId == int.Parse(employeeId) && a.CheckInTime.Date == today)
                    .ToListAsync();

                return Ok(todayAttendances);
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

        [HttpGet("Leave")]
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
        [ProducesResponseType(typeof(IEnumerable<EmployeeRes>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var employees = await _context.Employees
                    .Include(e => e.DepartmentId)
                    .ToListAsync();

                var employeeResponses = employees.Select(e => new EmployeeRes
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,
                    DOB = e.DOB,
                    Gender = e.Gender.ToString(),
                    ProfilePicURL = e.ProfilePicURL,
                    NFCIdNumber = e.NFCIdNumber,
                    PositionName = e.PositionName,
                    DepartmentName = e.DepartmentId != null ? new Department { ID = e.DepartmentId.ID, DepartmentName = e.DepartmentId.DepartmentName } : null,
                    PointEarned = e.PointEarned,
                    IsAdmin = e.IsAdmin
                }).ToList();

                return Ok(employeeResponses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("AddCertificate")]
        [ProducesResponseType(typeof(Certificate), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddCertificate([FromBody] AddCertificate certificateDto)
        {
            if (certificateDto == null)
            {
                return BadRequest("Certificate data is null.");
            }

            try
            {
                var employeeId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (employeeId == null)
                {
                    return Unauthorized();
                }

                var certificate = new Certificate
                {
                    EmployeeId = int.Parse(employeeId),
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
    }


}
