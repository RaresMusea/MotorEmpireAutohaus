using MotorEmpireAutohaus.View.App_Settings;
using MVVM.View.Account_Management;
using MVVM.View.Favorite_Posts;
using MVVM.View.Landing;
using MVVM.View.Post_Feed;
using MVVM.View.Post_Info;
using MVVM.View.Post_Upload;

namespace MVVM;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(MotorEmpire), typeof(MotorEmpire));
        Routing.RegisterRoute(nameof(Account), typeof(Account));
        Routing.RegisterRoute(nameof(MotorEmpire), typeof(MotorEmpire));
        Routing.RegisterRoute(nameof(About), typeof(About));
        Routing.RegisterRoute(nameof(UploadPost), typeof(UploadPost));
        Routing.RegisterRoute(nameof(PostFeed), typeof(PostFeed));
        Routing.RegisterRoute(nameof(PostInfo), typeof(PostInfo));
        Routing.RegisterRoute(nameof(FavoritePosts), typeof(FavoritePosts));
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

        parentAnimation.Commit(this, "TransitionAnimation", 16, 2000);
    }

    private async void SignOut(object o, EventArgs e)
    {
        bool answer = await DisplayAlert("Motor Empire Autohaus-SignOut", "Are you sure that you want to sign out?",
            "Yes", "No");
        if (answer)
        {
            await Current.GoToAsync("//LogIn", true);
        }
    }
}