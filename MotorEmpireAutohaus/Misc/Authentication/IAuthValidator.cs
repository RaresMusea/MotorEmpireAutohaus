using MotorEmpireAutohaus.Misc.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.Misc.Authentication
{
    public interface IAuthValidator
    {
        public AuthValidationResult ValidateName(string name);
        
        public AuthValidationResult ValidatePassword(string password);

        public AuthValidationResult ValidatePasswords(string password, string passwordConfirmation);

        public AuthValidationResult ValidateEmailAddress(string emailAddress);

        public bool ArePasswordsMatching(string password, string passwordConfirmation);

        public AuthValidationResult ValidateUsername(string userName); 

    }
}
