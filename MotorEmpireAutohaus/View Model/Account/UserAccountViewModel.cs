using CommunityToolkit.Mvvm.ComponentModel;
using MotorEmpireAutohaus.Services.Account_Services;
using CommunityToolkit.Mvvm.Input;
using MotorEmpireAutohaus.Misc.Authentication;
using MotorEmpireAutohaus.View;
using MotorEmpireAutohaus.View_Model.Base;
using MotorEmpireAutohaus.Models.User_Account_Model;

namespace MotorEmpireAutohaus.View_Model.Account
{

    public partial class UserAccountViewModel : BaseViewModel
    {
        private readonly AccountService _accountService;
        private readonly AuthValidation _authValidation;
        /*
                [ObservableProperty]
                private string _emailAddress;

                [ObservableProperty]
                private string _username;

                [ObservableProperty]
                private string _passwordConfirmation;

                [ObservableProperty]
                private string _profileImageUrl;*/

        [ObservableProperty]
        private UserAccount user;

        public UserAccountViewModel() { _accountService = new AccountService(); }

/*        public UserAccountViewModel(string name, string emailAddress, string username, string password) : base(name, password)
        {
            this._emailAddress = emailAddress;
            this._username = username;
        }*/
/*
        public UserAccountViewModel(string UUID, string name, string emailAddress, string username, string password,string profileImageURL) : base(UUID, name, password)
        {
            this._emailAddress = emailAddress;
            this._username = username;
            this._profileImageUrl= profileImageURL;
        }*/

        //Dependency injection
        public UserAccountViewModel(AuthValidation authValidation, AccountService accountService,UserAccount user)
        {
            _accountService = accountService;
            _authValidation = authValidation;
            this.user= user;
        }

        [RelayCommand]
        public async void Login()
        {
            if (_authValidation.ValidateLogin(user))
            {
                user.EmailAddress = user.EmailAddress.ToLower();
                if (_accountService.Login(user))
                {
                    //CrossPlatformMessageRenderer.RenderMessages("Login Success!", "OK",2);
                    await Shell.Current.GoToAsync($"//MotorEmpire?Name={user.Name}",
                                                  true);
                }
            }
        }

        partial void OnUserChanged(UserAccount value)
        {
            user = value;
        }

        [RelayCommand]
        public async void Register()
        {
            if (_authValidation.ValidateSignUp(user))
            {
                if (_accountService.SignUp(user))
                {
                    await Shell.Current.GoToAsync("//LogIn", true);
                }
            }
        }
    }
}

