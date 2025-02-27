using AutoMapper;
using Stock.API.DTOS.Stock;

namespace Stock.API.Mapping
{
    public class StockAutoMapperProfile : Profile
    {
        public StockAutoMapperProfile()
        {
            CreateMap<CreateStockDTO, Data.Entities.Stock>();

            CreateMap<UpdateStockDTO, Data.Entities.Stock>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StockId));
        }
    }
}
