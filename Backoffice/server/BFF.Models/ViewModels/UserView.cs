using BFF.Models.Attributes;
using Common.Models.DbModels;

namespace BFF.Models.ViewModels
{
    public class UserView : ViewModel
    {
        [TableFormField(Required = true)]
        [TableColumn]
        public string IdNumber { get; set; }
        [TableFormField(Required = true)]
        [TableColumn]
        public string Username { get; set; }
        [TableFormField(Required = true)]
        [TableColumn]
        public string EmailAddress { get; set; }
    }
}