using System.Text.RegularExpressions;

namespace AuthMS.Utils
{
    public static class Validator
    {
        public static bool ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;
            return true;
        }

        public static bool ValidateIdNumber(string idNumber)
        {
            var numberRegex = new Regex(@"^\d+$");
            if (string.IsNullOrEmpty(idNumber) || !numberRegex.IsMatch(idNumber))
                return false;
            return true;
        }

        public static bool ValidateUsername(string username)
        {
            var numberRegex = new Regex(@"^\d+$");
            if (string.IsNullOrEmpty(username))
                return false;
            return true;
        }
        
        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;
            return true;
        }
        
    }
}