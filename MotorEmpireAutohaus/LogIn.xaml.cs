using  Microsoft.Maui.Devices;
using MotorEmpireAutohaus.Misc.Common;

namespace MotorEmpireAutohaus;

public partial class LogIn : ContentPage,IPlatformDependentStyling
{
	public LogIn()
	{
		InitializeComponent();
		ApplySpecificStyleDependingOnPlatform();
	}


	public void ApplySpecificStyleDependingOnPlatform()
	{
		if (DeviceInfo.Platform == DevicePlatform.WinUI)
		{
			AuthenticationLogo.HeightRequest = 320;
			AuthenticationLogo.WidthRequest = 500;
			emailEntry.HeightRequest = 25;
			passwordEntry.HeightRequest = 25;
			emailEntry.WidthRequest = 250;
			passwordEntry.WidthRequest = 225;
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
			emailEntry.HeightRequest = 45;
			passwordEntry.HeightRequest = 45;
			passwordEntry.WidthRequest = 225;
			passwordFrame.WidthRequest = 300;
			passwordShowButton.Margin = new Thickness(-5);
        }
		
	}

	private void SetPasswordToVisible(object sender, EventArgs e)
	{
        passwordEntry.IsPassword=!passwordEntry.IsPassword;

    }

	private async void NavigateToSignUpPage(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//SignUp");
	}
}