using MotorEmpireAutohaus.MVVM.View_Models.Account;

namespace MotorEmpireAutohaus.View;

public partial class Account : ContentPage
{
    public Account(UserAccountViewModel usr)
    {
        BindingContext = usr;
        InitializeComponent();
    }
}