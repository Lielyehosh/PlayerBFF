using System;

namespace BFF.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TableIgnoreChoicesAttribute : Attribute
    {
        public TableIgnoreChoicesAttribute(params object[] ignoreValues)
        {
            IgnoreValues = ignoreValues;
        }

        public object[] IgnoreValues { get; }
    }
}