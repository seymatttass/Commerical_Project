using System.Linq.Expressions;
using System.Threading.Tasks;
using Invoice.API.Data.Entities;


namespace Invoice.API.Data.Repository
{
    public interface IInvoiceRepository
    {

        Task<Entities.Invoice> GetByIdAsync(int id);

        //Müşterinin tüm faturalarını görüntülemek için kullanılabilir
        Task<IEnumerable<Entities.Invoice>> GetAllAsync();

        //Belirli koşulları sağlayan fatura kayıtlarını bulmak için kullanılır. 
        Task<IEnumerable<Entities.Invoice>> FindAsync(Expression<Func<Entities.Invoice, bool>> predicate);

        //Kullanıcılar yeni fatura eklediklerinde
        Task AddAsync(Entities.Invoice entity);
        Task AddRangeAsync(IEnumerable<Entities.Invoice> entities);

        //fatura sildiklerinde
        Task<bool> RemoveAsync(int id);
        Task RemoveRangeAsync(IEnumerable<Entities.Invoice> entities);

        //fatura bilgilerini güncellediklerinde
        Task<bool> UpdateAsync(Entities.Invoice entity);

        //Belirli bir ID'ye sahip fatura kaydının veritabanında var olup olmadığını kontrol etmek için kullanılır. 
        Task<bool> ExistsAsync(int id);

    }
}
