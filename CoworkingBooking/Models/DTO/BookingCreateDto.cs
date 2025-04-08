namespace CoworkingBooking.Models.DTO
{
    public class BookingCreateDto
    {
        public int UserID { get; set; }
        public int WorkspaceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
