using System;

namespace BFF.Models.Attributes
{
    /// <summary>
    /// Customize Table Form Field
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class TableFormFieldAttribute : Attribute
    {
        /// <summary>
        /// Do not add this field to form at all. Will not be sent to client
        /// </summary>
        public bool Ignore { get; set; }
        
        /// <summary>
        /// Label of field input
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Is this field required, or can it be null?
        /// </summary>
        public bool Required { get; set; }
        
        /// <summary>
        /// Hide inside form, but still send in request
        /// </summary>
        public bool Hidden { get; set; }
        
        /// <summary>
        /// Show in form, but as read only
        /// </summary>
        public bool ReadOnly { get; set; }
    }

}