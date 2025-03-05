using AutoMapper;
using InvoiceDetails.Data.Repository;
using InvoiceDetails.DTOS.InvoiceDetails;

namespace InvoiceDetails.services
{
    public class InvoiceDetailsService : IInvoiceDetailsService
    {

        private readonly IInvoiceDetailsRepository _invoiceDetailsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<InvoiceDetailsService> _logger;
        public InvoiceDetailsService(
            IInvoiceDetailsRepository invoiceDetailsRepository,
            IMapper mapper,
            ILogger<InvoiceDetailsService> logger)
        {
            _invoiceDetailsRepository = invoiceDetailsRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Data.Entities.InvoiceDetails> AddAsync(CreateInvoiceDetailsDTO createInvoiceDetailsDto)
        {
            try
            {
                var invoiceDetails = _mapper.Map<Data.Entities.InvoiceDetails>(createInvoiceDetailsDto);
                await _invoiceDetailsRepository.AddAsync(invoiceDetails);
                return invoiceDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating InvoiceDetails");
                throw;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _invoiceDetailsRepository.RemoveAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting InvoiceDetails {id}");
                throw;
            }
        }
        public async Task<IEnumerable<Data.Entities.InvoiceDetails>> GetAllAsync()
        {
            try
            {
                return await _invoiceDetailsRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all InvoiceDetails");
                throw;
            }
        }
        public async Task<Data.Entities.InvoiceDetails> GetByIdAsync(int id)
        {
            try
            {
                return await _invoiceDetailsRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting InvoiceDetails {id}");
                throw;
            }
        }
        public async Task<bool> UpdateAsync(UpdateInvoiceDetailsDTO updateInvoiceDetailsDto)
        {
            try
            {
                // DTO'dan entity'ye dönüştürme
                var invoiceDetails = _mapper.Map<Data.Entities.InvoiceDetails>(updateInvoiceDetailsDto);

                // Adres güncelleme
                return await _invoiceDetailsRepository.UpdateAsync(invoiceDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating InvoiceDetails {updateInvoiceDetailsDto.InvoiceDetailsId}");
                throw;
            }
        }


    }
}
