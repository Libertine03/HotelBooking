using HotelBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.DataAccess.Repository.Interfaces
{
	public interface IRoomRepository : IRepository<Room>
	{
		Task Update(Room Room);
	}
}
