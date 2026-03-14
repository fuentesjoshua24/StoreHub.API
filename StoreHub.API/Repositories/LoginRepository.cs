using StoreHub.API.Common.Data;
using StoreHub.API.Common.Model;

namespace StoreHub.API.Repositories
{
    public interface ILoginRepository
    {
        User? FindByEmail(string emailAddr);
    }

    public class LoginRepository : ILoginRepository
    {
        private readonly StoreHubDbContext _context;

        public LoginRepository(StoreHubDbContext context)
        {
            _context = context;
        }

        public User? FindByEmail(string emailAddr)
        {
            return _context.Users.FirstOrDefault(u => u.EmailAddr == emailAddr);
        }
    }
}
