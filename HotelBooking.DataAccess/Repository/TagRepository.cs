using HotelBooking.DataAccess.Data;
using HotelBooking.DataAccess.Repository.Interfaces;
using HotelBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.DataAccess.Repository
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        private readonly PostgresDbContext _dbContext;
        public TagRepository(PostgresDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(Tag tag) 
        {
            _dbContext.Tags.Update(tag);
        }
    }
}
