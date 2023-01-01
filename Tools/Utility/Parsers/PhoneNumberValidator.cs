using System.Text.RegularExpressions;

namespace Tools.Utility.Parsers
{
    public static class PhoneNumberValidator
    {
        private const string Pattern =
            "^\\+?\\d{1,4}?[-.\\s]?\\(?\\d{1,3}?\\)?[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,9}$";


        public static bool IsPhoneNumberValid(string phoneNumber)
        {
            return phoneNumber is not null && Regex.IsMatch(phoneNumber, Pattern);
        }
    }
}