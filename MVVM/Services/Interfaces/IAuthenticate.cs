using MotorEmpireAutohaus.MVVM.Models.User_Account_Model;
using MotorEmpireAutohaus.MVVM.View_Models.Account;

namespace MotorEmpireAutohaus.MVVM.Services.Interfaces
{
    interface IAuthenticate
    {
        public bool Login(UserAccount user);

        public bool SignUp(UserAccount user);
    }
}