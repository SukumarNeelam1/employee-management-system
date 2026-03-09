using Employee.api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationController : ControllerBase
    {
        private readonly EmployeeDbContext _Context;

        public DesignationController(EmployeeDbContext context)
        {
            _Context = context;
        }

        // 🔹 GET ALL (WITH SIMPLE PAGINATION)
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var data = _Context.Designations
                    .Include(d => d.Department)
                    .Select(d => new
                    {
                        d.designationId,
                        d.designationName,
                        d.departmentId,
                        DepartmentName = d.Department.departmentName
                    })
                    .ToList();

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
                var designation = _Context.Designations.Find(id);

                if (designation == null)
                    return NotFound("Designation not found");

                return Ok(designation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // 🔹 ADD
        [Authorize(Roles = "HR")]
        [HttpPost]
        public IActionResult Add([FromBody] Designation model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Duplicate check (based on designationName)
                bool exists = _Context.Designations
                    .Any(d => d.designationName == model.designationName);

                if (exists)
                    return BadRequest("Designation name already exists");

                _Context.Designations.Add(model);
                _Context.SaveChanges();

                return Ok(new { message = "Designation Added Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // 🔹 UPDATE
        [Authorize(Roles = "HR")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Designation model)
        {
            try
            {
                var existing = _Context.Designations.Find(id);

                if (existing == null)
                    return NotFound("Designation not found");

                existing.departmentId = model.departmentId;
                existing.designationName = model.designationName;

                _Context.SaveChanges();

                return Ok(new { message = "Designation Updated Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // 🔹 DELETE
        // 🔹 DELETE
        [Authorize(Roles = "HR")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var designation = _Context.Designations.Find(id);

                if (designation == null)
                    return NotFound("Designation not found");

                bool usedByEmployee = _Context.Employees.Any(e => e.designationId == id);

                if (usedByEmployee)
                    return BadRequest(new { message = "Cannot delete designation because employees are assigned to it." });

                _Context.Designations.Remove(designation);
                _Context.SaveChanges();

                return Ok(new { message = "Designation Deleted Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
} 