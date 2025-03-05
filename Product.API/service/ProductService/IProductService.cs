using Product.API.DTOS.ProductDTO.Product;

namespace Product.API.service.ProductService
{
    public interface IProductService
    {
        Task<IEnumerable<Data.Entities.Product>> GetAllAsync();
        Task<Data.Entities.Product> GetByIdAsync(int id);
        Task<Data.Entities.Product> AddAsync(CreateProductDTO createProductDto);
        Task<bool> UpdateAsync(UpdateProductDTO updateProductDto);
        Task<bool> DeleteAsync(int id);
    }
}
