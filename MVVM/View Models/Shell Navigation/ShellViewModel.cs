using CommunityToolkit.Mvvm.Input;
using MotorEmpireAutohaus.MVVM.Models.User_Account_Model;
using MotorEmpireAutohaus.MVVM.View_Models.Core;
using MotorEmpireAutohaus.MVVM.View_Models.Base;
using CommunityToolkit.Mvvm.ComponentModel;


namespace MotorEmpireAutohaus.View_Model.Shell_Navigation
{
    public partial class ShellViewModel:BaseViewModel
    {

        [ObservableProperty]
        MotorEmpireViewModel userAccount;
        
        public ShellViewModel(MotorEmpireViewModel vm)
        {
            userAccount = vm;
        }


        [RelayCommand]
        public async void GoToAccountSettings()
        {
            await Shell.Current.GoToAsync("//Account");
        }
    }
}
