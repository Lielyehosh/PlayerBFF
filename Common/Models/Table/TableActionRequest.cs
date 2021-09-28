using System.Collections.Generic;

namespace Common.Models.Table
{
    public class TableActionRequest
    {
        public string Referer { get; set; }
        
        public bool IsConfirmed { get; set; }

        /// <summary>
        /// List of checked IDS, relevant if IsAll is false
        /// </summary>
        public List<string> Ids { get; set; }
        
        /// <summary>
        /// Should perform on all matching filters
        /// </summary>
        public bool IsAll { get; set; }
        
        /// <summary>
        /// Filters that the table is filtered by (used for IsAll = true)
        /// </summary>
        public List<TableQueryFilter> Filters { get; set; }
        
    }
    
    public class TableActionRequestWithData<T> : TableActionRequest
    {
        /// <summary>
        /// Custom data added from dialog
        /// </summary>
        public T DialogData { get; set; }
        
    }
}