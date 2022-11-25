using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.Misc.Common
{
    public interface ISignUpValidator
    {
        public string FormatName(string name);

        public bool ArePasswordsMatching(string password, string matchingPassword);

        public AuthValidationResult IsNameValid(string name);

        public AuthValidationResult SignUpPasswordValidation(string password, string passwordConfirmation);
    }
}
