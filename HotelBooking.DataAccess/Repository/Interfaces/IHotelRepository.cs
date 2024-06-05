using HotelBooking.Models;

namespace HotelBooking.DataAccess.Repository.Interfaces
{
	public interface IHotelRepository : IRepository<Hotel>
	{
		Task Update(Hotel hotel);
	}
}
