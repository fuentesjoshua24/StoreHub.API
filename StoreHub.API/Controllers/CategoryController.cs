using Microsoft.AspNetCore.Mvc;
using StoreHub.API.Common.Model;
using StoreHub.API.Services;

namespace StoreHub.API.Controllers
{
    [ApiController]
    //[Route("api/product")]
    [Route("api/[controller]/[Action]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetCategory()
        {
            CategoryResponse response = new CategoryResponse();
            _logger.LogInformation("GetProduct API called in Controller...");
            try
            {
                response = await _categoryService.GetCategory();
                if (!response.IsSuccess)
                {
                    return BadRequest(new
                    {
                        IsSuccess = response.IsSuccess,
                        Message = response.Message,
                        Data = response.Categories
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
                Data = response.Categories
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCategoryById(int id)
        {
            _logger.LogInformation("GetCategoryById API called in Controller...");

            CategoryResponse response = new CategoryResponse();
            try
            {
                response = await _categoryService.GetCategoryById(id);

                if (!response.IsSuccess)
                {
                    return NotFound(new
                    {
                        IsSuccess = response.IsSuccess,
                        Message = response.Message,
                        Data = response.Categories
                    });
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError($"GetCategoryById API Error: {ex.Message}");
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
                Data = response.Categories
            });
        }

    }
}
