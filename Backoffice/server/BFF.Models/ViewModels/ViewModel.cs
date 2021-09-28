using System;
using BFF.Models.Attributes;
using BFF.Models.Interfaces;
using Common.Utils.DbModels;

namespace BFF.Models.ViewModels
{
    public class ViewModel : IViewModel
    {
        public string Id { get; set; }
        [TableFormField(Label = "Creation Time")]
        public DateTime CreateAt { get; set; }
        [TableFormField(Label = "Last Update")]
        public DateTime ModifyAt { get; set; }
    }
}