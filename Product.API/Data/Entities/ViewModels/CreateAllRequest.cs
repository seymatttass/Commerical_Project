namespace Product.API.Data.Entities.ViewModels
{
    public class CreateAllRequest
    {
        public ProductCreateModel Product { get; set; }
        public CategoryCreateModel Category { get; set; }
    }
}
