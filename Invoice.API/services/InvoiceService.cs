using AutoMapper;
using Invoice.API.Data.Repository;
using Invoice.API.DTOS.Invoice;

namespace Invoice.API.services
{
    public class InvoiceService : IInvoiceService
    {


        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<InvoiceService> _logger;
        public InvoiceService(
            IInvoiceRepository invoiceRepository,
            IMapper mapper,
            ILogger<InvoiceService> logger)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Data.Entities.Invoice> AddAsync(CreateInvoiceDTO createInvoiceDto)
        {
            try
            {
                var invoice = _mapper.Map<Data.Entities.Invoice>(createInvoiceDto);
                await _invoiceRepository.AddAsync(invoice);
                return invoice;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating invoice");
                throw;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _invoiceRepository.RemoveAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting invoice {id}");
                throw;
            }
        }
        public async Task<IEnumerable<Data.Entities.Invoice>> GetAllAsync()
        {
            try
            {
                return await _invoiceRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all invoice");
                throw;
            }
        }
        public async Task<Data.Entities.Invoice> GetByIdAsync(int id)
        {
            try
            {
                return await _invoiceRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting invoice {id}");
                throw;
            }
        }
        public async Task<bool> UpdateAsync(UpdateInvoiceDTO updateInvoiceDto)
        {
            try
            {
                // DTO'dan entity'ye dönüştürme
                var invoice = _mapper.Map<Data.Entities.Invoice>(updateInvoiceDto);

                // Adres güncelleme
                return await _invoiceRepository.UpdateAsync(invoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating address {updateInvoiceDto.InvoiceId}");
                throw;
            }
        }
    }
}
