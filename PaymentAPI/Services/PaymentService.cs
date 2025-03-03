using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Payment.API.Data.Entities;
using Payment.API.Data.Repository;
using Payment.API.DTOS.Payments;

namespace Payment.API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IMapper mapper,
            ILogger<PaymentService> logger)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Paymentt> AddAsync(CreatePaymentDTO createPaymentDto)
        {
            try
            {
                var payment = _mapper.Map<Paymentt>(createPaymentDto);
                payment.Date = DateTime.UtcNow;

                await _paymentRepository.AddAsync(payment);
                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating payment");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _paymentRepository.RemoveAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting payment {id}");
                throw;
            }
        }

        public async Task<IEnumerable<Paymentt>> GetAllAsync()
        {
            try
            {
                return await _paymentRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all payments");
                throw;
            }
        }

        public async Task<Paymentt> GetByIdAsync(int id)
        {
            try
            {
                return await _paymentRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting payment {id}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(UpdatePaymentDTO updatePaymentDto)
        {
            try
            {
                var payment = _mapper.Map<Paymentt>(updatePaymentDto);
                payment.Date = DateTime.UtcNow; // Güncelleme zamanını kaydetmek için

                return await _paymentRepository.UpdateAsync(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating payment {updatePaymentDto.Id}");
                throw;
            }
        }

        public async Task<IEnumerable<Paymentt>> GetPaymentsByOrderIdAsync(int orderId)
        {
            try
            {
                return await _paymentRepository.GetPaymentsByOrderIdAsync(orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while retrieving payments for order {orderId}");
                throw;
            }
        }

        public async Task<bool> HasPaymentForBasketAsync(int basketId)
        {
            try
            {
                return await _paymentRepository.HasPaymentForBasketAsync(basketId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while checking payments for basket {basketId}");
                throw;
            }
        }
    }
}
