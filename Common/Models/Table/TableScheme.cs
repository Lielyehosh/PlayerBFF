using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Common.Models.Table
{
    public class TableScheme
    {
        public FormScheme FormScheme { get; set; }
        
        public string Id { get; set; }
        
        public string Label { get; set; }
        
        /// <summary>
        /// List of table columns, by order
        /// </summary>
        public List<TableColumnField> Columns { get; set; }
        
        /// <summary>
        /// List of actions
        /// </summary>
        public List<TableAction> Actions { get; set; }
        
        public int? PaginationLimit { get; set; }
        
        public bool? NoDelete { get; set; }
        
        public bool? NoCreate { get; set; }
        
        public bool? NoEdit { get; set; }
        
        [JsonIgnore]
        public Dictionary<string, TableFormField> FieldsById { get; set; }
        
        [JsonIgnore]
        public Dictionary<string, TableColumnField> ColumnsById { get; set; }
        
        public SortItem DefaultSort { get; set; }
        
        /// <summary>
        /// Any tags associated with table
        /// </summary>
        public List<string> Tags { get; set; }
        
        /// <summary>
        /// Can items in table be ordered
        /// </summary>
        public bool IsOrderable { get; set; }
    }
    
}