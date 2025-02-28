using AutoMapper;
using Payment.API.Data.Entities;
using Payment.API.DTOS.Payments;

namespace Payment.API.Mapping
{
    public class PaymentAutoMapperProfile : Profile
    {
        public PaymentAutoMapperProfile()
        {
            CreateMap<CreatePaymentDTO, Paymentt>()
                .ReverseMap();

            CreateMap<UpdatePaymentDTO, Paymentt>()
                .ReverseMap();
        }
    }
}
