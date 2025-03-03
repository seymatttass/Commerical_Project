using ProductShippinng.API.Data.Entities;
using ProductShippinng.API.DTOS.ProductShipping;

namespace ProductShippinng.API.services
{
    public interface IProductShippingService
    {

        Task<IEnumerable<ProductShipping>> GetAllAsync();
        Task<ProductShipping> GetByIdAsync(int id);
        Task<ProductShipping> AddAsync(CreateProductShippingDTO createProductShippingDto);
        Task<bool> UpdateAsync(UpdateProductShippingDTO updateProductShippingDto);
        Task<bool> DeleteAsync(int id);
    }
}
