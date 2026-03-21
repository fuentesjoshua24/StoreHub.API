using StoreHub.API.Common.Data;
using StoreHub.API.Common.Model;
using Microsoft.EntityFrameworkCore;


namespace StoreHub.API.Repositories
{
    public interface IUserRepository
    {
        public Task<UserResponse> GetUser();

        // Register
        Task AddUser(User user);
        // Login
        Task<User?> GetByEmail(string emailAddr); // ✅ new method

        // Update
        Task UpdateUser(User user);



    }

    public class UserRepository : IUserRepository
    {
        private readonly StoreHubDbContext _context;

        public UserRepository(StoreHubDbContext context)
        {
            _context = context;
        }

        public async Task<UserResponse> GetUser()
        {
            var response = new UserResponse();
            try
            {
                var users = await _context.Users.ToListAsync();
                response.Users = users;
                response.IsSuccess = true;
                response.Message = users.Any() ? "Users fetched successfully." : "No users found.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error fetching users: {ex.Message}";
            }
            return response;
        }

        // Register
        public async Task AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        // Login
        public async Task<User?> GetByEmail(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.EmailAddr == userName);
        }

        // Update
        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        


    }
}
