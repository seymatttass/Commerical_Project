using ProductShipping.DTOS.ProductShipping;

namespace ProductShipping.service
{
    public interface IProductShippingService
    {
        Task<IEnumerable<Data.Entities.ProductShipping>> GetAllAsync();
        Task<Data.Entities.ProductShipping> GetByIdAsync(int id);
        Task<Data.Entities.ProductShipping> AddAsync(CreateProductShippingDTO createproductShippingDto);
        Task<bool> UpdateAsync(UpdateProductShippingDTO updateproductShippingDto);
        Task<bool> DeleteAsync(int id);
    }
}
