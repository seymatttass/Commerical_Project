using AutoMapper;
using Users.API.Data.Entities;
using Users.API.DTOS.Users;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Users.API.Mapping
{
    public class UserAutoMapperProfile : Profile
    {
        public UserAutoMapperProfile()
        {
            CreateMap<CreateUserDTO, Userss>()
                .ReverseMap();

            CreateMap<UpdateUserDTO, Userss>()
                .ReverseMap();
        }
    }
}
