using Tools.Utility.PlatformDependentStyling;
using UserAccountViewModel = MVVM.View_Models.Account.UserAccountViewModel;

namespace MVVM.View.Authentication;

public partial class LogIn : ContentPage, IPlatformDependentStyling
{
    protected override void OnAppearing()
    {
        AuthContainer.Opacity = 0;

        base.OnAppearing();

        if (this.AnimationIsRunning("TransitionAnimation"))
        {
            return;
        }

        Animation parentAnimation = new()
        {
            //{ 0, 0.5, new Animation(v => AuthenticationLogo.Opacity = v, 0, 1, Easing.CubicIn) },
            { 0.2, 0.8, new Animation(v => AuthContainer.Opacity = v, 0, 1, Easing.CubicIn) }
        };

        parentAnimation.Commit(this, "TransitionAnimation", 16, 2000);
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
            PasswordEntry.WidthRequest = 225;
            PasswordFrame.WidthRequest = 300;
            PasswordShowButton.Margin = new Thickness(-5);
        }
    }


    private void SetPasswordToVisible(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
    }


    private async void NavigateToSignUpPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//SignUp", true);
    }
}