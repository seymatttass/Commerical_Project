using AutoMapper;
using ProductCategory.DTOS.ProductCategory;

namespace ProductCategory.Mapping
{
    public class ProductCategoryAutoMapperProfile : Profile
    {

        public ProductCategoryAutoMapperProfile()
        {
            CreateMap<CreateProductCategoryDTO, Data.Entities.ProductCategory>();

            CreateMap<UpdateProductCategoryDTO, Data.Entities.ProductCategory>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId));
        }
    }
}
