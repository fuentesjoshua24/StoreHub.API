using Microsoft.AspNetCore.Identity;
using StoreHub.API.Common.Model;
using StoreHub.API.Repositories;

namespace StoreHub.API.Services
{
    public interface ILoginService
    {
        User? Authenticate(string email, string password);
    }

    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _authRepository;
        private readonly PasswordHasher<User> _passwordHasher;

        public LoginService(ILoginRepository authRepository)
        {
            _authRepository = authRepository;
            _passwordHasher = new PasswordHasher<User>();
        }

        public User? Authenticate(string email, string password)
        {
            var user = _authRepository.FindByEmail(email);
            if (user != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                if (result == PasswordVerificationResult.Success)
                {
                    return user;
                }
            }
            return null;
        }
    }
}
