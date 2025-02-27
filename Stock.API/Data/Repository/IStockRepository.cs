using System.Linq.Expressions;

namespace Stock.API.Data.Repository
{
    public interface IStockRepository
    {

        Task<Entities.Stock> GetByIdAsync(int id);

        //Müşterinin tüm adreslerini görüntülemek için kullanılabilir
        Task<IEnumerable<Entities.Stock>> GetAllAsync();

        //Belirli koşulları sağlayan Addres kayıtlarını bulmak için kullanılır. 
        Task<IEnumerable<Entities.Stock>> FindAsync(Expression<Func<Entities.Stock, bool>> predicate);

        //Kullanıcılar yeni adres eklediklerinde
        Task AddAsync(Entities.Stock entity);
        Task AddRangeAsync(IEnumerable<Entities.Stock> entities);

        //Adresleri sildiklerinde
        Task<bool> RemoveAsync(int id);
        Task RemoveRangeAsync(IEnumerable<Entities.Stock> entities);

        //Adres bilgilerini güncellediklerinde
        Task<bool> UpdateAsync(Entities.Stock entity);

        //Belirli bir ID'ye sahip Addres kaydının veritabanında var olup olmadığını kontrol etmek için kullanılır. 
        Task<bool> ExistsAsync(int id);
    }
}
