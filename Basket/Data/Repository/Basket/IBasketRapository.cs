using System.Linq.Expressions;
using Basket.API.Data.Entities;
namespace Basket.API.Data.Repository.Basket
{
    public interface IBasketRepository
    {
        Task<Baskett> GetByIdAsync(int id); //ID’ye göre sepet getir
        Task<IEnumerable<Baskett>> GetAllAsync();//Tüm sepetleri getir
        Task<IEnumerable<Baskett>> FindAsync(Expression<Func<Baskett, bool>> predicate);//Şarta göre filtreli getir
        Task AddAsync(Baskett entity);//Yeni sepet ekle
        Task<bool> RemoveAsync(int id);//Sepeti sil
        Task RemoveRangeAsync(IEnumerable<Baskett> entities); //Birden fazla sepeti sil
        Task<bool> UpdateAsync(Baskett entity);//Sepeti güncelle
        Task<bool> ExistsAsync(int id);//Sepet var mı kontrol et
        Task<Baskett> GetByUserIdAsync(int userId);//Kullanıcıya ait sepeti getir
    }
}