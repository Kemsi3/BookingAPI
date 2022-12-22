using BookingAPI.Models;

namespace BookingAPI.IServices
{
    public interface IDeskRepository
    {
        Task<IResult> GetAllDesks();

        Task<IResult> GetDeskByOfficeId(string locationId);

        Task<IResult> AddDesk(Desk desk);

        Task<IResult> DeleteDeskById(string deskId);
    }
}
