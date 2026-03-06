using Employee.api.Model;
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
        public DepartmentMasterController (EmployeeDbContext context)
        {
            _Context = context;
        }

        [HttpGet("GetAllDepartments")]
        public IActionResult GetDepartment()
        {
            var depList = _Context.Departments.ToList();
            return Ok(depList);
        }

        [HttpPost("AddDepartment")]
        public IActionResult AddDepartment([FromBody] Department dept)
        {
            bool exists = _Context.Departments
                .Any(d => d.departmentName == dept.departmentName.ToLower());

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

        [HttpPut("UpdateDepartment")]
        public IActionResult UpdateDepartment([FromBody]Department dept)
        {
            var existingDept = _Context.Departments.Find(dept.departmentId);
            if(existingDept == null)
            {
                return NotFound("Department Not Found");
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

        [HttpDelete("DeleteDepartment/{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            var dept = _Context.Departments.Find(id);
            if(dept == null)
            {
                return NotFound("Department Not Found");
            }
            _Context.Departments.Remove(dept);
            _Context.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "Department Deleted Succesfully"
            });
        }
    }
}
