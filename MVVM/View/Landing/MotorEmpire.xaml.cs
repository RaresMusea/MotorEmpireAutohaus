using Tools.Utility.Messages;
using MotorEmpireViewModel = MVVM.View_Models.Core.MotorEmpireViewModel;

namespace MVVM.View.Landing;

public partial class MotorEmpire : ContentPage
{
    public MotorEmpire(MotorEmpireViewModel motorEmpireViewModel)
    {
        BindingContext = motorEmpireViewModel;
        InitializeComponent();
        StyleDependingOnOS();
    }

    protected override void OnAppearing()
    {
        SearchByCategory.Opacity = 0;
        SecondaryFrame.Opacity = 0;
        TitleText.Opacity = 0;
        MobileView.Opacity = 0;
        JumbotronImageAnimation.Opacity = 0;
        base.OnAppearing();
        if (this.AnimationIsRunning("TransitionAnimation"))
        {
            return;
        }

        if (!DesktopView.IsVisible)
        {
            JumbotronImageAnimation.IsAnimationPlaying = true;
            Animation parentAnimation = new()
            {
                { 0, 0.3, new Animation(v => JumbotronImageAnimation.Opacity = v, 0, 1, Easing.CubicIn) },
                { 0.3, 0.5, new Animation(v => MobileView.Opacity = v, 0, 1, Easing.CubicIn) },
                { 0.5, 1, new Animation(v => SearchByCategory.Opacity = v, 0, 1, Easing.CubicIn) }
            };

            parentAnimation.Commit(this, "TransitionAnimation", 16, 2000);
        }
        else
        {
            JumbotronImage.Opacity = 0;
            Animation parentAnimation = new()
            {
                { 0, 0.4, new Animation(v => JumbotronImage.Opacity = v, 0, 1, Easing.CubicIn) },
                // {0.2,0.5,new Animation(v=>desktopGreeting.Opacity=v,0,1,Easing.CubicIn)},
                { 0.1, 0.3, new Animation(v => TitleText.Opacity = v, 0, 1, Easing.CubicIn) },
                { 0.3, 0.6, new Animation(v => SearchFrame.Opacity = v, 0, 1, Easing.CubicIn) },
                { 0.2, 0.8, new Animation(v => PrimaryFrame.Opacity = v, 0, 1, Easing.CubicIn) },
                { 0.4, 1, new Animation(v => SecondaryFrame.Opacity = v, 0, 1, Easing.CubicIn) }
            };
            parentAnimation.Commit(this, "TransitionAnimation", 16, 2000);
        }
    }

    private void StyleDependingOnOS()
    {
        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            SecondaryFrame.WidthRequest = 600;

            CarFilterRow1.Orientation = StackOrientation.Horizontal;
            CarFilterRow2.Orientation = StackOrientation.Horizontal;
            CarTypePicker.WidthRequest = 200;
            ManufacturerPicker.WidthRequest = 200;
            ModelPicker.WidthRequest = 200;
            GenerationPicker.WidthRequest = 200;
            LowerPricePicker.WidthRequest = 100;
            UpperPricePicker.WidthRequest = 100;
            UpperYear.WidthRequest = 100;
            LowerYear.WidthRequest = 100;
            UpperMileagePicker.WidthRequest = 130;
            LowerMileagePicker.WidthRequest = 130;
            FuelTypePicker.WidthRequest = 200;
            MobileView.IsVisible = false;
            DesktopView.IsVisible = true;
        }

        if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
        {
            CarFilterRow1.Orientation = StackOrientation.Vertical;
            CarFilterRow2.Orientation = StackOrientation.Vertical;
            VerticalStack1.Margin = new Thickness(0, 0, 0, 15);
            VerticalStack2.Margin = new Thickness(0, 0, 0, 15);
            UpperPricePicker.Margin = new Thickness(0, 10, 15, 0);
            FuelTypeLabel.Margin = new Thickness(0, 0, 190, 0);
            FuelTypePicker.Margin = new Thickness(0, 10, 15, 0);
            FuelTypePicker.WidthRequest = 115;
            LowerMileagePicker.WidthRequest = 85;
            UpperMileagePicker.WidthRequest = 85;
            LowerPricePicker.WidthRequest = 70;
            UpperPricePicker.WidthRequest = 70;
            LowerYear.WidthRequest = 70;
            UpperYear.WidthRequest = 70;
            SecondaryFrame.WidthRequest = 420;
        }
    }

    private void DisplayCarFilters(object o, EventArgs e)
    {
        CarFiltersForm.Opacity = 0;
        //{AppThemeBinding Dark=#313131, Light=Grey}
        if (Application.Current!.RequestedTheme == AppTheme.Dark)
        {
            ToggleCarFilters.BackgroundColor = Color.FromArgb("#313131");
        }

        if (Application.Current.RequestedTheme == AppTheme.Light)
        {
            ToggleCarFilters.BackgroundColor = Colors.Grey;
        }

        CarFiltersForm.IsVisible = true;
        CarFiltersForm.FadeTo(1, 200, Easing.CubicIn);
    }

    private async void OnTextChangedEvent(object o, EventArgs e)
    {
        if (SearchBarEntry.Text.Length == 0)
        {
            await SearchButton.FadeTo(0, 150, Easing.CubicIn);
            SearchButton.IsVisible = false;
            await SearchByCategory.FadeTo(1, 150, Easing.CubicIn);
            SearchByCategory.IsVisible = true;
        }
        else
        {
            if (PrimaryFrame.IsVisible)
            {
                await PrimaryFrame.FadeTo(0, 150, Easing.CubicInOut);
                PrimaryFrame.IsVisible = false;
            }

            await SearchByCategory.FadeTo(0, 150, Easing.CubicIn);
            SearchByCategory.IsVisible = false;
            SearchButton.IsVisible = true;
            await SearchButton.FadeTo(1, 150, Easing.CubicIn);
        }
    }

    private async void ShowCategories(object o, EventArgs e)
    {
        SearchBar.IsEnabled = false;
        SearchButtonImage.IsEnabled = false;
        CrossPlatformMessageRenderer.RenderMessages("The search bar is now disabled because you activated the" +
                     " filter-based search. Close the filters in order to use the searchbar again", "OK", 4);

        PrimaryFrame.IsVisible = true;
        await PrimaryFrame.FadeTo(1, 170, Easing.BounceIn);
    }

    private async void CloseCarFilters(object o, EventArgs e)
    {
        ToggleCarFilters.BackgroundColor = Colors.Transparent;
        await CarFiltersForm.FadeTo(0, 240, Easing.CubicIn);
        CarFiltersForm.IsVisible = false;
        await Task.Delay(600);
        await PrimaryFrame.FadeTo(0, 250, Easing.CubicIn);
        PrimaryFrame.IsVisible = false;
        
        SearchBar.IsEnabled = true;
        SearchButtonImage.IsEnabled=true;
    }

}