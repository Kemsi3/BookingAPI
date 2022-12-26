using BookingAPI.Models;

namespace BookingAPI.Services
{
    public interface IUserService
    {
        User Get(UserLogin userLogin);
    }
}
