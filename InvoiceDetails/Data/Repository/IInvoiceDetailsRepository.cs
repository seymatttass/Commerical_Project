using System.Linq.Expressions;

namespace InvoiceDetails.Data.Repository
{
    public interface IInvoiceDetailsRepository
    {
        Task<Entities.InvoiceDetails> GetByIdAsync(int id);

        //Müşterinin tüm fatura detay görüntülemek için kullanılabilir
        Task<IEnumerable<Entities.InvoiceDetails>> GetAllAsync();

        //Belirli koşulları sağlayan fatura detay kayıtlarını bulmak için kullanılır. 
        Task<IEnumerable<Entities.InvoiceDetails>> FindAsync(Expression<Func<Entities.InvoiceDetails, bool>> predicate);

        //Kullanıcılar yeni fatura detay eklediklerinde
        Task AddAsync(Entities.InvoiceDetails entity);
        Task AddRangeAsync(IEnumerable<Entities.InvoiceDetails> entities);

        //fatura detay sildiklerinde
        Task<bool> RemoveAsync(int id);
        Task RemoveRangeAsync(IEnumerable<Entities.InvoiceDetails> entities);

        //fatura detay bilgilerini güncellediklerinde
        Task<bool> UpdateAsync(Entities.InvoiceDetails entity);

        //Belirli bir ID'ye sahip fatura detay kaydının veritabanında var olup olmadığını kontrol etmek için kullanılır. 
        Task<bool> ExistsAsync(int id);
    }
}
