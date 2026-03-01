using StoreHub.API.Common.Model;
using StoreHub.API.Repositories;

namespace StoreHub.API.Services
{
    public class StoreService : IStoreService
    {
        public readonly IStoreRepository _storeRepository;
        public readonly ILogger<StoreService> _logger;
        public readonly string EmailRegex = @"^[0-9a-zA-Z]+([._+-]?[0-9a-zA-Z]+)*@[0-9a-zA-Z]+.[a-zA-Z]{2,4}([.][a-zA-Z]{2,3})?$";
        public readonly string MobileRegex = @"^(?:\+63|0)9\d{9}$";


        public StoreService(IStoreRepository storeRepository, ILogger<StoreService> logger)
        {
            _storeRepository = storeRepository;
            _logger = logger;
        }

        public async Task<Response> AddProduct(AddProduct request)
        {
            Response response = new Response();

            //if (String.IsNullOrEmpty(request.UserName))
            //{
            //    response.IsSuccess = false;
            //    response.Message = "UserName can't null or empty.";
            //    return response;
            //}

            //if (String.IsNullOrEmpty(request.EmailAddr))
            //{
            //    response.IsSuccess = false;
            //    response.Message = "EmailAddr can't null or empty.";
            //    return response;
            //}
            //else
            //{
            //    if(!(Regex.IsMatch(request.EmailAddr, EmailRegex)))
            //    {
            //        response.IsSuccess = false;
            //        response.Message = "EmailAddr is not in correct format.";
            //        return response;
            //    }
            //}

            // Validate required product fields
            if (string.IsNullOrWhiteSpace(request.ProductName))
            {
                response.IsSuccess = false;
                response.Message = "ProductName can't be null or empty.";
                return response;
            }

            if (request.Price < 0m)
            {
                response.IsSuccess = false;
                response.Message = "Price can't be negative.";
                return response;
            }

            if (request.Stock < 0)
            {
                response.IsSuccess = false;
                response.Message = "Stock can't be negative.";
                return response;
            }

            _logger.LogInformation("AddProduct Calling in Service");
            return await _storeRepository.AddProduct(request);
        }

        public async Task<Response> GetProduct()
        {
            _logger.LogInformation("GetProduct Calling in Service");
            return await _storeRepository.GetProduct();
        }
        public async Task<Response> GetProductById(GetProductById request)
        {
            _logger.LogInformation("GetProductById Calling in Service");
            return await _storeRepository.GetProductById(request);
        }

        public async Task<Response> UpdateProduct(UpdateProduct request)
        {
            _logger.LogInformation("UpdateProduct Calling in Service");
            return await _storeRepository.UpdateProduct(request);
        }

        public async Task<Response> DeleteProduct(DeleteProduct request)
        {
            _logger.LogInformation("DeleteProduct Calling in Service");
            return await _storeRepository.DeleteProduct(request);
        }
    }
}
