using Microsoft.AspNetCore.Mvc;
using CoworkingBooking.Data;

namespace CoworkingBooking.Controllers
{
    [Route("api/database")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly AppDbContext context;

        public DatabaseController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet("test")]
        public IActionResult TestConnection()
        {
            try
            {
                var usersCount = context.Users.Count();
                return Ok($"✅ Подключение успешно! Количество пользователей: {usersCount}");
            }
            catch (Exception ex)
            {
                return BadRequest($"❌ Ошибка подключения: {ex.Message}");
            }
        }
    }
}
