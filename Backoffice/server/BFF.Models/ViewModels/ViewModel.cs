using System;
using System.ComponentModel.DataAnnotations;
using BFF.Models.Attributes;
using BFF.Models.Interfaces;
using Common.Utils.DbModels;

namespace BFF.Models.ViewModels
{
    public class ViewModel : IViewModel
    {
        [TableFormField(ReadOnly = true)]
        [Required]
        public string Id { get; set; }
        [TableFormField(Label = "Creation Time", ReadOnly = true)]
        [TableColumn(Label = "Creation Time")]
        public DateTime CreateAt { get; set; }
        [TableFormField(Label = "Last Update", ReadOnly = true)]
        [TableColumn(Label = "Last Update")]
        public DateTime ModifyAt { get; set; }
    }
}