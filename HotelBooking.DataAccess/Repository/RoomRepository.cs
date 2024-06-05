using HotelBooking.DataAccess.Data;
using HotelBooking.DataAccess.Repository.Interfaces;
using HotelBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.DataAccess.Repository
{
	public class RoomRepository : Repository<Room>, IRoomRepository
	{
        private readonly PostgresDbContext _context;
        public RoomRepository(PostgresDbContext context) : base(context)
        {
            _context = context;
        }

		public async Task Update(Room Room)
		{
			Room? room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == Room.Id);
			if (room != null)
			{
				room.RoomName = Room.RoomName;
				room.Price = Room.Price;
				room.Tags = Room.Tags;
				room.HotelId = Room.HotelId;

				if(Room.ImagesUrl != null)
				{
					room.ImagesUrl = Room.ImagesUrl;
				}
			}
		}
	}
}
