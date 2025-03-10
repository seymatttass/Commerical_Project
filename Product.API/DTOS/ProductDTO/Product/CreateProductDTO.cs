namespace Product.API.DTOS.ProductDTO.Product
{
    public class CreateProductDTO
    {
        public int ProductCategoryId { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
