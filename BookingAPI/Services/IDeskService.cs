using BookingAPI.Models;

namespace BookingAPI.IServices
{
    public interface IDeskService
    {
        Task<IResult> GetAllDesks();

        Task<IResult> GetAvailableDesks(DateTime startDate, DateTime endDate);

        Task<IResult> GetDeskByOfficeId(string locationId);

        Task<IResult> AddDesk(Desk desk);

        Task<IResult> DeleteDeskById(string deskId);
    }
}
