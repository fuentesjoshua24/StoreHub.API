namespace StoreHub.API.Common.Model
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class CategoryResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; internal set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
