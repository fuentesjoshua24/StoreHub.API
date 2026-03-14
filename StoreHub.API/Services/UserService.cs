using Microsoft.AspNetCore.Identity;
using StoreHub.API.Common.Model;
using StoreHub.API.Repositories;

namespace StoreHub.API.Services
{
    public interface IUserService
    {
        public Task<UserResponse> GetUser();
        // Add this for registration
        Task<User> Register(string email, string password);
    }

    public class UserService : IUserService
    {
        public readonly IUserRepository _userRepository;
        public readonly ILogger<UserService> _logger;
        public readonly string EmailRegex = @"^[0-9a-zA-Z]+([._+-]?[0-9a-zA-Z]+)*@[0-9a-zA-Z]+.[a-zA-Z]{2,4}([.][a-zA-Z]{2,3})?$";
        public readonly string MobileRegex = @"^(?:\+63|0)9\d{9}$";
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<UserResponse> GetUser()
        {
            _logger.LogInformation("GetProduct Calling in Service");
            return await _userRepository.GetUser();
        }

        // Register
        public async Task<User> Register(string email, string password)
        {
            // validate email format
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, EmailRegex))
            {
                throw new ArgumentException("Invalid email format");
            }

            var user = new User { EmailAddr = email };
            user.PasswordHash = _passwordHasher.HashPassword(user, password);

            await _userRepository.AddUser(user);
            _logger.LogInformation($"New user registered: {email}");

            return user;
        }
    }
}
