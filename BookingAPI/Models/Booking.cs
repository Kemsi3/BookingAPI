namespace BookingAPI.Models
{
    public class Booking
    {
        public Guid BookingId { get; set; }

        public string DeskId { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
