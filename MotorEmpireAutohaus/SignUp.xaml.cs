namespace MotorEmpireAutohaus;

public partial class SignUp : ContentPage
{
    public SignUp()
    {
        InitializeComponent();
    }

    private void SetPasswordToVisible(object sender, EventArgs e)
    {
        passwordEntry.IsPassword = !passwordEntry.IsPassword;

    }

    private async void NavigateToSignInPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LogIn");
    }
}