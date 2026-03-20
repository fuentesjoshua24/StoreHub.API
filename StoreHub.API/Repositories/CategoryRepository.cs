using StoreHub.API.Common.Data;
using StoreHub.API.Common.Model;
using Microsoft.EntityFrameworkCore;

namespace StoreHub.API.Repositories
{
    public interface ICategoryRepository
    {
        Task<CategoryResponse> GetCategory();
        Task<CategoryResponse> GetCategoryById(int id);
    }

    public class CategoryRepository : ICategoryRepository
    {
        private readonly StoreHubDbContext _context;

        public CategoryRepository(StoreHubDbContext context)
        {
            _context = context;
        }
        public async Task<CategoryResponse> GetCategory()
        {
            var response = new CategoryResponse();
            try
            {
                var categories = await _context.Categories.ToListAsync();
                response.Categories = categories;
                response.IsSuccess = true;
                response.Message = categories.Any() ? "Category fetched successfully." : "No category found.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error fetching category: {ex.Message}";
            }
            return response;
        }

        public async Task<CategoryResponse> GetCategoryById(int id)
        {
            var response = new CategoryResponse();
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(p => p.CategoryId == id);

                if (category != null)
                {
                    response.Categories = new List<Category> { category };
                    response.IsSuccess = true;
                    response.Message = "Product fetched successfully.";
                }
                else
                {
                    response.Categories = Enumerable.Empty<Category>();
                    response.IsSuccess = false;
                    response.Message = "Category not found.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error fetching category: {ex.Message}";
            }
            return response;
        }
    }
}
