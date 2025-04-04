using AutoMapper;
using InvoiceDetails.DTOS.InvoiceDetails;

namespace InvoiceDetails.Mapping
{
    public class InvoiceDetailsAutoMapperProfile : Profile
    {

        public InvoiceDetailsAutoMapperProfile()
        {
            CreateMap<CreateInvoiceDetailsDTO, Data.Entities.InvoiceDetails>()
                .ReverseMap();

            CreateMap<UpdateInvoiceDetailsDTO, Data.Entities.InvoiceDetails>()
                .ReverseMap();
        }
    }
}
