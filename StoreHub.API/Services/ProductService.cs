using StoreHub.API.Common.Model;
using StoreHub.API.Repositories;

namespace StoreHub.API.Services
{
    public interface IProductService
    {
        Task<ProductResponse> GetProduct();
        Task<ProductResponse> GetProducts();
        Task<ProductResponse> GetProductById(int id);
        Task<ProductWithSaleResponse> GetProductsOnSale();

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

        public async Task<ProductResponse> GetProducts()
        {
            //_logger.LogInformation("GetProducts called in Service");
            return await _productRepository.GetProducts();
        }

        public async Task<ProductResponse> GetProductById(int id)
        {
            // You can add extra business logic here if needed
            return await _productRepository.GetProductById(id);
        }

        public async Task<ProductWithSaleResponse> GetProductsOnSale()
        {
            return await _productRepository.GetProductsOnSale();
        }

    }
}
