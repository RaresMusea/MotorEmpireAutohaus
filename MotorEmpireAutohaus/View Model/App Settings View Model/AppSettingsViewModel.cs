using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MotorEmpireAutohaus.View.App_Settings;
using MotorEmpireAutohaus.View_Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.View_Model.App_Settings_View_Model
{
    public partial class AppSettingsViewModel:BaseViewModel
    {
        [ObservableProperty]
        private bool isDarkThemeEnabled= (Application.Current.UserAppTheme == AppTheme.Dark);

        public AppSettingsViewModel(){}

        [RelayCommand]
        public async void GoToAboutPage()
        {
            await Shell.Current.GoToAsync(nameof(About), true);
        }
    
    }
}
