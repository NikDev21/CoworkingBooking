namespace CoworkingBooking.Models.DTO
{
    public class BookingUpdateDto
    {
        public int WorkspaceId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Симуляция оплаты (в реальном случае пришёл бы payment token)
        public bool IsPaid { get; set; } = true;

        // Итоговая стоимость — может рассчитываться на фронте, но лучше пересчитать и на бэке
        public decimal? TotalPrice { get; set; }


    }
}
