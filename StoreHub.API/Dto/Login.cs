namespace StoreHub.API.Dto
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class AuthResponse
    {
        public string Token { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
    }

    public class RegisterDto
    {
        public string Email { get; set; }   // Username instead of email
        public string Password { get; set; }
    }

    public class ForgotPasswordDto
    {
        public string Email { get; set; }
    }

    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
