using System.Collections.Generic;

namespace Common.Models.Table
{
    /// <summary>
    /// Scheme of a form object
    /// </summary>
    public class TableForm
    {
        public string Title { get; set; }
        public List<TableFormField> Fields { get; set; }
    }
}