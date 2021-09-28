using System.Collections.Generic;

namespace Common.Models.Table
{
    public class TableAction
    {
        public string Id { get; set; }
        
        public string Label { get; set; }

        public string Dialog { get; set; }
        
        public bool IsMulti { get; set; }
        
        public object Filter { get; set; }
        
        public bool IsGlobal { get; set; }

        public List<string> Tags { get; set; }
        
        public bool ShowInTable { get; set; }
        
        public bool ShowInForm { get; set; }

        public bool? NoQuery { get; set; }
    }
}