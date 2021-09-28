using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Common.Models.Table
{
    public class TableColumnField
    {
        public string Id { get; set; }
        
        public string Label { get; set; }
        
        public List<TableOperator> Operators { get; set; }
        
        public bool Sortable { get; set; }

        public string Formatter { get; set; }

        [JsonIgnore]
        public TableFieldType Type { get; set; }

    }
}