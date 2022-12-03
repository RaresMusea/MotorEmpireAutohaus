using MotorEmpireAutohaus.Misc.Common;
using MotorEmpireAutohaus.View_Model.Account;

namespace MotorEmpireAutohaus;

public partial class SignUp : ContentPage, IPlatformDependentStyling
{

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (this.AnimationIsRunning("TransitionAnimation"))
        {
            return;
        }

        Animation parentAnimation = new()
        {
            {0.2,0.8,new Animation(v=>AuthContainer.Opacity=v,0,1,Easing.CubicIn) }
        };

        parentAnimation.Commit(this, "TransitionAnimation", 16, 2000, null, null);
    }

    public SignUp(UserAccountViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
        //Ensure the data binding in the constructor in order to aceess the view model properties directly from the XAML markup page.
        ApplySpecificStyleDependingOnPlatform();
    }

    private void SetPasswordToVisible(object sender, EventArgs e)
    {
        passwordEntry.IsPassword = !passwordEntry.IsPassword;
        passwordConfirmationEntry.IsPassword = passwordEntry.IsPassword;
    }

    public void ApplySpecificStyleDependingOnPlatform()
    {
        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            AuthenticationLogo.HeightRequest = 320;
            AuthenticationLogo.WidthRequest = 500;
            NameEntry.HeightRequest = 25;
            usernameEntry.HeightRequest = 25;
            emailEntry.HeightRequest = 25;
            passwordEntry.HeightRequest = 25;
            passwordConfirmationEntry.HeightRequest = 25;
            NameEntry.WidthRequest = 250;
            usernameEntry.WidthRequest = 250;
            emailEntry.WidthRequest = 250;
            passwordEntry.WidthRequest = 225;
            passwordConfirmationEntry.WidthRequest = 300;
            passwordFrame.WidthRequest = 300;
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
            usernameEntry.HeightRequest = 45;
            emailEntry.HeightRequest = 45;
            passwordEntry.HeightRequest = 45;
            passwordEntry.WidthRequest = 225;
            passwordFrame.WidthRequest = 300;
            passwordShowButton.Margin = new Thickness(-5);
        }
    }

    private async void NavigateToSignInPage(object sender, EventArgs e)
    {
        await AuthContainer.FadeTo(0);
        await Shell.Current.GoToAsync("//LogIn",true);
    }
}