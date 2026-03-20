using StoreHub.API.Common.Model;
using StoreHub.API.Repositories;

namespace StoreHub.API.Services
{
    public interface ICategoryService
    {
        Task<CategoryResponse> GetCategory();
        Task<CategoryResponse> GetCategoryById(int id);
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<CategoryResponse> GetCategory()
        {
            // You can add extra business logic here if needed
            return await _categoryRepository.GetCategory();
        }

        public async Task<CategoryResponse> GetCategoryById(int id)
        {
            // You can add extra business logic here if needed
            return await _categoryRepository.GetCategoryById(id);
        }
    }
}
