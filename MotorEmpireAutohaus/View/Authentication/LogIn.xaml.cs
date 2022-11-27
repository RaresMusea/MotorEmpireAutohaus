
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using  Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics.Text;
using MotorEmpireAutohaus.Misc.Common;
using MotorEmpireAutohaus.Misc.Prebuilt_Components;
using MotorEmpireAutohaus.Storage;
using MotorEmpireAutohaus.View_Model.Account;
using MySqlConnector;
using System.Diagnostics;
using System.Threading;

namespace MotorEmpireAutohaus;

public partial class LogIn : ContentPage, IPlatformDependentStyling
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
            //{ 0, 0.5, new Animation(v => AuthenticationLogo.Opacity = v, 0, 1, Easing.CubicIn) },
			{0.2,0.8,new Animation(v=>AuthContainer.Opacity=v,0,1,Easing.CubicIn) }
        };

        parentAnimation.Commit(this, "TransitionAnimation", 16, 2000, null, null);
    }

	public LogIn(UserAccount userAccount)
	{

        BindingContext = userAccount;
        InitializeComponent();
        ApplySpecificStyleDependingOnPlatform();
		//Connect();
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
		await Shell.Current.GoToAsync("//SignUp",true);
	}

}
