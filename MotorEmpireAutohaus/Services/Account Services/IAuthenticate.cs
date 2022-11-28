using MotorEmpireAutohaus.View_Model.Account;

namespace MotorEmpireAutohaus.Services.Account_Services
{
    interface IAuthenticate
    {
        public bool Login(UserAccount user);

        public bool SignUp(UserAccount user);
    }
}