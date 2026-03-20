using StoreHub.API.Common.Data;
using StoreHub.API.Common.Model;
using Microsoft.EntityFrameworkCore;


namespace StoreHub.API.Repositories
{
    public interface IPromotionRepository
    {
        Task<PromotionResponse> GetPromotion();
    }
    public class PromotionRepository : IPromotionRepository
    {
        private readonly StoreHubDbContext _context;

        public PromotionRepository(StoreHubDbContext context)
        {
            _context = context;
        }

        public async Task<PromotionResponse> GetPromotion()
        {
            var response = new PromotionResponse();
            try
            {
                var promotions = await _context.Promotions.ToListAsync();
                response.Promotions = promotions;
                response.IsSuccess = true;
                response.Message = promotions.Any() ? "Promotions fetched successfully." : "No promotions found.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error fetching promotions: {ex.Message}";
            }
            return response;
        }
    }
}
