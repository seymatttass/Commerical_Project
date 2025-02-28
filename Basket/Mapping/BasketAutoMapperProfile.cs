using AutoMapper;
using Basket.API.Data.Entities;
using Basket.API.DTOS;
using Basket.API.DTOS.BasketDTO.Basket;

namespace Basket.API.Mapping
{
    public class BasketAutoMapperProfile : Profile
    {
        public BasketAutoMapperProfile()
        {
            CreateMap<CreateBasketDTO, Baskett>().ReverseMap();
            CreateMap<UpdateBasketDTO, Baskett>().ReverseMap();

            CreateMap<CreateBasketItemDTO, BasketItem>().ReverseMap();
            CreateMap<UpdateBasketItemDTO, BasketItem>().ReverseMap();
        }
    }
}
