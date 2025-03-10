namespace Product.API.DTOS.ProductDTO.Product
{
    public class UpdateProductDTO
    {
        public int ProductId { get; set; }
        public int ProductCategoryId { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
