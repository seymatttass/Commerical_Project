using Users.API.Data.Entities;
using Users.API.DTOS.Users;

namespace Users.API.Services
{
    public interface IUserService
    {
        Task<IEnumerable<Userss>> GetAllAsync();
        Task<Userss> GetByIdAsync(int id);
        Task<Userss> AddAsync(CreateUserDTO createUserDto);
        Task<bool> UpdateAsync(UpdateUserDTO updateUserDto);
        Task<bool> DeleteAsync(int id);
        Task<Userss?> AuthenticateAsync(string username, string password); // Kullanıcı doğrulama
    }
}
