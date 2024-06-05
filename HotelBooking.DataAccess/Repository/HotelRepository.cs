using HotelBooking.DataAccess.Data;
using HotelBooking.DataAccess.Repository.Interfaces;
using HotelBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.DataAccess.Repository
{
	public class HotelRepository : Repository<Hotel>, IHotelRepository
	{
		private readonly PostgresDbContext _context;
        public HotelRepository(PostgresDbContext context) : base(context) 
        {
			_context = context;
        }

        public async Task Update(Hotel hotel)
		{
			Hotel? _hotelFromDb = await _context.Hotels.FirstOrDefaultAsync(h => h.Id == hotel.Id);
			if(_hotelFromDb != null)
			{
				_hotelFromDb.Name = hotel.Name;
				_hotelFromDb.Country = hotel.Country;
				_hotelFromDb.City = hotel.City;
				_hotelFromDb.Street = hotel.Street;
				_hotelFromDb.Description = hotel.Description;
				_hotelFromDb.Rating = hotel.Rating;
				_hotelFromDb.TagsID = hotel.TagsID;
				_hotelFromDb.phoneNumber = hotel.phoneNumber;
				_hotelFromDb.Email = hotel.Email;
				_hotelFromDb.siteUrl = hotel.siteUrl;

				if (hotel.ImagesUrl != null)
				{
					_hotelFromDb.ImagesUrl = hotel.ImagesUrl;
				}
			}
		}
	}
}
