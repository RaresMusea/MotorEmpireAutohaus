using MotorEmpireAutohaus.View_Model.Base;
using CommunityToolkit;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MotorEmpireAutohaus.View_Model.Account;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MotorEmpireAutohaus.View_Model.Shell_Navigation
{
    [QueryProperty (nameof(UserAccount),nameof(UserAccount))]
    public partial class ShellViewModel:BaseViewModel
    {

        [ObservableProperty]
        MotorEmpireViewModel userAccount;
        
        public ShellViewModel(MotorEmpireViewModel vm)
        {
            this.userAccount = vm;
        }


        [RelayCommand]
        public async void GoToAccountSettings()
        {
            await Shell.Current.GoToAsync("//Account");
        }
    }
}
