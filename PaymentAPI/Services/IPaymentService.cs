using Payment.API.Data.Entities;
using Payment.API.DTOS.Payments;

namespace Payment.API.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<Paymentt>> GetAllAsync();
        Task<Paymentt> GetByIdAsync(int id);
        Task<Paymentt> AddAsync(CreatePaymentDTO createPaymentDto);
        Task<bool> UpdateAsync(UpdatePaymentDTO updatePaymentDto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Paymentt>> GetPaymentsByOrderIdAsync(int orderId);
        Task<bool> HasPaymentForBasketAsync(int basketId);
    }
}
