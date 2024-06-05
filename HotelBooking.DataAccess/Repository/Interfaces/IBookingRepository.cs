using HotelBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.DataAccess.Repository.Interfaces
{
	public interface IBookingRepository : IRepository<BookedRoom>
	{
		Task Update(BookedRoom bookedRoom);
	}
}
