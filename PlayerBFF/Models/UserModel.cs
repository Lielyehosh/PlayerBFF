using System.ComponentModel.DataAnnotations;

namespace PlayerBFF.Models
{
    public class UserModel
    {
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}