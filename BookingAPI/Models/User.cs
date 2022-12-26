namespace BookingAPI.Models
{
    public class User
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }  

        public string Password { get; set; }

        public string Role{ get; set; }   

    }
}
