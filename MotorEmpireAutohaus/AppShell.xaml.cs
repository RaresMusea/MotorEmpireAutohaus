namespace MotorEmpireAutohaus;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
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
            {0,0.8,new Animation(v=>FlyoutImageBanner.Opacity=v,0,1,Easing.BounceIn)}
        };

        parentAnimation.Commit(this, "TransitionAnimation", 16, 2000, null, null);
    }

}
