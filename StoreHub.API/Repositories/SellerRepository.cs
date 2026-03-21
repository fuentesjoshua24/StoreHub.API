using Microsoft.EntityFrameworkCore;
using StoreHub.API.Common.Data;
using StoreHub.API.Common.Model;
using StoreHub.API.Common.Model;

namespace StoreHub.API.Repositories
{
    public interface ISellerRepository
    {
        Task<SellerResponse> GetSellerById(int id);
        Task<ProductResponse> GetSellerProducts(int sellerId);
    }

    public class SellerRepository : ISellerRepository
    {
        private readonly StoreHubDbContext _context;

        public SellerRepository(StoreHubDbContext context)
        {
            _context = context;
        }

        public async Task<SellerResponse> GetSellerById(int userId)
        {
            var response = new SellerResponse();
            try
            {
                var seller = await _context.Sellers
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.UserId == userId);

                if (seller != null)
                {
                    response.Sellers = new List<Seller> { seller };
                    response.IsSuccess = true;
                    response.Message = "Seller fetched successfully.";
                }
                else
                {
                    response.Sellers = Enumerable.Empty<Seller>();
                    response.IsSuccess = false;
                    response.Message = "Seller not found.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error fetching seller: {ex.Message}";
            }
            return response;
        }

        public async Task<ProductResponse> GetSellerProducts(int sellerId)
        {
            var response = new ProductResponse();
            try
            {
                var products = await _context.SellerMappings
                    .Where(sm => sm.SellerId == sellerId)
                    .Include(sm => sm.Product)
                    .Select(sm => sm.Product)
                    .ToListAsync();

                response.Products = products;
                response.IsSuccess = products.Any();
                response.Message = products.Any()
                    ? "Products fetched successfully."
                    : "No products found for this seller.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error fetching products: {ex.Message}";
            }
            return response;
        }
    }
}
