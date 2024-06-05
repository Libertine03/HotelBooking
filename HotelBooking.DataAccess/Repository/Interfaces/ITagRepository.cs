using HotelBooking.Models;

namespace HotelBooking.DataAccess.Repository.Interfaces
{
    public interface ITagRepository : IRepository<Tag>
    {
        void Update(Tag tag);
    }
}
