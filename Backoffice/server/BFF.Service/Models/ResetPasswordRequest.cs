namespace BFF.Service.Controllers
{
    public class ResetPasswordRequest
    {
        public string ConfirmPassword { get; set; }
        public string Password { get; set; }
    }
}