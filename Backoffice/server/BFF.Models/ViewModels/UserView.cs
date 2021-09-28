using BFF.Models.Attributes;
using Common.Models.DbModels;

namespace BFF.Models.ViewModels
{
    public class UserView : ViewModel
    {
        // [TableFormField(Label = "Id Number")]
        public string IdNumber { get; set; }
        // [TableFormField(Label = "Username")]
        public string Username { get; set; }
        // [TableFormField(Label = "Email Address")]
        public string EmailAddress { get; set; }
    }
}