using AutoMapper;
using Shipping.API.DTOS.Shipping;

namespace Shipping.API.Mapping
{
    public class ShippingAutoMapperProfile : Profile
    {
        public ShippingAutoMapperProfile()
        {
            CreateMap<CreateShippingDTO, Data.Entities.Shipping>();

            CreateMap<UpdateShippingDTO, Data.Entities.Shipping>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ShippingId));
        }
    }
}
