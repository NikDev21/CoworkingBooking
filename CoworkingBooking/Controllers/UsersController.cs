using Microsoft.AspNetCore.Mvc;
using CoworkingBooking.Data;
using CoworkingBooking.Models;
using Microsoft.EntityFrameworkCore;
using CoworkingBooking.Models.DTO;



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
    }
}
