﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KFHRBackEnd.Models.Entites;
using KFHRBackEnd.Models.Entites.Request.Department;
using KFHRBackEnd.Models.Entites.Request;

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


        [HttpPost("AddCertificate")]
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

                var employee = new Employee
                {
                    Name = employeeDto.Name,
                    Email = employeeDto.Email,
                    Password = hashedPassword,
                    DOB = employeeDto.DOB,
                    Gender = employeeDto.Gender,
                    ProfilePicURL = employeeDto.ProfilePicURL,
                    NFCIdNumber = employeeDto.NFCIdNumber,
                    PositionId = employeeDto.PositionId,
                    DepartmentId = employeeDto.DepartmentId,
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

                if (!string.IsNullOrEmpty(employeeDto.ProfilePicURL))
                {
                    existingEmployee.ProfilePicURL = employeeDto.ProfilePicURL;
                }

                existingEmployee.NFCIdNumber = employeeDto.NFCIdNumber;
                existingEmployee.PositionId = employeeDto.PositionId;
                existingEmployee.DepartmentId = employeeDto.DepartmentId;
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
