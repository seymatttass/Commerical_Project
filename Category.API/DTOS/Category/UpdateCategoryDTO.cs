namespace Category.API.DTOS.Category
{
    public class UpdateCategoryDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}
