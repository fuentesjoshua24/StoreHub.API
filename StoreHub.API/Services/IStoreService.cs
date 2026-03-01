using StoreHub.API.Common.Model;

namespace StoreHub.API.Services
{
    public interface IStoreService
    {
        public Task<Response> AddProduct(AddProduct request);
        public Task<Response> DeleteProduct(DeleteProduct request);
        public Task<Response> GetProduct();
        public Task<Response> GetProductById(GetProductById request);
        public Task<Response> UpdateProduct(UpdateProduct request);
    }
}
