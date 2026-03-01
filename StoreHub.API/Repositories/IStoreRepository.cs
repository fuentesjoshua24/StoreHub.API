using StoreHub.API.Common.Model;

namespace StoreHub.API.Repositories
{
    public interface IStoreRepository
    {
        public Task<Response> AddProduct(AddProduct request);
        public Task<Response> DeleteProduct(DeleteProduct request);
        public Task<Response> GetProduct();
        public Task<Response> UpdateProduct(UpdateProduct request);
    }
}
