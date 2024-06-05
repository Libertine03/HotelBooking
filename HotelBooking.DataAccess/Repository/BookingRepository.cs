using HotelBooking.DataAccess.Data;
using HotelBooking.DataAccess.Repository.Interfaces;
using HotelBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.DataAccess.Repository
{
	public class BookingRepository : Repository<BookedRoom>, IBookingRepository
	{
		private readonly PostgresDbContext _context;
        public BookingRepository(PostgresDbContext context) : base(context)
        {
			_context = context;
        }

		public async Task Update(BookedRoom bookedRoom)
		{
			BookedRoom? _bookedRoom = await _context.BookedRooms.FirstOrDefaultAsync(br => br.Id == bookedRoom.Id);
			if(_bookedRoom != null)
			{

			}
		}
	}
}
