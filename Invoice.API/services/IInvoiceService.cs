using Invoice.API.DTOS.Invoice;

namespace Invoice.API.services
{
    public interface IInvoiceService
    {
        Task<IEnumerable<Data.Entities.Invoice>> GetAllAsync();
        Task<Data.Entities.Invoice> GetByIdAsync(int id);
        Task<Data.Entities.Invoice> AddAsync(CreateInvoiceDTO createInvoiceDto);
        Task<bool> UpdateAsync(UpdateInvoiceDTO updateInvoiceDto);
        Task<bool> DeleteAsync(int id);
    }
}
