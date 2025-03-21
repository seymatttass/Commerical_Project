using AutoMapper;
using Category.API.DTOS.Category;

namespace Category.API.Mapping
{
    public class CategoryAutoMapperProfile : Profile
    {
        public CategoryAutoMapperProfile()
        {
            CreateMap<CreateCategoryDTO, Data.Entities.Category>();

            CreateMap<UpdateCategoryDTO, Data.Entities.Category>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId));
        }
    }
}

