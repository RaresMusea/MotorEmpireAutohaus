using UserAccount = MVVM.Models.User_Account_Model.UserAccount;

namespace MVVM.Services.Interfaces
{
    public interface IAuthenticate
    {
        public bool Login(UserAccount user);

        public bool SignUp(UserAccount user);
    }
}