namespace BFF.Models
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string ConfirmPassword { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public bool Terms { get; set; }
    }
}