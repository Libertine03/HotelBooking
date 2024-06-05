using System.Linq.Expressions;
namespace HotelBooking.DataAccess.Repository.Interfaces
{
	public interface IRepository<T>
	{
		Task<T> GetAsync(Expression<Func<T, bool>> filter, string? includeProp = null);	
		Task<IEnumerable<T>> GetAllAsync(string? includeProp = null);
		Task<T> AddAsync(T entity);
		Task RemoveAsync(T entity);
		Task RemoveRangeAsync(IEnumerable<T> entities);
	}
}
