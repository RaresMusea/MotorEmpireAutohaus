using MotorEmpireAutohaus.View_Model.App_Settings_View_Model;

namespace MotorEmpireAutohaus.View.App_Settings;

public partial class AppSettings : ContentPage
{
	public AppSettings(AppSettingsViewModel appSettingsViewModel)
	{
		InitializeComponent();
        BindingContext = appSettingsViewModel;
    }

    protected override void OnAppearing()
    {
        GenerateOpeningAnimation();
    }

    private void GenerateOpeningAnimation()
    {
        if (this.AnimationIsRunning("TransitionAnimation"))
        {
            return;
        }

        Animation openingAnimation = new()
            {
                {0,0.4, new Animation(v=>image.Opacity=v,0,1,Easing.CubicIn)},
                {0.4,1, new Animation(v=>mainFrame.Opacity=v,0,1,Easing.CubicIn)}
            };

        openingAnimation.Commit(this, "TransitionAnimation", 16, 2000);

    }


    private void OnSwitchChanged(object o, EventArgs e)
	{
		if (darkThemeSwitch.IsToggled)
		{
			Application.Current.UserAppTheme = AppTheme.Dark;
        }
		else
		{
			Application.Current.UserAppTheme=AppTheme.Light;
		}

    }
}