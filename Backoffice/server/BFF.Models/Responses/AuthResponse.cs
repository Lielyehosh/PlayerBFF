namespace BFF.Models
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
}