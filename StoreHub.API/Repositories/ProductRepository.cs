using StoreHub.API.Common.Data;
using StoreHub.API.Common.Model;
using Microsoft.EntityFrameworkCore;


namespace StoreHub.API.Repositories
{
    public interface IProductRepository
    {
        Task<ProductResponse> GetProduct();
        Task<ProductResponse> GetProductById(int id);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly StoreHubDbContext _context;

        public ProductRepository(StoreHubDbContext context)
        {
            _context = context;
        }

        public async Task<ProductResponse> GetProduct()
        {
            var response = new ProductResponse();
            try
            {
                var products = await _context.Products.ToListAsync();
                response.Products = products;
                response.IsSuccess = true;
                response.Message = products.Any() ? "Products fetched successfully." : "No products found.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error fetching products: {ex.Message}";
            }
            return response;
        }

        public async Task<ProductResponse> GetProductById(int id)
        {
            var response = new ProductResponse();
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);

                if (product != null)
                {
                    response.Products = new List<Product> { product };
                    response.IsSuccess = true;
                    response.Message = "Product fetched successfully.";
                }
                else
                {
                    response.Products = Enumerable.Empty<Product>();
                    response.IsSuccess = false;
                    response.Message = "Product not found.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error fetching product: {ex.Message}";
            }
            return response;
        }

    }
}
