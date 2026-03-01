using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoreHub.API.Common.Model;
using StoreHub.API.Services;
using System.Text.Json.Serialization;

namespace StoreHub.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        public readonly IStoreService _storeService;
        public readonly ILogger<StoreController> _logger;


        public StoreController(IStoreService storeService, ILogger<StoreController> logger)
        {
            _storeService = storeService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct(AddProduct request)
        {
            Response response = new Response();
            _logger.LogInformation($"AddProduct API Calling in Controller...{JsonConvert.SerializeObject(request)}");
            try
            {
                response = await _storeService.AddProduct(request);
                if (!response.IsSuccess)
                {
                    return BadRequest(new
                    {
                        IsSuccess = response.IsSuccess,
                        Message = response.Message
                    });
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError($"AddProduct API Error Occur : Message {ex.Message}");
                return BadRequest(new { isSuccess = response.IsSuccess, Message = response.Message });
            }

            return Ok(new
            {
                IsSuccess = response.IsSuccess,
                Message = response.Message
            });
        }

        [HttpGet]
        public async Task<ActionResult> GetProduct()
        {
            Response response = new Response();
            _logger.LogInformation($"GetProduct API Calling in Controller...");
            try
            {
                response = await _storeService.GetProduct();
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
                _logger.LogError($"GetProduct API Error Occur : Message {ex.Message}");
                return BadRequest(new { isSuccess = response.IsSuccess, Message = response.Message });
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
            Response response = new Response();
            _logger.LogInformation($"GetProductById API Calling in Controller...");
            try
            {
                response = await _storeService.GetProductById(new GetProductById { ProductId = id });
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
                _logger.LogError($"GetProductById API Error Occur : Message {ex.Message}");
                return BadRequest(new { isSuccess = response.IsSuccess, Message = response.Message });
            }

            return Ok(new
            {
                IsSuccess = response.IsSuccess,
                Message = response.Message,
                Data = response.Products
            });
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct(UpdateProduct request)
        {
            Response response = new Response();
            _logger.LogInformation($"UpdateProduct API Calling in Controller...{JsonConvert.SerializeObject(request)}");
            try
            {
                response = await _storeService.UpdateProduct(request);
                if (!response.IsSuccess)
                {
                    return BadRequest(new
                    {
                        IsSuccess = response.IsSuccess,
                        Message = response.Message
                    });
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError($"AddProduct API Error Occur : Message {ex.Message}");
                return BadRequest(new { isSuccess = response.IsSuccess, Message = response.Message });
            }

            return Ok(new
            {
                IsSuccess = response.IsSuccess,
                Message = response.Message
            });
        }
        [HttpPost]
        public async Task<ActionResult> DeleteProduct(DeleteProduct request)
        {
            Response response = new Response();
            _logger.LogInformation($"DeleteProduct API Calling in Controller...{JsonConvert.SerializeObject(request)}");
            try
            {
                response = await _storeService.DeleteProduct(request);
                if (!response.IsSuccess)
                {
                    return BadRequest(new
                    {
                        IsSuccess = response.IsSuccess,
                        Message = response.Message
                    });
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError($"DeleteProduct API Error Occur : Message {ex.Message}");
                return BadRequest(new { isSuccess = response.IsSuccess, Message = response.Message });
            }

            return Ok(new
            {
                IsSuccess = response.IsSuccess,
                Message = response.Message
            });
        }
    }
}
