
using HotelBooking.DataAccess.Data;
using HotelBooking.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelBooking.DataAccess.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly PostgresDbContext _dbContext;
		internal DbSet<T> _dbSet;
        public Repository(PostgresDbContext context)
        {
			_dbContext = context;
			_dbSet = _dbContext.Set<T>();
			_dbContext.Hotels.Include(u => u.TagsID).Include(u => u.Tags).Include(u => u.Rooms).ToListAsync();
        }

		public async Task<T> AddAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
			return entity;
		}

		public async Task<IEnumerable<T>> GetAllAsync(string? includeProp = null)
		{
			IQueryable<T> query = _dbSet;
			if(!string.IsNullOrEmpty(includeProp))
			{
				foreach(var prop in includeProp.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(prop);
				}
			}
			return await query.ToListAsync();
        }

		public async Task<T> GetAsync(Expression<Func<T, bool>> filter, string? includeProp = null)
		{
			IQueryable<T> query = _dbSet;
			query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProp))
            {
                foreach (var prop in includeProp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);
                }
            }

            return await query.FirstOrDefaultAsync();
		}

		public Task RemoveAsync(T entity)
		{
			_dbSet.Remove(entity);
			return Task.CompletedTask;
		}

		public Task RemoveRangeAsync(IEnumerable<T> entities)
		{
			_dbSet.RemoveRange(entities);
			return Task.CompletedTask;
		}
	}
}
