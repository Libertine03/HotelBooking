using HotelBooking.DataAccess.Data;
using HotelBooking.DataAccess.Repository.Interfaces;

namespace HotelBooking.DataAccess.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		public IHotelRepository Hotel { get ; private set; }
        public ITagRepository Tag { get; private set; }
		public IRoomRepository Room { get; private set; }
		public IBookingRepository BookingRoom { get; private set; }

		private readonly PostgresDbContext _context;
        public UnitOfWork(PostgresDbContext context)
        {
			_context = context;
			Hotel = new HotelRepository(_context);
			Tag = new TagRepository(_context);
			Room = new RoomRepository(_context);
			BookingRoom = new BookingRepository(_context);
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();


		private bool _disposed;
		public async ValueTask DisposeAsync()
		{
			await DisposeAsync(true);
			GC.SuppressFinalize(this);
		}
		protected virtual async ValueTask DisposeAsync(bool disposing)
		{
			if(!_disposed)
			{
				if(disposing)
				{
					await _context.DisposeAsync();
				}

				_disposed = true;
			}
		}
	}
}
