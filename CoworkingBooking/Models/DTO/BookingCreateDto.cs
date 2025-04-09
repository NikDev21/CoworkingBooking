namespace CoworkingBooking.Models.DTO
{
    public class BookingCreateDto
    {
        public int UserID { get; set; }
        public int WorkspaceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsPaid { get; set; } = true;

        // Итоговая стоимость — может рассчитываться на фронте, но лучше пересчитать и на бэке
        public decimal? TotalPrice { get; set; }
    }
}
