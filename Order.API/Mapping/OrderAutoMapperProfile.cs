using AutoMapper;
using Order.API.Data.Entities;
using Order.API.DTOS.OrderItemDTO.OrderItem;
using Order.API.DTOS.OrdersDTO.Orders;

namespace Order.API.Mapping
{
    public class OrderAutoMapperProfile : Profile
    {
        public OrderAutoMapperProfile()
        {
            CreateMap<CreateOrderItemDTO, OrderItemss>().ReverseMap();
            CreateMap<UpdateOrderItemDTO, OrderItemss>().ReverseMap();

            CreateMap<CreateOrdersDTO, Orderss>().ReverseMap();
            CreateMap<UpdateOrdersDTO, Orderss>().ReverseMap();
        }
    }
}
