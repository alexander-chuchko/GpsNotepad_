using System.Text.RegularExpressions;

namespace GpsNotepad.Helpers
{
    public class Validation
    {
        private static Regex patternForName;
        private static Regex patternForEmailAddress;
        private static Regex patternForPassword;
        static Validation()
        {
            patternForName = new Regex(@"^[A-Z]([a-z][A-Z]?){2,15}$");
            patternForEmailAddress = new Regex(@"^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}");
            patternForPassword = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z!#$%&()*+,./:;<=>?@^_{|}[\]~\d]{8,16}");
        }
        public static bool IsValidatedName(string name)
        {
            var validationResult = false;
            if (patternForName.IsMatch(name))
            {
                validationResult = true;
            }
            return validationResult;
        }
        public static bool IsValidatedEmail(string email)
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
        public static bool IsInformationInLabelAndLatitudeAndLongitude(string label, string description, string latitude, string longitude)
        {
            var validationResult = false;
            if (!string.IsNullOrWhiteSpace(label) && !string.IsNullOrWhiteSpace(description)&&!string.IsNullOrWhiteSpace(latitude)&&!string.IsNullOrWhiteSpace(longitude)) //I will finish
            {
                validationResult = true;
            }
            return validationResult;
        }
    }
}
