using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVM.Services.Account_Services;
using MVVM.Services.Authentication;
using MVVM.View.Favorite_Posts;
using Storage.Firebase_Storage;
using Tools.Encryption;
using Tools.Handlers;
using Tools.Utility.Messages;
using Tools.Utility.Parsers;
using BaseViewModel = MVVM.View_Models.Base.BaseViewModel;
using UserAccount = MVVM.Models.User_Account_Model.UserAccount;

namespace MVVM.View_Models.Account
{
    [QueryProperty(nameof(UserAccount), nameof(UserAccount))]
    [QueryProperty(nameof(UpdateNeeded), nameof(UpdateNeeded))]
    public partial class UserAccountViewModel : BaseViewModel
    {
        
        private readonly AccountService accountService;
        private readonly AuthValidation authValidation;

        [ObservableProperty] private UserAccount user;

        private UserAccount userCopy;

        [ObservableProperty] private bool isEditable;

        [ObservableProperty] private string editButtonText = "Edit account information";

        [ObservableProperty] private bool addedPhoneNumber;

        [ObservableProperty] private bool addPhoneNumberVisible;

        [ObservableProperty] private bool removePhoneNumberVisible;

        [ObservableProperty] private bool updateNeeded;


        public UserAccountViewModel()
        {
            accountService = new AccountService();
        }

        //Dependency injection
        public UserAccountViewModel(AuthValidation authValidation, AccountService accountService, UserAccount user)
        {
            this.accountService = accountService;
            this.authValidation = authValidation;

            if (UserPreferencesProvider.LoggedInAccount is null)
            {
                if (Preferences.ContainsKey("email") && Preferences.Default.Get("email", "") != "")
                {
                    string email = Preferences.Default.Get("email", "");
                    string uuid = accountService.GetUuidByEmail(email);
                    this.user = (UserAccount)accountService.RetrieveByUuid(uuid);
                    this.user.Uuid = uuid;
                    this.user.EmailAddress = email;
                }
                else
                {
                    this.user = user;
                    this.user.Uuid = this.accountService.GetUuidByEmail(this.user.EmailAddress);
                }
            }
            else
            {
                this.user = UserPreferencesProvider.LoggedInAccount;
                this.user.Uuid = this.accountService.GetUuidByEmail(this.user.EmailAddress);
            }
            userCopy = new UserAccount();
            isEditable = false;
            AddedPhoneNumber = !this.accountService.HasPhoneNumber(this.user.Uuid);
            AddPhoneNumberVisible = !this.accountService.HasPhoneNumber(this.user.Uuid);
            removePhoneNumberVisible = this.accountService.HasPhoneNumber(this.user.Uuid);
        }

        partial void OnAddPhoneNumberVisibleChanged(bool value)
        {
            AddPhoneNumberVisible = value;
            RemovePhoneNumberVisible = !value;
        }


        [RelayCommand]
        private async void Login()
        {
            if (authValidation.ValidateLogin(user))
            {
                user.EmailAddress = user.EmailAddress.ToLower();
                if (accountService.Login(user))
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
            userCopy = (UserAccount)accountService.RetrieveByUuid(this.user.Uuid);
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

                CrossPlatformMessageRenderer.RenderMessages(
                    "Your device does not support instant capture!\nIn order to use this feature, you'll need a device which supports instant photo/video capture!",
                    "OK", 5);
                return null;
            }

            return null;
        }

        private static async Task<FileResult> PickPhoto()
        {
            try
            {
                FileResult photo = await MediaPicker.PickPhotoAsync();
                if (photo is not null)
                {
                    return photo;
                }
                else
                {
                    CrossPlatformMessageRenderer.RenderMessages(
                   "An error occured while attempting to access a resource which was recently accessed.\nPlease try again!",
                   "Retry", 5);
                }
            }
            catch (Exception)
            {
                CrossPlatformMessageRenderer.RenderMessages(
                    "An error occured while attempting to access a resource which was recently accessed.\nPlease try again!",
                    "Retry", 5);
                return null;
            }

            return null;
        }

