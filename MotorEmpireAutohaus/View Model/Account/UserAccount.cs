using CommunityToolkit.Mvvm.ComponentModel;
using MotorEmpireAutohaus.Services.Account_Services;
using CommunityToolkit.Mvvm.Input;
using MotorEmpireAutohaus.Misc.Authentication;
using MotorEmpireAutohaus.View;

namespace MotorEmpireAutohaus.View_Model.Account
{
    [QueryProperty (nameof(Name),nameof(Name))]
    public partial class UserAccount : User
    {
        private readonly AccountService _accountService;
        private readonly AuthValidation _authValidation;

        [ObservableProperty]
        private string _emailAddress;

        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _passwordConfirmation;

        [ObservableProperty]
        private string _profileImageUrl;

        public UserAccount() { _accountService = new AccountService(); }

        public UserAccount(string name, string emailAddress, string username, string password) : base(name, password)
        {
            this._emailAddress = emailAddress;
            this._username = username;
        }

        public UserAccount(string UUID, string name, string emailAddress, string username, string password,string profileImageURL) : base(UUID, name, password)
        {
            this._emailAddress = emailAddress;
            this._username = username;
            this._profileImageUrl= profileImageURL;
        }

        //Dependency injection
        public UserAccount(AuthValidation authValidation, AccountService accountService)
        {
            this._accountService = accountService;
            this._authValidation = authValidation;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is UserAccount)) return false;

            UserAccount castedProp = obj as UserAccount;
            if (Username == castedProp.Username && EmailAddress == castedProp.EmailAddress)
                return true;

            return false;

        }

        public static bool operator ==(UserAccount a, UserAccount b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(UserAccount a, UserAccount b)
        {
            return !(a == b);
        }

        public override bool IsEmpty()
        {
            if (this == null)
            {
                return true;
            }

            return (UUID == "" || Name == "" || EmailAddress == "" || Password == "" || Username == "");
        }

        public override int GetHashCode()
        {
            object o = new();
            return o.GetHashCode();
        }

        [RelayCommand]
        public async void Login()
        {
            if (_authValidation.ValidateLogin(this))
            {
                EmailAddress = EmailAddress.ToLower();
                if (_accountService.Login(this))
                {
                    //CrossPlatformMessageRenderer.RenderMessages("Login Success!", "OK",2);
                    await Shell.Current.GoToAsync($"{nameof(MotorEmpire)}?Name={Name}",
                                                  true);
                }
            }
        }

        [RelayCommand]
        public async void Register()
        {
            if (_authValidation.ValidateSignUp(this))
            {
                if (_accountService.SignUp(this))
                {
                    await Shell.Current.GoToAsync("//LogIn", true);
                }
            }
        }
    }
}

