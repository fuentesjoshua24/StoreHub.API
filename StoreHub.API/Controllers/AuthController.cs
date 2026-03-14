using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreHub.API.Common.Model;
using StoreHub.API.CommonUtility;
using StoreHub.API.Dto;
using StoreHub.API.Repositories;

namespace StoreHub.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtUtil _jwtUtil;

        public AuthController(IUserRepository userRepository, JwtUtil jwtUtil)
        {
            _userRepository = userRepository;
            _jwtUtil = jwtUtil;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var passwordHasher = new PasswordHasher<User>();

            // Step 1: create the user object
            var user = new User
            {
                EmailAddr = dto.Email,              // store full email
                CreatedDate = DateTime.UtcNow
            };

            // Step 2: hash the password using the user object
            user.PasswordHash = passwordHasher.HashPassword(user, dto.Password);

            // Step 3: save to DB
            await _userRepository.AddUser(user);

            return Ok(new { message = "Registration successful" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userRepository.GetByEmail(dto.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result != PasswordVerificationResult.Success)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var token = _jwtUtil.GenerateToken(user.UserId, user.EmailAddr);

            // ✅ Use AuthResponse here
            var response = new AuthResponse
            {
                Token = token,
                Id = user.UserId,
                Email = user.EmailAddr
            };

            return Ok(response);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await _userRepository.GetByEmail(dto.Email);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Generate a reset token (for demo purposes, a GUID)
            var resetToken = Guid.NewGuid().ToString();

            // Save token to DB (add a ResetToken + Expiry field in User model)
            user.ResetToken = resetToken;
            user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);
            await _userRepository.UpdateUser(user);

            // TODO: Send token via email (SMTP, SendGrid, etc.)
            return Ok(new { message = "Password reset token generated", token = resetToken });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await _userRepository.GetByEmail(dto.Email);
            if (user == null || user.ResetToken != dto.Token || user.ResetTokenExpiry < DateTime.UtcNow)
            {
                return BadRequest(new { message = "Invalid or expired token" });
            }

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, dto.NewPassword);

            // Clear token after use
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            await _userRepository.UpdateUser(user);

            return Ok(new { message = "Password reset successful" });
        }


    }
    
}
