using System.ComponentModel.DataAnnotations;

namespace PlayerBFF.Controllers
{
    internal class UserModel
    {
        public string Username { get; set; }
        public string PasswordHashed { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}