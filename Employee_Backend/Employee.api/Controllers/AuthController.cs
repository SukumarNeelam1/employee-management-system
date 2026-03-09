using Employee.api.DTOs;
using Employee.api.Model;
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
    public class AuthController : ControllerBase
    {
        private readonly EmployeeDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(EmployeeDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var emailExists = await _context.Employees
                .AnyAsync(x => x.email == model.Email);

            if (emailExists)
                return BadRequest("Email already exists");

            var user = new EmployeeModel
            {
                name = model.Name,
                email = model.Email,
                passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                role = "User",
                createdDate = DateTime.Now,
                modifiedDate = DateTime.Now
            };

            _context.Employees.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "User registered successfully"
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var user = await _context.Employees
                .FirstOrDefaultAsync(x => x.email == model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.passwordHash))
                return Unauthorized("Invalid Credentials");

            // key present inside the app.settings.json file
            var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.email),
                new Claim(ClaimTypes.Role, user.role)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                employeeId = user.employeeId,
                name = user.name,
                role = user.role
            });
        }
    }
}