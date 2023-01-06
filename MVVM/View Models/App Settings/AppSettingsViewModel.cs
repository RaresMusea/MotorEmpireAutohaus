using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVM.View.App_Settings;
using BaseViewModel = MVVM.View_Models.Base.BaseViewModel;

namespace MVVM.View_Models.App_Settings;

public partial class AppSettingsViewModel : BaseViewModel
{
    [ObservableProperty] private bool isDarkThemeEnabled;

    public AppSettingsViewModel()
    {
        isDarkThemeEnabled = Application.Current!.RequestedTheme == AppTheme.Dark;
    }

    [RelayCommand]
    private async void GoToAboutPage()
    {
        await Shell.Current.GoToAsync(nameof(About), true);
    }
}