using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.View_Model.Account
{
    interface IAuthenticate
    {
        public bool Login(UserAccount user);

        public bool SignUp(UserAccount user);
    }
}
