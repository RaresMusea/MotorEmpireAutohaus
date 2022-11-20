using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.Services.Account_Services
{
    public class AccountService
    {

        private static string tableReference = "user";

        public void SignUp()
        {
             Shell.Current.GoToAsync("//LandingPage");
        }
    }
}
