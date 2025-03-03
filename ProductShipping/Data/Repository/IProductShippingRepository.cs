using System.Linq.Expressions;

namespace ProductShipping.Data.Repository
{
    public interface IProductShippingRepository
    {
        Task<Entities.ProductShipping> GetByIdAsync(int id);

        //Müşterinin tüm ProductShipping görüntülemek için kullanılabilir
        Task<IEnumerable<Entities.ProductShipping>> GetAllAsync();

        //Belirli koşulları sağlayan ProductShipping kayıtlarını bulmak için kullanılır. 
        Task<IEnumerable<Entities.ProductShipping>> FindAsync(Expression<Func<Entities.ProductShipping, bool>> predicate);

        //Kullanıcılar yeni ProductShipping eklediklerinde
        Task AddAsync(Entities.ProductShipping entity);
        Task AddRangeAsync(IEnumerable<Entities.ProductShipping> entities);

        //ProductShipping sildiklerinde
        Task<bool> RemoveAsync(int id);
        Task RemoveRangeAsync(IEnumerable<Entities.ProductShipping> entities);

        //ProductShipping bilgilerini güncellediklerinde
        Task<bool> UpdateAsync(Entities.ProductShipping entity);

        //Belirli bir ID'ye sahip ProductShipping kaydının veritabanında var olup olmadığını kontrol etmek için kullanılır. 
        Task<bool> ExistsAsync(int id);
    }
}
