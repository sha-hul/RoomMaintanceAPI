using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RoomMaintenanceAPI.DTO;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RoomMaintenanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private Common objcom = new Common();
        public LoginController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO model)
        {
            if (string.IsNullOrWhiteSpace(model.EmpId) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest(new
                {
                    status = false,
                    errorcode = 101,
                    message = "Employee ID and Password are required"
                });
            }

            var user = _context.Users.FirstOrDefault(x => x.EmpId.ToLower() == model.EmpId.ToLower());

            if (user == null)
            {
                return NotFound(new
                {
                    status = false,
                    errorcode = 102,
                    message = "Employee ID not found"
                });
            }

            var hashed = objcom.HashPassword(model.Password);

            if (user.Password != hashed)
            {
                return Unauthorized(new
                {
                    status = false,
                    errorcode = 103,
                    message = "Incorrect password"
                });
            }

            // Create JWT token
            var claims = new[]
            {
                new Claim("empID", user.EmpId),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            // Store JWT in HttpOnly cookie
            Response.Cookies.Append("auth_token", jwtToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,          
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(2)
            });

            // Return user info (NOT the token)
            return Ok(new
            {
                status = true,
                errorcode = 0,
                message = "Login successful",
                role = user.Role,
                empId = user.EmpId,
                empName = user.EmpName,
                mail = user.Mail
            });
        }

    }

}