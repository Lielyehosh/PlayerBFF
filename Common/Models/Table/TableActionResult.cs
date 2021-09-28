namespace Common.Models.Table
{
    public class TableActionResult
    {
        public string Confirm { get; set; }
        
        public string Success { get; set; }
        
        public string Error { get; set; }
        
        public bool Refresh { get; set; }
        
        public string Redirect { get; set; }
        
        public static TableActionResult ConfirmResult(string message)
        {
            return new TableActionResult {Confirm = message};
        }
    }
}