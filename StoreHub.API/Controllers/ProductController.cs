using Microsoft.AspNetCore.Mvc;
using StoreHub.API.Common.Model;
using StoreHub.API.Services;

namespace StoreHub.API.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetProduct()
        {
            ProductResponse response = new ProductResponse();
            _logger.LogInformation("GetProduct API called in Controller...");
            try
            {
                response = await _productService.GetProduct();
                if (!response.IsSuccess)
                {
                    return BadRequest(new
                    {
                        IsSuccess = response.IsSuccess,
                        Message = response.Message,
                        Data = response.Products
                    });
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError($"GetProduct API Error: {ex.Message}");
                return BadRequest(new { isSuccess = response.IsSuccess, Message = response.Message });
            }

            return Ok(new
            {
                IsSuccess = response.IsSuccess,
                Message = response.Message,
                Data = response.Products
            });
        }
    }

}
