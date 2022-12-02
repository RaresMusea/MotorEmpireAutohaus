using MotorEmpireAutohaus.View_Model;

namespace MotorEmpireAutohaus.View;

public partial class MotorEmpire: ContentPage
{
    public MotorEmpire(MotorEmpireViewModel motorEmpireViewModel)
    {
        BindingContext = motorEmpireViewModel;
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
            jumbotronImageAnimation.IsAnimationPlaying=true;
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
               // {0.2,0.5,new Animation(v=>desktopGreeting.Opacity=v,0,1,Easing.CubicIn)},
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

            carFilterRow1.Orientation = StackOrientation.Horizontal;
            carFilterRow2.Orientation = StackOrientation.Horizontal;
            carTypePicker.WidthRequest = 200;
            manufacturerPicker.WidthRequest = 200;
            modelPicker.WidthRequest = 200;
            generationPicker.WidthRequest = 200;
            lowerPricePicker.WidthRequest = 100;
            upperPricePicker.WidthRequest = 100;
            upperYear.WidthRequest = 100;
            lowerYear.WidthRequest = 100;
            upperMileagePicker.WidthRequest = 130;
            lowerMileagePicker.WidthRequest = 130;
            fuelTypePicker.WidthRequest = 200;
            mobileView.IsVisible = false;
            DesktopView.IsVisible = true;
        }

        if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform==DevicePlatform.iOS)
        {
            carFilterRow1.Orientation = StackOrientation.Vertical;
            carFilterRow2.Orientation= StackOrientation.Vertical;
            verticalStack1.Margin = new Thickness(0,0,0,15);
            verticalStack2.Margin = new Thickness(0, 0, 0, 15);
            upperPricePicker.Margin = new Thickness(0, 10, 30, 0);
            fuelTypeLabel.Margin = new Thickness(0, 0, 200, 0);
            fuelTypePicker.Margin = new Thickness(0, 10, 50, 0);
            fuelTypePicker.WidthRequest = 110;
            lowerMileagePicker.WidthRequest = 80;
            upperMileagePicker.WidthRequest = 80;
            lowerPricePicker.WidthRequest = 70;
            upperPricePicker.WidthRequest= 70;
            lowerYear.WidthRequest = 70;
            upperYear.WidthRequest = 70;
            secondaryFrame.WidthRequest = 400;
        }
    }

    private void DisplayCarFilters(object o, EventArgs e)
    {
        carFiltersForm.Opacity = 0;
        //{AppThemeBinding Dark=#313131, Light=Grey}
        if (Application.Current.RequestedTheme == AppTheme.Dark)
        {
            toggleCarFilters.BackgroundColor = Color.FromArgb("#313131");

        }
        if (Application.Current.RequestedTheme == AppTheme.Light)
        {
            toggleCarFilters.BackgroundColor = Colors.Grey;
        }

        carFiltersForm.IsVisible = true;
        carFiltersForm.FadeTo(1, 200, Easing.CubicIn);
        
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

    private void EnableModelFiltering(object o, EventArgs e)
    {
        modelPicker.IsEnabled= true;        
    }

    private async void CloseCarFilters(object o, EventArgs e)
    {

        await carFiltersForm.FadeTo(0, 150, Easing.CubicIn);
        carFiltersForm.IsVisible = false;
        await Task.Delay(1000);
        await primaryFrame.FadeTo(0, 150, Easing.CubicIn);
        primaryFrame.IsVisible = false;
    }
}