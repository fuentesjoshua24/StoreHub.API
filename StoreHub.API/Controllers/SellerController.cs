using Microsoft.AspNetCore.Mvc;
using StoreHub.API.Common.Model;
using StoreHub.API.Services;

namespace StoreHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class SellerController : ControllerBase
    {
        private readonly ISellerService _sellerService;
        private readonly ILogger<SellerController> _logger;

        public SellerController(ISellerService sellerService, ILogger<SellerController> logger)
        {
            _sellerService = sellerService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetSellerById(int id)
        {
            _logger.LogInformation("GetSellerById API called in Controller...");

            SellerResponse response = new SellerResponse();
            try
            {
                response = await _sellerService.GetSellerById(id);

                if (!response.IsSuccess)
                {
                    return NotFound(new
                    {
                        IsSuccess = response.IsSuccess,
                        Message = response.Message,
                        Data = response.Sellers
                    });
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError($"GetSellerById API Error: {ex.Message}");
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
                Data = response.Sellers
            });
        }

        [HttpGet("{sellerId}")]
        public async Task<ActionResult> GetSellerProducts(int sellerId)
        {
            _logger.LogInformation($"GetSellerProducts API called for SellerId: {sellerId}");

            ProductResponse response = new ProductResponse();
            try
            {
                response = await _sellerService.GetSellerProducts(sellerId);

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
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Validation failed: {ex.Message}");
                return BadRequest(new { IsSuccess = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError($"GetSellerProducts API Error: {ex.Message}");
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
