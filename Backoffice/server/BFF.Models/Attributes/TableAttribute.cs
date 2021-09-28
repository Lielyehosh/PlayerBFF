using System;

namespace BFF.Models.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class TableAttribute: Attribute
    {
        public TableAttribute(string label)
        {
            Label = label;
        }

        /// <summary>
        /// Label of field input
        /// </summary>
        public string Label { get; set; }

    }
}