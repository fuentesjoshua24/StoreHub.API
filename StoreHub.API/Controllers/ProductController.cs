using Microsoft.AspNetCore.Mvc;
using StoreHub.API.Common.Model;
using StoreHub.API.Services;

namespace StoreHub.API.Controllers
{
    [ApiController]
    //[Route("api/product")]
    [Route("api/[controller]/[Action]")]
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

        [HttpGet]
        public async Task<ActionResult> GetProducts()
        {
            _logger.LogInformation("GetProducts API called in Controller...");

            ProductResponse response = new ProductResponse();
            try
            {
                response = await _productService.GetProducts();

                if (!response.IsSuccess)
                {
                    return NotFound(new
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
                _logger.LogError($"GetProducts API Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    IsSuccess = response.IsSuccess,
                    Message = response.Message
                });
            }

            return Ok(new
            {
                IsSuccess = response.IsSuccess,
                Message = response.Message,
                Data = response.Products
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProductById(int id)
        {
            _logger.LogInformation("GetProductById API called in Controller...");

            ProductResponse response = new ProductResponse();
            try
            {
                response = await _productService.GetProductById(id);

                if (!response.IsSuccess)
                {
                    return NotFound(new
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
                _logger.LogError($"GetProductById API Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    IsSuccess = response.IsSuccess,
                    Message = response.Message
                });
            }

            return Ok(new
            {
                IsSuccess = response.IsSuccess,
                Message = response.Message,
                Data = response.Products
            });
        }

        [HttpGet]
        public async Task<ActionResult> GetProductsOnSale()
        {
            _logger.LogInformation("GetProductsOnSale API called in Controller...");
            ProductWithSaleResponse response = new ProductWithSaleResponse();

            try
            {
                response = await _productService.GetProductsOnSale();
                if (!response.IsSuccess)
                {
                    return NotFound(new
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
                _logger.LogError($"GetProductsOnSale API Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    IsSuccess = response.IsSuccess,
                    Message = response.Message
                });
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
