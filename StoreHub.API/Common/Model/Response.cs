namespace StoreHub.API.Common.Model
{
    public class UserResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; internal set; }
        public List<User> Users { get; set; }
    }
}
