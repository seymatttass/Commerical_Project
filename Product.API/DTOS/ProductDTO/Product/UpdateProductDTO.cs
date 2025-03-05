namespace Product.API.DTOS.ProductDTO.Product
{
    public class UpdateProductDTO
    {
        public int ProductId { get; set; }
        public int ProductCategoryId { get; set; }
        public int Code { get; set; }
        public int Name { get; set; }
        public int Price { get; set; }
    }
}
