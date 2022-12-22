using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tools.Utility.Parsers
{
    public class PhoneNumberValidator
    {
        private const string pattern = "^\\+?\\d{1,4}?[-.\\s]?\\(?\\d{1,3}?\\)?[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,9}$";


        public static bool IsPhoneNumberValid(string phoneNumber)
        {
            if(phoneNumber is not null)
            {
                return Regex.IsMatch(phoneNumber, pattern);
            }
            return false;
        }
    }
}
