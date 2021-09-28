using System;
using Common.Models.Table;

namespace BFF.Models.Attributes
{
    public class TableColumnAttribute : Attribute
    {
        private bool? _notSortable;

        /// <summary>
        /// Ordinal of column
        /// </summary>
        public int Ordinal { get; set; }

        /// <summary>
        /// Label of column
        /// </summary>
        public string Label { get; set; }
        
        /// <summary>
        /// Column operators
        /// </summary>
        public TableOperator[] Operators { get; set; }

        /// <summary>
        /// Is column sortable
        /// </summary>
        public bool NotSortable
        {
            get => _notSortable ?? false;
            set => _notSortable = value;
        }

        public bool? IsNotSortable => _notSortable;

        /// <summary>
        /// Does this table sorted by default using this field
        /// </summary>
        public bool IsDefaultSort { get; set; }
        
        /// <summary>
        /// If this field is sorted by default, is it Descending?
        /// </summary>
        public bool DefaultSortDesc { get; set; }
        
        /// <summary>
        /// Explicit db field for query and mapping
        /// </summary>
        public string DbField { get; set; }
        
        public string Formatter { get; set; }
    }
}