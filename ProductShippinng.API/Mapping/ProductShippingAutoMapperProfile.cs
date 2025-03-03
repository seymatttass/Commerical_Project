using AutoMapper;
using ProductShippinng.API.Data.Entities;
using ProductShippinng.API.DTOS.ProductShipping;

namespace ProductShippinng.API.Mapping
{
    public class ProductShippingAutoMapperProfile : Profile
    {
        public ProductShippingAutoMapperProfile()
        {
            //dto dönüşüm kodları
            CreateMap<CreateProductShippingDTO, ProductShipping>()
                .ReverseMap();

            CreateMap<UpdateProductShippingDTO, ProductShipping>()
                .ReverseMap();
        }
    }
}
