using Microsoft.AspNetCore.Mvc;
using CoworkingBooking.Data;
using CoworkingBooking.Models;
using Microsoft.EntityFrameworkCore;
using CoworkingBooking.Models.DTO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CoworkingBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext context;
        public BookingsController(AppDbContext context)
        {
            this.context = context;
        }
        [HttpGet] // Get all bookings
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await context.Bookings
                .Include(b => b.User)
                .Include(b => b.Workspace)
                .ToListAsync();
        }
        [HttpGet("{id}")] // Get booking by id
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();
            return booking;
        }
        [HttpPost] // Add(Create) new booking
        public async Task<ActionResult<Booking>> CreateBooking([FromBody] BookingCreateDto bookingDto)
        {
            if (bookingDto.StartDate < DateTime.UtcNow || bookingDto.EndDate <= bookingDto.StartDate)
                return BadRequest("You cannot create booking in the past or with incorrect time.");

            var duration = bookingDto.EndDate - bookingDto.StartDate;
            if (duration.TotalMinutes < 30 || duration.TotalHours > 8)
                return BadRequest("Booking duration must be from 30 minutes till 8 hours.");

            bool hasConflict = await context.Bookings.AnyAsync(b =>
                b.WorkspaceId == bookingDto.WorkspaceId &&
                ((bookingDto.StartDate >= b.StartDate && bookingDto.StartDate < b.EndDate) ||
                 (bookingDto.EndDate > b.StartDate && bookingDto.EndDate <= b.EndDate) ||
                 (bookingDto.StartDate <= b.StartDate && bookingDto.EndDate >= b.EndDate)));

            if (hasConflict)
                return Conflict("This workspace is already booked for the selected time.");

            // 3. Расчёт стоимости
            var pricePer30Min = 2.00m;
            var totalBlocks = Math.Ceiling(duration.TotalMinutes / 30);
            var calculatedPrice = (decimal)totalBlocks * pricePer30Min;

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var booking = new Booking
            {
                UserID = userId,
                WorkspaceId = bookingDto.WorkspaceId,
                StartDate = bookingDto.StartDate,
                EndDate = bookingDto.EndDate,
                TotalPrice = calculatedPrice,
                IsPaid = bookingDto.IsPaid
            };

            context.Bookings.Add(booking);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
        }
        [HttpPut("{id}")] // Update booking
        public async Task<IActionResult> UpdateBooking(int id, BookingUpdateDto bookingDto)
        {
            var booking = await context.Bookings.FindAsync(id);
            if (bookingDto.StartDate != null)
                booking.StartDate = bookingDto.StartDate.Value;
            if (bookingDto.EndDate != null)
                booking.EndDate = bookingDto.EndDate.Value;
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")] // Delete booking
        public async Task<IActionResult> CancleBooking(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var booking = await context.Bookings.FirstOrDefaultAsync(b => b.Id == id && b.UserID == userId);
            if (booking == null)
                return NotFound();

            context.Bookings.Remove(booking);
            await context.SaveChangesAsync();

            return NoContent();
        }

        

        [HttpGet("mine")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<object>>> GetMyBookings()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var bookings = await context.Bookings
                .Include(b => b.Workspace)
                .Where(b => b.UserID == userId)
                .OrderByDescending(b => b.StartDate)
                .Select(b => new {
                    b.Id,
                    b.StartDate,
                    b.EndDate,
                    b.TotalPrice,
                    b.IsPaid,
                    WorkspaceName = b.Workspace.Name,
                    WorkspaceLocation = b.Workspace.Location
                })
                .ToListAsync();

            return Ok(bookings);
        }

        [HttpGet("occupied")]
        public async Task<ActionResult<IEnumerable<object>>> GetOccupiedSlots(int workspaceId, DateTime date)
        {
            var start = date.Date;
            var end = start.AddDays(1);

            var bookings = await context.Bookings
                .Where(b => b.WorkspaceId == workspaceId &&
                            b.StartDate >= start &&
                            b.EndDate <= end)
                .Select(b => new { b.StartDate, b.EndDate })
                .ToListAsync();

            return Ok(bookings);
        }
        
       
    }
}
