using System;
using BFF.Models.Attributes;
using BFF.Models.Interfaces;
using Common.Utils.DbModels;

namespace BFF.Models.ViewModels
{
    public class ViewModel : IViewModel
    {
        public string Id { get; set; }
        [TableColumn(Name = "Creation Time")]
        public DateTime CreateAt { get; set; }
        [TableColumn(Name = "Last Update")]
        public DateTime ModifyAt { get; set; }
    }
}