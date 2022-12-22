using CommunityToolkit.Mvvm.ComponentModel;
using MotorEmpireAutohaus.Services.Account_Services;
using CommunityToolkit.Mvvm.Input;
using MotorEmpireAutohaus.MVVM.Models.User_Account_Model;
using MotorEmpireAutohaus.MVVM.Services.Authentication;
using MotorEmpireAutohaus.MVVM.View_Models.Base;
using System.Runtime.InteropServices;
using System.Windows;
using CommunityToolkit.Maui.Layouts;
using MotorEmpireAutohaus.Tools.Utility.Messages;
using System.ComponentModel;
using MotorEmpireAutohaus.MVVM.Models.Base;

namespace MotorEmpireAutohaus.MVVM.View_Models.Account
{

    [QueryProperty (nameof(UserAccount), nameof(UserAccount))]
    public partial class UserAccountViewModel : BaseViewModel
    {
        //Win32 API call
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

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

        private UserAccount userCopy;

        [ObservableProperty]
        private bool isEditable=false;

        [ObservableProperty]
        private string editButtonText="Edit account information";

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
            userCopy = new UserAccount();
            isEditable = false;            
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
            userCopy = (UserAccount)_accountService.RetrieveByUuid(this.user.UUID);
            userCopy.Password = this.user.Password;
        }

        private static async Task TakePhoto()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo is not null)
                {
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                    using Stream sourceItem = await photo.OpenReadAsync();
                    using FileStream localFileStream = File.OpenWrite(localFilePath);
                    await sourceItem.CopyToAsync(localFileStream);
                }
            }
        }

        private bool CheckForIdenticalCredentialsAtUpdate()
        {
            UserAccount databaseEntry = (UserAccount) _accountService.RetrieveByUuid(user.UUID);

            if (!(user.Username!=databaseEntry.Username || user.Name!=databaseEntry.Name || user.EmailAddress!=databaseEntry.EmailAddress))
            {
                CrossPlatformMessageRenderer.RenderMessages("Cannot modify your account details since your new credentials are identical to the old ones. Try modifying your current details by adding new credentials and then try again.", "Okay", 5);
                return true;
            }

            return false;
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

        [RelayCommand]
        public async void ToggleOptions()
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
            }
            else
            {
                string action = await Application.Current.MainPage.DisplayActionSheet("Manage Profile Picture", "Cancel", null, "Upload a live photo", "Upload a photo from your device");
                if(action=="Upload a live photo")
                {
                    await TakePhoto();
                }
            }
        }



        [RelayCommand]
        public async void TakePhotoAndUpload()
        {
            await TakePhoto();  
        }

        [RelayCommand]
        public async void EditButtonTextClicked()
        {
            IsEditable = !IsEditable;
            if (!IsEditable)
            {
                if (!CheckForIdenticalCredentialsAtUpdate())
                {
                    if (_authValidation.ValidateNewCredentialsBeforeUpdate(user))
                    {
                        bool answerIsPositive = await Application.Current.MainPage.DisplayAlert("Motor Empire Autohaus-Update account information",
                            "Are you sure that you want to modify your account information?\nYour old credentials will be permanently lost!\nYou will also need to use the new credentials on your further authentications within the app.\n Continue?",
                            "Proceed to account info update",
                            "Cancel");

                        if (answerIsPositive)
                        {
                            user = (UserAccount)_accountService.Update(user);
                            CrossPlatformMessageRenderer.RenderMessages($"Update successfull, {user.Name}!\n You'll be automatically redirected to the main page of the application...", "Allright", 4);
                            await Shell.Current.GoToAsync("../MotorEmpire", true, new Dictionary<string, object>
                            {
                                { "UserAccount",user }
                            });
                        }
                    }
                }
            }
        }

        [RelayCommand]
        public void RevertEditChanges()
        {
            string currentPassword = user.Password;
            UserAccount oldUser=(UserAccount) _accountService.RetrieveByUuid(user.UUID);
            user.Name = oldUser.Name;
            user.EmailAddress=oldUser.EmailAddress;
            user.Username= oldUser.Username;
            user.UUID=oldUser.UUID;
            user.ProfileImageUrl=oldUser.ProfileImageUrl;
            user.Password = currentPassword;
        }

    }
}

