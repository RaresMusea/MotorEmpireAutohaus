using MVVM.Models.User_Account_Model;

namespace MVVM;

public static class UserPreferencesProvider
{
    public static UserAccount LoggedInAccount { get; set; }
}
