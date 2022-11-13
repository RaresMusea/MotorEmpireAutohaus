namespace MotorEmpireAutohaus;

public partial class LogIn : ContentPage
{
	public LogIn()
	{
		InitializeComponent();
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