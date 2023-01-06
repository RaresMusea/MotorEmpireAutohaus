using AppSettingsViewModel = MVVM.View_Models.App_Settings.AppSettingsViewModel;

namespace MVVM.View.App_Settings;

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
        Image.Opacity = 0;
        MainFrame.Opacity = 0;

        if (this.AnimationIsRunning("TransitionAnimation"))
        {
            return;
        }

        Animation openingAnimation = new()
        {
            { 0, 0.4, new Animation(v => Image.Opacity = v, 0, 1, Easing.CubicIn) },
            { 0.4, 1, new Animation(v => MainFrame.Opacity = v, 0, 1, Easing.CubicIn) }
        };

        openingAnimation.Commit(this, "TransitionAnimation", 16, 2000);
    }


    private void OnSwitchChanged(object o, EventArgs e)
    {
        if (DarkThemeSwitch.IsToggled)
        {
            Application.Current!.UserAppTheme = AppTheme.Dark;
        }
        else
        {
            Application.Current!.UserAppTheme = AppTheme.Light;
        }
    }
}