namespace Product.API.DTOS.Product
{
    public class CreateProductDTO
    {
        public int ProductCategoryId { get; set; }
        public int Code { get; set; }
        public int Name { get; set; }
        public int Price { get; set; }
    }
}
