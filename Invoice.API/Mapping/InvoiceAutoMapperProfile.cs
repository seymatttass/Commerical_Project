using AutoMapper;
using Invoice.API.DTOS.Invoice;

namespace Invoice.API.Mapping
{
    public class InvoiceAutoMapperProfile : Profile
    {

        public InvoiceAutoMapperProfile()
        {
                        //dto dönüşüm kodları
            CreateMap<CreateInvoiceDTO, Data.Entities.Invoice>()
                .ReverseMap();

            CreateMap<UpdateInvoiceDTO, Data.Entities.Invoice>()
                .ReverseMap();
        }
    }
}
