using StoreHub.API.Common.Data;
using StoreHub.API.Common.Model;
using Microsoft.EntityFrameworkCore;


namespace StoreHub.API.Repositories
{
    public interface IProductRepository
    {
        Task<ProductResponse> GetProduct();
        Task<ProductResponse> GetProducts();
        Task<ProductResponse> GetProductById(int id);
        Task<ProductWithSaleResponse> GetProductsOnSale();
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

        public async Task<ProductResponse> GetProducts()
        {
            var response = new ProductResponse();
            try
            {
                var products = await _context.SellerMappings
                    .Include(sm => sm.Product)
                    .Select(sm => sm.Product)
                    .Distinct() // avoid duplicates if product is mapped to multiple sellers
                    .ToListAsync();

                response.Products = products;
                response.IsSuccess = products.Any();
                response.Message = products.Any()
                    ? "Products fetched successfully."
                    : "No products found in seller mappings.";
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
                var product = await _context.Products
                    .Include(p => p.SellerMappings)   // include mappings only
                    .FirstOrDefaultAsync(p => p.ProductId == id);

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

        public async Task<ProductWithSaleResponse> GetProductsOnSale()
        {
            var response = new ProductWithSaleResponse();
            try
            {
                //var productsOnSale = await (
                //    from p in _context.Products
                //    join s in _context.Sales on p.ProductId equals s.ProductId
                //    where s.OnSale == true || s.Discount > 0
                //    select new Product
                //    {
                //        ProductId = p.ProductId,
                //        ProductName = p.ProductName,
                //        Brand = p.Brand,
                //        Price = p.Price,
                //        SalePrice = s.SalePrice,
                //        Discount = s.Discount,
                //        Description = p.Description,
                //        SKU = p.SKU,
                //        SaleStock = s.SaleStock,
                //        CategoryId = p.CategoryId,
                //        ImageUrl = p.ImageUrl,
                //        IsActive = p.IsActive,
                //        CreatedDate = s.CreatedDate,
                //        UpdatedDate = s.UpdatedDate
                //    }).ToListAsync();

                var productsOnSale = await (
                   from p in _context.Products
                   join s in _context.Sales on p.ProductId equals s.ProductId
                   where s.OnSale == true || s.Discount > 0
                   select new ProductWithSale
                   {
                       ProductId = p.ProductId,
                       ProductName = p.ProductName,
                       Brand = p.Brand,
                       Price = p.Price,
                       SalePrice = s.SalePrice,
                       Discount = s.Discount,
                       OnSale = s.OnSale,
                       Description = p.Description,
                       SKU = p.SKU,
                       SaleStock = s.SaleStock,
                       CategoryId = p.CategoryId,
                       ImageUrl = p.ImageUrl,
                       IsActive = p.IsActive,
                       CreatedDate = s.CreatedDate,
                       UpdatedDate = s.UpdatedDate
                   }).ToListAsync();

                response.Products = productsOnSale;
                response.IsSuccess = true;
                response.Message = productsOnSale.Any() ? "Products on sale fetched successfully." : "No products on sale.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error fetching products on sale: {ex.Message}";
            }
            return response;
        }

    }
}
