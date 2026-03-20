using StoreHub.API.Common.Model;
using StoreHub.API.Repositories;

namespace StoreHub.API.Services
{
    public interface IPromotionService
    {
        Task<PromotionResponse> GetPromotion();
    }
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;

        public PromotionService(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }

        public async Task<PromotionResponse> GetPromotion()
        {
            // You can add extra business logic here if needed
            return await _promotionRepository.GetPromotion();
        }
    }
}
