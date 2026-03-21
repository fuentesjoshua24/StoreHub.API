using Microsoft.AspNetCore.Mvc;
using StoreHub.API.Common.Model;
using StoreHub.API.Services;

namespace StoreHub.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserService _userService;
        public readonly ILogger<UserController> _logger;


        public UserController(IUserService storeService, ILogger<UserController> logger)
        {
            _userService = storeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetUser()
        {
            UserResponse response = new UserResponse();
            _logger.LogInformation($"GetUser API Calling in Controller...");
            try
            {
                response = await _userService.GetUser();
                if (!response.IsSuccess)
                {
                    return BadRequest(new
                    {
                        IsSuccess = response.IsSuccess,
                        Message = response.Message,
                        Data = response.Users
                    });
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError($"GetUser API Error Occur : Message {ex.Message}");
                return BadRequest(new { isSuccess = response.IsSuccess, Message = response.Message });
            }

            return Ok(new
            {
                IsSuccess = response.IsSuccess,
                Message = response.Message,
                Data = response.Users
            });
        }



    }
}
