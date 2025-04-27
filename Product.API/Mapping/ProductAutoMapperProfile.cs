using AutoMapper;
using Product.API.DTOS.CategoryDTO.Category;
using Product.API.DTOS.ProductCategoryDTO.ProductCategory;
using Product.API.DTOS.ProductDTO.Product;

namespace Product.API.Mapping
{
    public class ProductAutoMapperProfile : Profile
    {

        public ProductAutoMapperProfile()
        {
            CreateMap<CreateProductDTO, Data.Entities.Product>();

            CreateMap<UpdateProductDTO, Data.Entities.Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId));

            CreateMap<CreateCategoryDTO, Data.Entities.Category>();

            CreateMap<UpdateCategoryDTO, Data.Entities.Category>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId));

            CreateMap<CreateProductCategoryDTO, Data.Entities.ProductCategory>();

            CreateMap<UpdateProductCategoryDTO, Data.Entities.ProductCategory>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId));
        }
    }
}
