using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MotorEmpireAutohaus.MVVM.View_Models.Base;

namespace MotorEmpireAutohaus.MVVM.View_Model.App_Settings;

public partial class AppSettingsViewModel:BaseViewModel
{
    [ObservableProperty]
    private bool isDarkThemeEnabled=true;

    public AppSettingsViewModel()
    {
        isDarkThemeEnabled = Application.Current.RequestedTheme == AppTheme.Dark;
    }

    [RelayCommand]
    public async void GoToAboutPage()
    {
        await Shell.Current.GoToAsync("//About", true);
    }

}
