namespace Product.API.Data.Entities.ViewModels
{
    public class CategoryCreateModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; } = true;
    }
}
