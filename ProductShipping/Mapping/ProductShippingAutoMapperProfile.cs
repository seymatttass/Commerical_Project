using AutoMapper;
using ProductShipping.DTOS.ProductShipping;

namespace ProductShipping.Mapping
{
    public class ProductShippingAutoMapperProfile : Profile
    {

        public ProductShippingAutoMapperProfile()
        {
            CreateMap<CreateProductShippingDTO, Data.Entities.ProductShipping>();

            CreateMap<UpdateProductShippingDTO, Data.Entities.ProductShipping>();
               // .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));
        }

    }
}
