using AutoMapper;
using Basket.API.Data.Entities;
using Basket.API.DTOS;

namespace Basket.API.Mapping
{
    public class BasketAutoMapperProfile : Profile
    {
        public BasketAutoMapperProfile()
        {
            CreateMap<CreateBasketDTO, Baskett>()
                .ReverseMap();

            CreateMap<UpdateBasketDTO, Baskett>()
                .ReverseMap();
        }
    }
}
