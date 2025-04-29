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
            CreateMap<CreateProductDTO, Data.Entities.Product>()
                .ReverseMap();

            CreateMap<UpdateProductDTO, Data.Entities.Product>()
                .ReverseMap();

            CreateMap<CreateCategoryDTO, Data.Entities.Category>()
                .ReverseMap();

            CreateMap<UpdateCategoryDTO, Data.Entities.Category>()
                .ReverseMap();

            CreateMap<CreateProductCategoryDTO, Data.Entities.ProductCategory>()
                .ReverseMap();

            CreateMap<UpdateProductCategoryDTO, Data.Entities.ProductCategory>()
                .ReverseMap();
        }


    }
}
