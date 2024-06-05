

namespace HotelBooking.DataAccess.Repository.Interfaces
{
	public interface IUnitOfWork
	{
		IHotelRepository Hotel { get; }
		ITagRepository Tag { get; }
		IRoomRepository Room { get; }
		IBookingRepository BookingRoom { get; }
		Task SaveAsync();
	}
}
