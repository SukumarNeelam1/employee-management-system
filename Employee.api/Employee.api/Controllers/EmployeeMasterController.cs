using Employee.api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeMasterController : ControllerBase
    {
        private readonly EmployeeDbContext _Context;

        public EmployeeMasterController(EmployeeDbContext context)
        {
            _Context = context;
        }

        // 🔹 GET ALL
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var data = (from emp in _Context.Employees
                            join des in _Context.Designations
                            on emp.designationId equals des.designationId

                            join dept in _Context.Departments
                            on des.departmentId equals dept.departmentId

                            select new
                            {
                                emp.employeeId,
                                emp.name,
                                emp.email,
                                emp.contactNo,
                                emp.city,
                                emp.state,
                                emp.pincode,
                                emp.address,
                                emp.role,

                                emp.designationId,
                                des.designationName,

                                dept.departmentId,
                                dept.departmentName,

                                emp.createdDate,
                                emp.modifiedDate
                            }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // 🔹 GET BY ID
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
        [HttpPost]
        public IActionResult Add([FromBody] EmployeeModel emp)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (emp == null)
                    return BadRequest("Invalid employee data");

                bool employeeExists = _Context.Employees
                    .Any(e => e.contactNo == emp.contactNo || e.email == emp.email);

                // Check duplicate contactNo
                bool contactExists = _Context.Employees
                    .Any(e => e.contactNo == emp.contactNo);

                if (contactExists)
                    return BadRequest("Contact number already exists");

                // Check duplicate email
                bool emailExists = _Context.Employees
                    .Any(e => e.email == emp.email);

                if (employeeExists)
                    return BadRequest("Employee already exists");

                if (emailExists)
                    return BadRequest("Email already exists");

                emp.createdDate = DateTime.Now;
                emp.modifiedDate = DateTime.Now;

                _Context.Employees.Add(emp);
                _Context.SaveChanges();

                return Ok(new { message = "Employee Added Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // 🔹 UPDATE
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

                return Ok(new {message ="Employee Updated Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // 🔹 DELETE
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Find user by email
                var user = await _Context.Employees
                    .FirstOrDefaultAsync(x => x.email == model.email && x.contactNo == model.contactNo);

                // If user not found
                if (user == null)
                    return Unauthorized(new { Message = "Invalid Credentials" });

                // Success response
                return Ok(new
                {
                    message = "Login Successful",
                    data = new
                    {
                        user.employeeId,
                        user.name,
                        user.email,
                        user.contactNo,
                        user.designationId,
                        //user.designationName,
                        user.role
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}