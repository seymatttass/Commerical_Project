using System.Linq.Expressions;

namespace Shipping.API.Data.Repository
{
    public interface IShippingRepository
    {
        Task<Entities.Shipping> GetByIdAsync(int id);

        //Müşterinin tüm nakliye görüntülemek için kullanılabilir
        Task<IEnumerable<Entities.Shipping>> GetAllAsync();

        //Belirli koşulları sağlayan nakliye kayıtlarını bulmak için kullanılır. 
        Task<IEnumerable<Entities.Shipping>> FindAsync(Expression<Func<Entities.Shipping, bool>> predicate);

        //Kullanıcılar yeni nakliye eklediklerinde
        Task AddAsync(Entities.Shipping entity);
        Task AddRangeAsync(IEnumerable<Entities.Shipping> entities);

        //nakliye sildiklerinde
        Task<bool> RemoveAsync(int id);
        Task RemoveRangeAsync(IEnumerable<Entities.Shipping> entities);

        //nakliye bilgilerini güncellediklerinde
        Task<bool> UpdateAsync(Entities.Shipping entity);

        //Belirli bir ID'ye sahip nakliye kaydının veritabanında var olup olmadığını kontrol etmek için kullanılır. 
        Task<bool> ExistsAsync(int id);
    }
}
