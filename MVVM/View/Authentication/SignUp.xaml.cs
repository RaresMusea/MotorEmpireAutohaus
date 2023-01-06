using Tools.Utility.PlatformDependentStyling;
using UserAccountViewModel = MVVM.View_Models.Account.UserAccountViewModel;

namespace MVVM.View.Authentication;

public partial class SignUp : ContentPage, IPlatformDependentStyling
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
            { 0.2, 0.8, new Animation(v => AuthContainer.Opacity = v, 0, 1, Easing.CubicIn) }
        };

        parentAnimation.Commit(this, "TransitionAnimation", 16, 2000);
    }

    public SignUp(UserAccountViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
        //Ensure the data binding in the constructor in order to access the view model properties directly from the XAML markup page.
        ApplySpecificStyleDependingOnPlatform();
    }

    private void SetPasswordToVisible(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        PasswordConfirmationEntry.IsPassword = PasswordEntry.IsPassword;
    }

    public void ApplySpecificStyleDependingOnPlatform()
    {
        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            AuthenticationLogo.HeightRequest = 320;
            AuthenticationLogo.WidthRequest = 500;
            NameEntry.HeightRequest = 25;
            UsernameEntry.HeightRequest = 25;
            EmailEntry.HeightRequest = 25;
            PasswordEntry.HeightRequest = 25;
            PasswordConfirmationEntry.HeightRequest = 25;
            NameEntry.WidthRequest = 250;
            UsernameEntry.WidthRequest = 250;
            EmailEntry.WidthRequest = 250;
            PasswordEntry.WidthRequest = 225;
            PasswordConfirmationEntry.WidthRequest = 300;
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
            NameEntry.HeightRequest = 45;
            UsernameEntry.HeightRequest = 45;
            EmailEntry.HeightRequest = 45;
            PasswordEntry.HeightRequest = 45;
            PasswordEntry.WidthRequest = 225;
            PasswordFrame.WidthRequest = 300;
            PasswordShowButton.Margin = new Thickness(-5);
        }
    }

    private async void NavigateToSignInPage(object sender, EventArgs e)
    {
        await AuthContainer.FadeTo(0);
        await Shell.Current.GoToAsync("//LogIn", true);
    }
}