using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BaseViewModel = MVVM.View_Models.Base.BaseViewModel;
using MotorEmpireViewModel = MVVM.View_Models.Core.MotorEmpireViewModel;


namespace MVVM.View_Models.Shell_Navigation
{
    public partial class ShellViewModel : BaseViewModel
    {
        [ObservableProperty] private MotorEmpireViewModel userAccount;

        public ShellViewModel(MotorEmpireViewModel vm)
        {
            userAccount = vm;
        }

        [RelayCommand]
        private async void GoToAccountSettings()
        {
            await Shell.Current.GoToAsync("//Account");
        }
    }
}