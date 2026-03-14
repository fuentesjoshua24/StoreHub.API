using StoreHub.API.Common.Model;
using StoreHub.API.Repositories;

namespace StoreHub.API.Services
{
    public interface IProductService
    {
        Task<ProductResponse> GetProduct();
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductResponse> GetProduct()
        {
            // You can add extra business logic here if needed
            return await _productRepository.GetProduct();
        }
    }
}
