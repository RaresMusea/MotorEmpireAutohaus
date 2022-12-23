using CommunityToolkit.Mvvm.ComponentModel;
using MotorEmpireAutohaus.Services.Account_Services;
using CommunityToolkit.Mvvm.Input;
using MotorEmpireAutohaus.MVVM.Models.User_Account_Model;
using MotorEmpireAutohaus.MVVM.Services.Authentication;
using MotorEmpireAutohaus.MVVM.View_Models.Base;
using System.Runtime.InteropServices;
using MotorEmpireAutohaus.Tools.Utility.Messages;
using MotorEmpireAutohaus.Tools.Encryption;
using Tools.Utility.Parsers;
using MotorEmpireAutohaus.Storage.Firebase_Storage;
using MotorEmpireAutohaus.View;

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

        [ObservableProperty]
        private UserAccount user;

        private UserAccount userCopy;

        [ObservableProperty]
        private bool isEditable=false;

        [ObservableProperty]
        private string editButtonText="Edit account information";

        [ObservableProperty]
        private bool addedPhoneNumber;

        [ObservableProperty]
        private bool addPhoneNumberVisible;

        [ObservableProperty]
        private bool removePhoneNumberVisible;



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
            user.UUID = _accountService.GetUuidByEmail(user.EmailAddress);
            this.user= user;
            userCopy = new UserAccount();
            isEditable = false;
            AddedPhoneNumber = !_accountService.HasPhoneNumber(user.UUID);
            AddPhoneNumberVisible = ! _accountService.HasPhoneNumber(user.UUID);
            removePhoneNumberVisible = _accountService.HasPhoneNumber(user.UUID);
        }

        partial void OnAddPhoneNumberVisibleChanged(bool value)
        {
            AddPhoneNumberVisible = value;
            RemovePhoneNumberVisible = !value;
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
                    await Shell.Current.GoToAsync($"{nameof(MotorEmpire)}?Name={user.Name}",
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

        private static async Task<FileResult> TakePhoto()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo is not null)
                {
                    return photo;
                }

                CrossPlatformMessageRenderer.RenderMessages("Your device does not support instant capture!\nIn order to use this feature, you'll need a device which supports instant photo/video capture!", "OK", 5);
                return null;
            }

            return null;
        }

        private static async Task<FileResult> PickPhoto()
        {
            FileResult photo = await MediaPicker.PickPhotoAsync();
            if(photo is not null)
            {
                return photo;
            }

            CrossPlatformMessageRenderer.RenderMessages("An error occured while attempting to acess a resource which was recently accessed.\nPlease try again!", "Retry", 5);
            return null;
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
                    await TakePhotoAndUpload();
                }
                else
                {
                    await PickPhotoAndUpload();
                }
            }
        }



        [RelayCommand]
        public async Task TakePhotoAndUpload()
        {
            FileResult photo= await TakePhoto(); 
            if (photo is not null)
            {
                string path = $"Images/ProfilePictures/{user.UUID}";
                string urlToPicture = await FirebaseCloudStorage.AddFileToFirebaseCloudStorageAsync(photo, path);

                if(urlToPicture is not null)
                {
                    _accountService.UpdateProfilePictureForUser(user.UUID, urlToPicture);
                    user.ProfileImageUrl = urlToPicture;
                    return;
                }
            }
            CrossPlatformMessageRenderer.RenderMessages("An error occurred while trying to manipulate the photo you've just took. Retry to take the picture and try again!", "Retry", 4);
        }

        [RelayCommand]
        public async Task PickPhotoAndUpload()
        {
            FileResult photo=await PickPhoto();

            if(photo is not null)
            {
                string path = $"Images/ProfilePictues/{user.UUID}";
                string urlToPicture = await FirebaseCloudStorage.AddFileToFirebaseCloudStorageAsync(photo, path);

                if(urlToPicture is not null)
                {
                    _accountService.UpdateProfilePictureForUser(user.UUID, urlToPicture);
                    user.ProfileImageUrl= urlToPicture;
                    return;
                }
            }

            CrossPlatformMessageRenderer.RenderMessages("An error occurred while attempting to manipulate the picture you've just chosen. Try to pick the profile picture again!", "Retry", 4);
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
                            await Shell.Current.GoToAsync($"//MotorEmpire?Name={user.Name}", true);
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

        [RelayCommand]
        public async void ChangePassword()
        {
            bool answerIsPositive = await Application.Current.MainPage.DisplayAlert("Motor Empire Autohaus-Password Change",
                           "Changing password will edit the credentials used for authentication.\nContinue?",
                           "Change my password",
                           "Cancel");
            
            if(answerIsPositive)
            {
                string oldPassword = await Application.Current.MainPage.DisplayPromptAsync("Password Change", "Before changing the password, type your most recent password here.\n This will help us verify that you are the owner of this account.\n");
                if(Encrypter.EncryptPassword(oldPassword)==_accountService.RetrievePasswordByUuid(user.UUID))
                {
                    string newPassword = "";

                    do
                    {
                        newPassword = await Application.Current.MainPage.DisplayPromptAsync("Password Change", "Type your new password here:\n");
                        var passwordValidation=_authValidation.ValidatePassword(newPassword);
                        if (!passwordValidation.ValidationPassed)
                        {
                            CrossPlatformMessageRenderer.RenderMessages(passwordValidation.Remark,"Retry",5);
                        }
                    }
                    while (!_authValidation.ValidatePassword(newPassword).ValidationPassed);

                    _accountService.UpdatePasswordForUser(user, newPassword);
                    CrossPlatformMessageRenderer.RenderMessages("Password changed successfully!", "Ok", 5);

                }
            }
        }



        [RelayCommand]
        public async void AddPhoneNumber()
        {
            bool answerIsPositive = await Application.Current.MainPage.DisplayAlert("Motor Empire Autohaus-Provide a phone number",
               "Adding your phone number will help other users find and get in touch with you more easilly.\nWe care for your privacy, so we assure you that your number will only be used within this application.\nDo you want to add your number? You can remove it anytime.",
               "Add my phone number",
               "I'll add it later on");

            if (answerIsPositive)
            {
                string phoneNumber = await Application.Current.MainPage.DisplayPromptAsync("Add a phone number", "Type your phone number here:\n","OK","Cancel",null,15,Keyboard.Numeric,"+40 (Change the prefix according to your country)");
                
                if(!PhoneNumberValidator.IsPhoneNumberValid(phoneNumber))
                {
                    CrossPlatformMessageRenderer.RenderMessages("The provided phone number is invalid!\nPlease try again!", "Retry", 5);
                    return;
                }

                if (_accountService.SetPhoneNumber(user.UUID, phoneNumber))
                {
                    AddPhoneNumberVisible = false;
                    removePhoneNumberVisible = true;
                    user.PhoneNumber = phoneNumber;
                }

            }
        }

        [RelayCommand]
        public async void RemovePhoneNumber()
        {
            bool answerIsPositive = await Application.Current.MainPage.DisplayAlert("Motor Empire Autohaus-Remove the provided phone number",
               "Remove the phone number linked to your account?\nYou can add it back anytime.",
               "Yes, I want to remove my phone number associated with this acccount",
               "No");

            if(answerIsPositive) 
            {
                if (_accountService.RemovePhoneNumber(user.UUID))
                {
                    AddPhoneNumberVisible = true;
                    removePhoneNumberVisible = false;
                    user.PhoneNumber= "";
                }
            }
        }

        [RelayCommand]
        public async void DeleteAccount()
        {
            bool answerIsPositive = await Application.Current.MainPage.DisplayAlert("Motor Empire Autohaus-Delete account",
                           "Are you sure that you want to delete your account?\n" +
                           "This action is irreversible!\nYou won't be able to recover your data after this step and all of your posts and informations shared within the app will be permanently erased.\n" +
                           "Continue?",
                           "Yes, delete my account",
                           "No, I want to keep my account");

            if (answerIsPositive)
            {
                if (_accountService.Delete(user))
                {
                    user = null;
                    await Shell.Current.GoToAsync("//SignUp", true);
                }
            }
        }

    }
}

