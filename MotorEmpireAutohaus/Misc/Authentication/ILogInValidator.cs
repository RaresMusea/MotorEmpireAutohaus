using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.Misc.Common
{
    public interface ILogInValidator
    {
        public AuthValidationResult IsPassowrdValid();
        public AuthValidationResult IsEmailValid();
        public AuthValidationResult ValidateLogin();
    }
}
