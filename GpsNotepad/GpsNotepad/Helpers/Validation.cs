using System.Text.RegularExpressions;

namespace GpsNotepad.Helpers
{
    public class Validation
    {
        private static Regex patternForEmailAddress;
        private static Regex patternForPassword;
        static Validation()
        {
            //^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}) - проверка для email address
            //patternForEmailAddress = new Regex(@"(^[^0-9]{4,16})");
            patternForEmailAddress = new Regex(@"^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}");
            patternForPassword = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z!#$%&()*+,./:;<=>?@^_{|}[\]~\d]{8,16}");
        }
        public static bool IsValidatedLogin(string email)
        {
            var validationResult = false;
            if (patternForEmailAddress.IsMatch(email))
            {
                validationResult = true;
            }
            return validationResult;
        }
        public static bool IsValidatedPassword(string password)
        {
            var validationResult = false;
            if (patternForPassword.IsMatch(password))
            {
                validationResult = true;
            }
            return validationResult;
        }
        public static bool CompareStrings(string password, string confirmPassword)
        {
            var comparisonResult = false;
            var returnedResult = string.Compare(password, confirmPassword, false);
            if (returnedResult == 0)
            {
                comparisonResult = true;
            }
            return comparisonResult;
        }
        //Method for checking the existence of information
        public static bool IsInformationInNameAndNickName(string label, string latitude, string longitude)
        {
            var validationResult = false;
            if (!string.IsNullOrEmpty(label) && !string.IsNullOrEmpty(latitude)&&!string.IsNullOrEmpty(longitude)) //I will finish
            {
                validationResult = true;
            }
            return validationResult;
        }
    }
}
