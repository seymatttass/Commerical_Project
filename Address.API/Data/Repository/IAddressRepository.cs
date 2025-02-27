using Address.API.Data.Entities;
using System.Linq.Expressions;

namespace Address.API.Data.Repository
{
    public interface IAddressRepository
    {
        Task<Addres> GetByIdAsync(int id);

        //Müşterinin tüm adreslerini görüntülemek için kullanılabilir
        Task<IEnumerable<Addres>> GetAllAsync();

        //Belirli koşulları sağlayan Addres kayıtlarını bulmak için kullanılır. 
        Task<IEnumerable<Addres>> FindAsync(Expression<Func<Addres, bool>> predicate);

        //Kullanıcılar yeni adres eklediklerinde
        Task AddAsync(Addres entity);
        Task AddRangeAsync(IEnumerable<Addres> entities);

        //Adresleri sildiklerinde
        Task<bool> RemoveAsync(int id);
        Task RemoveRangeAsync(IEnumerable<Addres> entities);

        //Adres bilgilerini güncellediklerinde
        Task<bool> UpdateAsync(Addres entity);

        //Belirli bir ID'ye sahip Addres kaydının veritabanında var olup olmadığını kontrol etmek için kullanılır. 
        Task<bool> ExistsAsync(int id);
    }
}
