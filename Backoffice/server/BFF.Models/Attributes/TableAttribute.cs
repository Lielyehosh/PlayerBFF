using System;

namespace BFF.Models.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public TableAttribute(string label)
        {
            Label = label;
        }
        public string Label { get; set; }
        public int PaginationLimit { get; set; }
        public bool NoDelete { get; set; }
        public bool NoCreate { get; set; }
        public bool NoEdit { get; set; }

    }
}