        private bool CheckForIdenticalCredentialsAtUpdate()
        {
            UserAccount databaseEntry = (UserAccount)accountService.RetrieveByUuid(user.Uuid);

            if (!(user.Username != databaseEntry.Username || user.Name != databaseEntry.Name ||
                  user.EmailAddress != databaseEntry.EmailAddress))
            {
                CrossPlatformMessageRenderer.RenderMessages(
                    "Cannot modify your account details since your new credentials are identical to the old ones. Try modifying your current details by adding new credentials and then try again.",
                    "Okay", 5);
                return true;
            }

            return false;
        }

        [RelayCommand]
        private async void Register()
        {
            if (authValidation.ValidateSignUp(user))
            {
                if (accountService.SignUp(user))
                {
                    await Shell.Current.GoToAsync($"//MotorEmpire?Name={user.Name}",
                        true);
                }
            }
        }

        [RelayCommand]
        private async void ToggleOptions()
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                MouseHandler.mouse_event(MouseHandler.MouseeventRightDown | MouseHandler.MouseeventRightUp,
                    0, 0, 0, 0);
            }
            else
            {
                string action = await Application.Current!.MainPage!.DisplayActionSheet("Manage Profile Picture",
                    "Cancel", null, "Upload a live photo", "Upload a photo from your device");
                if (action == "Upload a live photo")
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
        private async Task TakePhotoAndUpload()
        {
            FileResult photo = await TakePhoto();
            if (photo is not null)
            {
                string path = $"Images/ProfilePictures/{user.Uuid}";
                string urlToPicture = await FirebaseCloudStorage.AddFileToFirebaseCloudStorageAsync(photo, path);

                if (urlToPicture is not null)
                {
                    accountService.UpdateProfilePictureForUser(user.Uuid, urlToPicture);
                    user.ProfileImageUrl = urlToPicture;
                    return;
                }
            }

            CrossPlatformMessageRenderer.RenderMessages(
                "An error occurred while trying to manipulate the photo you've just took. Retry to take the picture and try again!",
                "Retry", 4);
        }

        [RelayCommand]
        private async Task PickPhotoAndUpload()
        {
            try
            {
                FileResult photo = await PickPhoto();

                if (photo is not null)
                {
                    string path = $"Images/ProfilePictures/{user.Uuid}";
                    string urlToPicture = await FirebaseCloudStorage.AddFileToFirebaseCloudStorageAsync(photo, path);

                    if (urlToPicture is not null)
                    {
                        accountService.UpdateProfilePictureForUser(user.Uuid, urlToPicture);
                        user.ProfileImageUrl = urlToPicture;
                        return;
                    }
                }
                else
                {
                    CrossPlatformMessageRenderer.RenderMessages(
                    "An error occurred while attempting to manipulate the picture you've just chosen. Try to pick the profile picture again!",
                    "Retry", 4);
                    return;
                }
            }
            catch (Exception)
            {

                CrossPlatformMessageRenderer.RenderMessages(
                    "An error occurred while attempting to manipulate the picture you've just chosen. Try to pick the profile picture again!",
                    "Retry", 4);
            }
        }

        [RelayCommand]
        private async void EditButtonTextClicked()
        {
            IsEditable = !IsEditable;
            if (IsEditable) return;
            if (CheckForIdenticalCredentialsAtUpdate()) return;
            if (!authValidation.ValidateNewCredentialsBeforeUpdate(user)) return;
            bool answerIsPositive = await Application.Current!.MainPage!.DisplayAlert(
                "Motor Empire Autohaus-Update account information",
                "Are you sure that you want to modify your account information?\nYour old credentials will be permanently lost!\nYou will also need to use the new credentials on your further authentications within the app.\n Continue?",
                "Proceed to account info update",
                "Cancel");

            if (!answerIsPositive) return;
            user = (UserAccount)accountService.Update(user);
            CrossPlatformMessageRenderer.RenderMessages(
                $"Update successful, {user.Name}!\n You'll be automatically redirected to the main page of the application...",
                "Alright", 4);
            await Shell.Current.GoToAsync($"//MotorEmpire?Name={user.Name}", true);
        }

        [RelayCommand]
        private void RevertEditChanges()
        {
            string currentPassword = user.Password;
            UserAccount oldUser = (UserAccount)accountService.RetrieveByUuid(user.Uuid);
            user.Name = oldUser.Name;
            user.EmailAddress = oldUser.EmailAddress;
            user.Username = oldUser.Username;
            user.Uuid = oldUser.Uuid;
            user.ProfileImageUrl = oldUser.ProfileImageUrl;
            user.Password = currentPassword;
        }

        [RelayCommand]
        private async void GoToSavedPosts()
        {
            UpdateNeeded = true;
            await Shell.Current.GoToAsync($"{nameof(FavoritePosts)}?UpdateNeeded={UpdateNeeded}", true);
            UpdateNeeded = false;
        }

        [RelayCommand]
        private async void ChangePassword()
        {
            bool answerIsPositive = await Application.Current!.MainPage!.DisplayAlert(
                "Motor Empire Autohaus-Password Change",
                "Changing password will edit the credentials used for authentication.\nContinue?",
                "Change my password",
                "Cancel");

            if (!answerIsPositive) return;
            string oldPassword = await Application.Current.MainPage.DisplayPromptAsync("Password Change",
                "Before changing the password, type your most recent password here.\n This will help us verify that you are the owner of this account.\n");
            if (Encrypter.EncryptPassword(oldPassword) != accountService.RetrievePasswordByUuid(user.Uuid)) return;
            string newPassword;

            do
            {
                newPassword =
                    await Application.Current.MainPage.DisplayPromptAsync("Password Change",
                        "Type your new password here:\n");
                var passwordValidation = authValidation.ValidatePassword(newPassword);
                if (!passwordValidation.ValidationPassed)
                {
                    CrossPlatformMessageRenderer.RenderMessages(passwordValidation.Remark, "Retry", 5);
                }
            } while (!authValidation.ValidatePassword(newPassword).ValidationPassed);

            accountService.UpdatePasswordForUser(user, newPassword);
            CrossPlatformMessageRenderer.RenderMessages("Password changed successfully!", "Ok", 5);
        }


        [RelayCommand]
        private async void AddPhoneNumber()
        {
            bool answerIsPositive = await Application.Current!.MainPage!.DisplayAlert(
                "Motor Empire Autohaus-Provide a phone number",
                "Adding your phone number will help other users find and get in touch with you more easily.\nWe care for your privacy, so we assure you that your number will only be used within this application.\nDo you want to add your number? You can remove it anytime.",
                "Add my phone number",
                "I'll add it later on");

            if (!answerIsPositive) return;
            string phoneNumber = await Application.Current.MainPage.DisplayPromptAsync("Add a phone number",
                "Type your phone number here:\n", "OK", "Cancel", null, 15, Keyboard.Numeric,
                "+40 (Change the prefix according to your country)");

            if (!PhoneNumberValidator.IsPhoneNumberValid(phoneNumber))
            {
                CrossPlatformMessageRenderer.RenderMessages("The provided phone number is invalid!\nPlease try again!",
                    "Retry", 5);
                return;
            }

            if (!accountService.SetPhoneNumber(user.Uuid, phoneNumber)) return;
            AddPhoneNumberVisible = false;
            removePhoneNumberVisible = true;
            user.PhoneNumber = phoneNumber;
        }

        [RelayCommand]
        private async void RemovePhoneNumber()
        {
            bool answerIsPositive = await Application.Current!.MainPage!.DisplayAlert(
                "Motor Empire Autohaus-Remove the provided phone number",
                "Remove the phone number linked to your account?\nYou can add it back anytime.",
                "Yes, I want to remove my phone number associated with this acccount",
                "No");

            if (answerIsPositive)
            {
                if (accountService.RemovePhoneNumber(user.Uuid))
                {
                    AddPhoneNumberVisible = true;
                    removePhoneNumberVisible = false;
                    user.PhoneNumber = "";
                }
            }
        }

        [RelayCommand]
        private async void DeleteAccount()
        {
            bool answerIsPositive = await Application.Current!.MainPage!.DisplayAlert(
                "Motor Empire Autohaus-Delete account",
                "Are you sure that you want to delete your account?\n" +
                "This action is irreversible!\nYou won't be able to recover your data after this step and all of your posts and information shared within the app will be permanently erased.\n" +
                "Continue?",
                "Yes, delete my account",
                "No, I want to keep my account");

            if (answerIsPositive)
            {
                if (accountService.Delete(user))
                {
                    user = null;
                    await Shell.Current.GoToAsync("//SignUp", true);
                }
            }
        }
    }
}