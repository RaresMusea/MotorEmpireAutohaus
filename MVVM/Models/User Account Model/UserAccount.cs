using CommunityToolkit.Mvvm.ComponentModel;

namespace MVVM.Models.User_Account_Model
{
    public partial class UserAccount : Account
    {
        [ObservableProperty] private string emailAddress;

        [ObservableProperty] private string username;

        [ObservableProperty] private string profileImageUrl;

        [ObservableProperty] private string passwordConfirmation;

        [ObservableProperty] private string phoneNumber;

        public UserAccount()
        {
        }

        public UserAccount(string name, string emailAddress, string username, string password) : base(name, password)
        {
            EmailAddress = emailAddress;
            Username = username;
        }

        public UserAccount(string uuid, string name, string emailAddress, string username, string password,
            string profileImageUrl) : base(uuid, name, password)
        {
            EmailAddress = emailAddress;
            Username = username;
            ProfileImageUrl = profileImageUrl;
        }

        public UserAccount(string uuid, string name, string emailAddress, string username, string password,
            string profileImageUrl, string phoneNumber) : base(uuid, name, password)
        {
            EmailAddress = emailAddress;
            Username = username;
            ProfileImageUrl = profileImageUrl;
            PhoneNumber = phoneNumber;
        }

        public override bool Equals(object obj)
        {
            if(this == null && obj == null)
            {
                return true;
            }

            if (this == null)
            {
                return false;
            }

            var account = obj as UserAccount;
            if (account is null) return false;

            UserAccount castedProp = account;
            if (Username == castedProp.Username && EmailAddress == castedProp.EmailAddress)
                return true;

            return false;
        }

        public static bool operator == (UserAccount a, UserAccount b)
        {
            return a!.Equals(b);
        }

        public static bool operator !=(UserAccount a, UserAccount b)
        {
            return !(a == b);
        }

        public static explicit operator UserAccount(string v)
        {
            throw new NotImplementedException();
        }

        public override bool IsEmpty()
        {
            return (Uuid == "" || Name == "" || EmailAddress == "" || Password == "" || Username == "");
        }

        public override int GetHashCode()
        {
            object o = new();
            return o.GetHashCode();
        }
    }
}