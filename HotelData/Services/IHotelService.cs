using HotelData.DTOs;

namespace HotelData.Services
{
    public interface IHotelService
    {
        Task GetHotelDataFromSources();
        Task<IList<HotelDTO>> GetAllHotels();
    }
}
