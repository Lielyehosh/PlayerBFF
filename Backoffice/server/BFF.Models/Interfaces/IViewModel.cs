using System;

namespace BFF.Models.Interfaces
{
    public interface IViewModel
    {
        string Id { get; set; }
        DateTime CreateAt { get; set; }
        DateTime ModifyAt { get; set; }
    }
}