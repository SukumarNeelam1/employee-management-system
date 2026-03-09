using Employee.api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employee.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentMasterController : ControllerBase
    {
        // Dependecy Injection
        public readonly EmployeeDbContext _Context;
        public DepartmentMasterController(EmployeeDbContext context)
        {
            _Context = context;
        }

        [HttpGet("GetAllDepartments")]
        public IActionResult GetDepartment()
        {
            var depList = _Context.Departments.ToList();
            return Ok(depList);
        }


        [Authorize(Roles = "HR")]
        [HttpPost("AddDepartment")]
        public IActionResult AddDepartment([FromBody] Department dept)
        {
            bool exists = _Context.Departments
                .Any(d => d.departmentName.ToLower() == dept.departmentName.Trim().ToLower());

            if (exists)
            {
                return BadRequest("Department name must be unique");
            }

            _Context.Departments.Add(dept);
            _Context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Department Created Successfully"
            });
        }

        [Authorize(Roles = "HR")]
        [HttpPut("UpdateDepartment")]
        public IActionResult UpdateDepartment([FromBody] Department dept)
        {
            var existingDept = _Context.Departments.Find(dept.departmentId);
            if (existingDept == null)
            {
                return NotFound("Department Not Found");
            }

            bool exists = _Context.Departments
                .Any(d => d.departmentName.ToLower() == dept.departmentName.ToLower()
                       && d.departmentId != dept.departmentId);

            if (exists)
            {
                return BadRequest("Department name must be unique");
            }

            existingDept.departmentName = dept.departmentName;
            existingDept.isActive = dept.isActive;

            _Context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Department Updated Successfully"
            });
        }
        [Authorize(Roles = "HR")]
        [HttpDelete("{id}")]
        public IActionResult DeleteDesignation(int id)
        {
            var designation = _Context.Designations.Find(id);

            if (designation == null)
                return NotFound("Designation not found");

            // CHECK IF USED BY EMPLOYEE
            bool usedByEmployee = _Context.Employees.Any(e => e.designationId == id);

            if (usedByEmployee)
                return BadRequest("Cannot delete designation because employees are assigned to it.");

            _Context.Designations.Remove(designation);
            _Context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Designation Deleted Successfully"
            });
        }
    }
}