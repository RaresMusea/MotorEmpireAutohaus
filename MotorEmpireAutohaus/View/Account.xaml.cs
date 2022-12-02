using MotorEmpireAutohaus.View_Model.Account;

namespace MotorEmpireAutohaus.View;

public partial class Account : ContentPage
{
    public Account(UserAccount usr)
    {
        BindingContext = usr;
        InitializeComponent();
    }
}