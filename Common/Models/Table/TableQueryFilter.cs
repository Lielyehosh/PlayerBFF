namespace Common.Models.Table
{
    public class TableQueryFilter
    {
        public string Field { get; set; }
        
        public TableOperator Operator { get; set; }
        
        public object Value { get; set; }
    }
}