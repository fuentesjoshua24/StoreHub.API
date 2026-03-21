using Microsoft.AspNetCore.Identity;
using StoreHub.API.Common.Model;
using StoreHub.API.Repositories;

namespace StoreHub.API.Services
{
    public interface ISellerService
    {
        Task<SellerResponse> GetSellerById(int id);
        Task<ProductResponse> GetSellerProducts(int sellerId);
    }

    public class SellerService : ISellerService
    {
        public readonly ISellerRepository _sellerRepository;
        public readonly ILogger<SellerService> _logger;

        public SellerService(ISellerRepository sellerRepository, ILogger<SellerService> logger)
        {
            _sellerRepository = sellerRepository;
            _logger = logger;
        }

        public async Task<SellerResponse> GetSellerById(int id)
        {
            // You can add extra business logic here if needed
            return await _sellerRepository.GetSellerById(id);
        }

        public async Task<ProductResponse> GetSellerProducts(int sellerId)
        {
            if (sellerId <= 0)
            {
                throw new ArgumentException("Invalid seller ID.");
            }

            return await _sellerRepository.GetSellerProducts(sellerId);
        }
    }
}
