using Address.API.Data.Entities;
using AutoMapper;

namespace Address.API.Mapping
{
    public class AddressAutoMapperProfile : Profile
    {
        public AddressAutoMapperProfile()
        {

            //dto dönüşüm kodları
            CreateMap<CreateAddressDTO, Addres>()
                .ReverseMap();

            CreateMap<UpdateAddressDTO, Addres>()
                .ReverseMap();
        }
    }
}
