using Tools.Utility.PlatformDependentStyling;
using System;
using UserAccountViewModel = MVVM.View_Models.Account.UserAccountViewModel;
using MVVM.Models.User_Account_Model;
using MVVM.Services.Interfaces;
using MySqlConnector;
using Tools.Handlers;

namespace MVVM.View.Authentication;

public partial class LogIn : ContentPage, IPlatformDependentStyling
{
    protected override async void OnAppearing()
    {
        AuthContainer.Opacity = 0;
        EmailEntry.Text = "";
        PasswordEntry.Text = "";
        base.OnAppearing();

        if (this.AnimationIsRunning("TransitionAnimation"))
        {
            return;
        }

        Animation parentAnimation = new()
        {
            { 0.2, 0.8, new Animation(v => AuthContainer.Opacity = v, 0, 1, Easing.CubicIn) }
        };

        parentAnimation.Commit(this, "TransitionAnimation", 16, 2000);

        if(DeviceInfo.Platform == DevicePlatform.Android)
        {
            string emailAddress = Preferences.Default.Get("email", "");
            /*CrossPlatformMessageRenderer.RenderMessages(user, "ok", 3000);*/
            if (!string.IsNullOrEmpty(emailAddress))
            {
                UserPreferencesProvider.LoggedInAccount = GetUserBasedOnPreferences();
                Logger.CurrentlyLoggedInUuid = UserPreferencesProvider.LoggedInAccount.Uuid;
                await Shell.Current.GoToAsync($"//MotorEmpire?Name={UserPreferencesProvider.LoggedInAccount.Name}",
                    true);
            }
        }

    }

    public LogIn(UserAccountViewModel userAccount)
    {
        BindingContext = userAccount;
        InitializeComponent();
        ApplySpecificStyleDependingOnPlatform();
    }


    public void ApplySpecificStyleDependingOnPlatform()
    {
        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            AuthenticationLogo.HeightRequest = 320;
            AuthenticationLogo.WidthRequest = 500;
            EmailEntry.HeightRequest = 25;
            PasswordEntry.HeightRequest = 25;
            EmailEntry.WidthRequest = 250;
            PasswordEntry.WidthRequest = 225;
            PasswordFrame.WidthRequest = 300;
            AuthContainer.Margin = new Thickness(0, 0, 0, 20);
            RememberMeLabel.FontSize = 16;
            ForgotPasswordLabel.FontSize = 16;
            WelcomeLabel.FontSize = 30;
            QuestionLabel.FontSize = 20;
            SignUpLabel.FontSize = 25;
        }

        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            EmailEntry.HeightRequest = 45;
            PasswordEntry.HeightRequest = 45;
            PasswordEntry.WidthRequest = 275;
            PasswordFrame.WidthRequest = 300;
            PasswordShowButton.Margin = new Thickness(5,0,0,0);
        }
    }


    private void SetPasswordToVisible(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
    }


    private async void NavigateToSignUpPage(object sender, EventArgs e)
    {
        BindingContext = null;
        await Shell.Current.GoToAsync("//SignUp", true);
    }

    private void RememberUserOnChecked(object sender, EventArgs e)
    {
        if (RememberMeCheckBox.IsChecked)
        {
            Preferences.Default.Set("email", EmailEntry.Text);
            return;
        }

        Preferences.Clear();

    }

    public UserAccount GetUserBasedOnPreferences()
    {
        IConnectableDataSource.Connect();
        string emailAddress = Preferences.Default.Get("email", "");
        const string tableReference = "User";

        MySqlCommand command = new($"SELECT UUID FROM {tableReference} WHERE EmailAddress=@email",
               IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
        command.Prepare();
        command.Parameters.AddWithValue("@email", emailAddress);
        MySqlDataReader reader = command.ExecuteReader();

        string uuid = "";
        while (reader.Read())
        {
            uuid = reader.GetString(0);
        }

        reader.Close();

        MySqlCommand retrievalCommand = new MySqlCommand($"SELECT * FROM {tableReference} WHERE UUID=@uuid",
              IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
        retrievalCommand.Prepare();
        retrievalCommand.Parameters.AddWithValue("@uuid", uuid);

        MySqlDataReader retrievalReader = retrievalCommand.ExecuteReader();

        UserAccount returnValue = null;

        while (retrievalReader.Read())
        {
            string unique = retrievalReader.GetString(1);
            string name = retrievalReader.GetString(2);
            string email = retrievalReader.GetString(3);
            string username = retrievalReader.GetString(4);
            string password = retrievalReader.GetString(5);
            string profileImage = retrievalReader.GetString(6);
            string phoneNumber = retrievalReader.IsDBNull(7) ? null : retrievalReader.GetString(7);
            returnValue = phoneNumber is not null
                ? new UserAccount(unique, name, email, username, password, profileImage, phoneNumber)
                : new UserAccount(unique, name, email, username, password, profileImage);
        }

        retrievalReader.Close();
        return returnValue;
    }
}