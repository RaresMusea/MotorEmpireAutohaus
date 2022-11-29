using MotorEmpireAutohaus.View_Model;

namespace MotorEmpireAutohaus.View;

public partial class MotorEmpire: ContentPage
{
    public MotorEmpire()
    {
        InitializeComponent();
        StyleDependingOnOS();
    }

    protected override void OnAppearing()
    {
        searchByCategory.Opacity = 0;
        secondaryFrame.Opacity = 0;
        titleText.Opacity = 0;
        mobileView.Opacity = 0;
        jumbotronImageAnimation.Opacity = 0;
        base.OnAppearing();
        if (this.AnimationIsRunning("TransitionAnimation"))
        {
            return;
        }

        if (!DesktopView.IsVisible)
        {
            jumbotronImageAnimation.IsAnimationPlaying = false;
            Animation parentAnimation = new()
            {
            {0,0.3,new Animation(v=>jumbotronImageAnimation.Opacity=v,0,1,Easing.CubicIn) },
            {0.3,0.5,new Animation(v=>mobileView.Opacity=v,0,1,Easing.CubicIn)},
            {0.5,1, new Animation(v=>searchByCategory.Opacity=v,0,1,Easing.CubicIn)}
            };

            parentAnimation.Commit(this, "TransitionAnimation", 16, 2000, null, null);
        }
        else
        {
            jumbotronImage.Opacity = 0;
            Animation parentAnimation = new()
            {
                {0,0.4, new Animation(v=>jumbotronImage.Opacity=v,0,1,Easing.CubicIn)},
                {0.1,0.3, new Animation (v=>titleText.Opacity=v,0,1,Easing.CubicIn) },
                {0.3,0.6, new Animation (v=>SearchFrame.Opacity=v,0,1,Easing.CubicIn) },
                {0.2,0.8, new Animation (v=>primaryFrame.Opacity=v,0,1,Easing.CubicIn)},
                {0.4,1,new Animation(v=>secondaryFrame.Opacity=v,0,1,Easing.CubicIn)}
            };
            parentAnimation.Commit(this,"TransitionAnimation",16,2000);
        }
    }

    private void StyleDependingOnOS()
    {
        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            secondaryFrame.WidthRequest = 600;
            mobileView.IsVisible = false;
            DesktopView.IsVisible = true;
        }

        if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform==DevicePlatform.Android)
        {
            secondaryFrame.WidthRequest = 400;
        }
    }

    private void DisplayCarFilters(object o, EventArgs e)
    {
        CarFilters.IsVisible = true;
    }

    private async void OnTextChangedEvent(object o, EventArgs e)
    {
        if (searchBarEntry.Text.Length == 0)
        {
            await searchButton.FadeTo(0, 150, Easing.CubicIn);
            searchButton.IsVisible = false;
            await searchByCategory.FadeTo(1,150,Easing.CubicIn);
            searchByCategory.IsVisible = true;
        }
        else
        {
            if (primaryFrame.IsVisible)
            {
                await primaryFrame.FadeTo(0, 150, Easing.CubicInOut);
                primaryFrame.IsVisible = false;
            }

            await searchByCategory.FadeTo(0, 150, Easing.CubicIn);
            searchByCategory.IsVisible = false;
            searchButton.IsVisible = true;
            await searchButton.FadeTo(1, 150, Easing.CubicIn);
        }
    }

    private async void ShowCategories(object o, EventArgs e)
    {
        primaryFrame.IsVisible = true;
        await primaryFrame.FadeTo(1, 170, Easing.BounceIn);
    }

}