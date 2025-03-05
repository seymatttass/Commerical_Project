using System.Linq.Expressions;

namespace Product.API.Data.Repository.CategoryRepository
{
    public interface ICategoryRepository
    {
        Task<Entities.Category> GetByIdAsync(int id);

        //Müşterinin tüm Category görüntülemek için kullanılabilir
        Task<IEnumerable<Entities.Category>> GetAllAsync();

        //Belirli koşulları sağlayan Category kayıtlarını bulmak için kullanılır. 
        Task<IEnumerable<Entities.Category>> FindAsync(Expression<Func<Entities.Category, bool>> predicate);

        //Kullanıcılar yeni Category eklediklerinde
        Task AddAsync(Entities.Category entity);
        Task AddRangeAsync(IEnumerable<Entities.Category> entities);

        //Category sildiklerinde
        Task<bool> RemoveAsync(int id);
        Task RemoveRangeAsync(IEnumerable<Entities.Category> entities);

        //Category bilgilerini güncellediklerinde
        Task<bool> UpdateAsync(Entities.Category entity);

        //Belirli bir ID'ye sahip Category kaydının veritabanında var olup olmadığını kontrol etmek için kullanılır. 
        Task<bool> ExistsAsync(int id);
    }
}
