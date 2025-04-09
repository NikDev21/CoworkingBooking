using Microsoft.AspNetCore.Mvc;
using CoworkingBooking.Data;
using CoworkingBooking.Models;
using Microsoft.EntityFrameworkCore;
using CoworkingBooking.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;



namespace CoworkingBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext context;

        private readonly ILogger<UsersController> logger;

        public UsersController(AppDbContext context, ILogger<UsersController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet] // Get all users
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            logger.LogInformation("Get all users");
            return await context.Users.ToListAsync();
        }

        [HttpGet("{id}")] // Get user by id
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            return user;
        }
        [HttpPost] // Add(Create) new user
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")] // Update user
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto userDto)
        {
            var user = await context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            if (userDto.Name != null)
                user.Name = userDto.Name;

            if (userDto.Email != null)
                user.Email = userDto.Email;

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")] // Delete user
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await context.Users.FindAsync(userId);

            if (user == null)
                return NotFound();



            return Ok(new
            {
                user.Id,
                user.Email,
                user.Name,
                RegisteredAt = user.CreatedAt
            });
        }
        [HttpGet("me/payments")]
        [Authorize]
        public async Task<IActionResult> GetMyPayments()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var payments = await context.Payments
                .Where(p => p.UserId == userId)
                .Include(p => p.Booking)
                .OrderByDescending(p => p.PaidAt)
                .Select(p => new {
                    p.Id,
                    p.Amount,
                    p.Status,
                    p.PaidAt,
                    Start = p.Booking.StartDate,
                    End = p.Booking.EndDate
                })
                .ToListAsync();

            return Ok(payments);
        }
    }
}
