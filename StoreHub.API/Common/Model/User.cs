namespace StoreHub.API.Common.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string EmailAddr { get; set; }   // Only username
        public string PasswordHash { get; set; } // hashed password
        public DateTime CreatedDate { get; set; } // creation date


        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }
    }
}
