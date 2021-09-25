using BFF.Models.Attributes;
using Common.Models.DbModels;

namespace BFF.Models.ViewModels
{
    public class UserView : ViewModel
    {
        [TableColumn(Name = "Id Number")]
        public string IdNumber { get; set; }
        [TableColumn(Name = "Username")]
        public string Username { get; set; }
        [TableColumn(Name = "Email Address")]
        public string EmailAddress { get; set; }
    }
}