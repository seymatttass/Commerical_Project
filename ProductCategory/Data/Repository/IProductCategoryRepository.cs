using System.Linq.Expressions;

namespace ProductCategory.Data.Repository
{
    public interface IProductCategoryRepository 
    {
        Task<Entities.ProductCategory> GetByIdAsync(int id);

        //Müşterinin tüm ProductCategory görüntülemek için kullanılabilir
        Task<IEnumerable<Entities.ProductCategory>> GetAllAsync();

        //Belirli koşulları sağlayan ProductCategory kayıtlarını bulmak için kullanılır. 
        Task<IEnumerable<Entities.ProductCategory>> FindAsync(Expression<Func<Entities.ProductCategory, bool>> predicate);

        //Kullanıcılar yeni ProductCategory eklediklerinde
        Task AddAsync(Entities.ProductCategory entity);
        Task AddRangeAsync(IEnumerable<Entities.ProductCategory> entities);

        //ProductCategory sildiklerinde
        Task<bool> RemoveAsync(int id);
        Task RemoveRangeAsync(IEnumerable<Entities.ProductCategory> entities);

        //ProductCategory bilgilerini güncellediklerinde
        Task<bool> UpdateAsync(Entities.ProductCategory entity);

        //Belirli bir ID'ye sahip ProductCategory kaydının veritabanında var olup olmadığını kontrol etmek için kullanılır. 
        Task<bool> ExistsAsync(int id);
    }
}
