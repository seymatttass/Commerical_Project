using System.Linq.Expressions;

namespace Product.API.Data.Repository
{
    public interface IProductRepository
    {
        Task<Entities.Product> GetByIdAsync(int id);

        //Müşterinin tüm ürün görüntülemek için kullanılabilir
        Task<IEnumerable<Entities.Product>> GetAllAsync();

        //Belirli koşulları sağlayan ürün kayıtlarını bulmak için kullanılır. 
        Task<IEnumerable<Entities.Product>> FindAsync(Expression<Func<Entities.Product, bool>> predicate);

        //Kullanıcılar yeni ürün eklediklerinde
        Task AddAsync(Entities.Product entity);
        Task AddRangeAsync(IEnumerable<Entities.Product> entities);

        //ürün sildiklerinde
        Task<bool> RemoveAsync(int id);
        Task RemoveRangeAsync(IEnumerable<Entities.Product> entities);

        //ürün bilgilerini güncellediklerinde
        Task<bool> UpdateAsync(Entities.Product entity);

        //Belirli bir ID'ye sahip ürün kaydının veritabanında var olup olmadığını kontrol etmek için kullanılır. 
        Task<bool> ExistsAsync(int id);
    }
}
