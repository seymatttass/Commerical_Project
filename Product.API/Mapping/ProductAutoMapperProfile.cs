using AutoMapper;
using Product.API.DTOS.Product;

namespace Product.API.Mapping
{
    public class ProductAutoMapperProfile : Profile
    {
        public ProductAutoMapperProfile()
        {
            CreateMap<CreateProductDTO, Data.Entities.Product>();

            CreateMap<UpdateProductDTO, Data.Entities.Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId));
        }
    }
}
