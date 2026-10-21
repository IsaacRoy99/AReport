using ForgeReportSystem.Api.Data;
using ForgeReportSystem.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForgeReportSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(new
            {
                message = "Email is required."
            });
        }

        var user = await _context.Users
            .Include(u => u.Department)
            .FirstOrDefaultAsync(u =>
                u.Email.ToLower() == request.Email.ToLower());

        if (user == null)
        {
            return Unauthorized(new
            {
                message = "Invalid email address."
            });
        }

        return Ok(new
        {
            message = "Login successful.",

            user = new
            {
                id = user.Id,
                name = user.FullName,
                email = user.Email,
                role = user.Role,
                departmentId = user.DepartmentId,
                department = user.Department?.Name,
                site = user.Department?.Site
            }
        });
    }
}