using System;
using BFF.Models.Attributes;
using BFF.Models.Interfaces;
using Common.Utils.DbModels;

namespace BFF.Models.ViewModels
{
    public class ViewModel : IViewModel
    {
        [TableFormField(ReadOnly = true)]
        public string Id { get; set; }
        [TableFormField(Label = "Creation Time", ReadOnly = true)]
        public DateTime CreateAt { get; set; }
        [TableFormField(Label = "Last Update", ReadOnly = true)]
        public DateTime ModifyAt { get; set; }
    }
}