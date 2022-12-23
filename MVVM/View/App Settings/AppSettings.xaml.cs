using MotorEmpireAutohaus.MVVM.View_Model.App_Settings;
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
        image.Opacity = 0;
        mainFrame.Opacity = 0;

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