using InvoiceDetails.DTOS.InvoiceDetails;

namespace InvoiceDetails.services
{
    public interface IInvoiceDetailsService
    {

        Task<IEnumerable<Data.Entities.InvoiceDetails>> GetAllAsync();
        Task<Data.Entities.InvoiceDetails> GetByIdAsync(int id);
        Task<Data.Entities.InvoiceDetails> AddAsync(CreateInvoiceDetailsDTO createInvoiceDetailsDto);
        Task<bool> UpdateAsync(UpdateInvoiceDetailsDTO updateInvoiceDetailsDto);
        Task<bool> DeleteAsync(int id);


    }
}
