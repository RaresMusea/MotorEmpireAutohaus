using MotorEmpireAutohaus.Services.Feed;
using MotorEmpireAutohaus.View;
using MotorEmpireAutohaus.View.App_Settings;
using MotorEmpireAutohaus.View.Core;
using MotorEmpireAutohaus.View_Model;
using MotorEmpireAutohaus.MVVM.View_Models.Account;

namespace MotorEmpireAutohaus;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        //Routing.RegisterRoute(nameof(MotorEmpire),typeof(MotorEmpire));
        Routing.RegisterRoute(nameof(Feed), typeof(Feed));
        Routing.RegisterRoute(nameof(About), typeof(About));
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (this.AnimationIsRunning("TransitionAnimation"))
        {
            return;
        }

        Animation parentAnimation = new()
        {
            //{0,0.8,new Animation(v=>FlyoutImageBanner.Opacity=v,0,1,Easing.BounceIn)}
        };

        parentAnimation.Commit(this, "TransitionAnimation", 16, 2000, null, null);
    }

    private async void SignOut(object o, EventArgs e)
    {
        bool answer = await DisplayAlert("Motor Empire Autohaus-SignOut", "Are you sure that you want to sign out?", "Yes", "No");
        if (answer)
        {
            await Current.GoToAsync("//LogIn", true);
        }
    }


}
