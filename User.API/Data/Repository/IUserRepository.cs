using System.Linq.Expressions;
using Users.API.Data.Entities;

namespace Users.API.Data.Repository
{
    public interface IUserRepository
    {
        Task<Userss> GetByIdAsync(int id);

        Task<IEnumerable<Userss>> GetAllAsync();

        Task<IEnumerable<Userss>> FindAsync(Expression<Func<Userss, bool>> predicate);

        Task AddAsync(Userss entity);

        Task AddRangeAsync(IEnumerable<Userss> entities);

        Task<bool> RemoveAsync(int id);

        Task RemoveRangeAsync(IEnumerable<Userss> entities);

        Task<bool> UpdateAsync(Userss entity);

        Task<bool> ExistsAsync(int id);

        Task<Userss?> AuthenticateAsync(string username, string password);
    }
}
