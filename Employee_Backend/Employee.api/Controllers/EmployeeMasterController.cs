using Employee.api.Model;
using Employee.api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Employee.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeMasterController : ControllerBase
    {
        private readonly EmployeeDbContext _Context;
        private readonly IConfiguration _config;

        public EmployeeMasterController(EmployeeDbContext context,IConfiguration config)
        {
            _Context = context;
            _config = config;
        }

        // 🔹 GET ALL
        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var data = (from emp in _Context.Employees

                            join des in _Context.Designations
                            on emp.designationId equals des.designationId into desGroup
                            from des in desGroup.DefaultIfEmpty()

                            join dept in _Context.Departments
                            on des.departmentId equals dept.departmentId into deptGroup
                            from dept in deptGroup.DefaultIfEmpty()

                            select new
                            {
                                emp.employeeId,
                                emp.name,
                                emp.email,
                                emp.contactNo,
                                emp.state,
                                emp.pincode,
                                emp.role,

                                designationName = des != null ? des.designationName : null,
                                departmentName = dept != null ? dept.departmentName : null
                            }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // 🔹 GET BY ID
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var emp = _Context.Employees.Find(id);

                if (emp == null)
                    return NotFound("Employee not found");

                return Ok(emp);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // 🔹 ADD
        [Authorize(Roles = "HR")]
        [HttpPost]
        public IActionResult Add([FromBody] EmployeeCreateDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Invalid employee data");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Email duplicate check
                bool emailExists = _Context.Employees
                    .Any(e => e.email == dto.email);

                if (emailExists)
                    return BadRequest("Email already exists");

                // Contact number duplicate check (only if provided)
                if (!string.IsNullOrEmpty(dto.contactNo))
                {
                    bool contactExists = _Context.Employees
                        .Any(e => e.contactNo == dto.contactNo);

                    if (contactExists)
                        return BadRequest("Contact number already exists");
                }

                // Convert DTO → Model
                var emp = new EmployeeModel
                {
                    name = dto.name,
                    email = dto.email,
                    //passwordHash = dto.password,
                    contactNo = dto.contactNo,
                    city = dto.city,
                    state = dto.state,
                    pincode = dto.pincode,
                    altContactNo = dto.altContactNo,
                    address = dto.address,
                    designationId = dto.designationId,

                    role = "Employee",
                    createdDate = DateTime.Now,
                    modifiedDate = DateTime.Now
                };

                _Context.Employees.Add(emp);
                _Context.SaveChanges();

                return Ok(new
                {
                    success = true,
                    message = "Employee Added Successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        // 🔹 UPDATE
        [Authorize(Roles ="HR")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] EmployeeModel emp)
        {
            try
            {
                var existing = _Context.Employees.Find(id);

                if (existing == null)
                    return NotFound("Employee not found");

                existing.name = emp.name;
                existing.contactNo = emp.contactNo;
                existing.email = emp.email;
                existing.city = emp.city;
                existing.state = emp.state;
                existing.pincode = emp.pincode;
                existing.altContactNo = emp.altContactNo;
                existing.address = emp.address;
                existing.designationId = emp.designationId;
                existing.modifiedDate = DateTime.Now;

                _Context.SaveChanges();

                return Ok(new { message = "Employee Updated Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // 🔹 DELETE
        [Authorize(Roles ="HR")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var emp = _Context.Employees.Find(id);

                if (emp == null)
                    return NotFound("Employee not found");

                _Context.Employees.Remove(emp);
                _Context.SaveChanges();

                return Ok("Employee Deleted Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}