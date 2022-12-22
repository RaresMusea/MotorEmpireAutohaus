using Microsoft.VisualBasic;
using MotorEmpireAutohaus.MVVM.View_Models.Account;

namespace MotorEmpireAutohaus.View;

public partial class Account : ContentPage
{
    public Account(UserAccountViewModel usr)
    {
        BindingContext = usr;
        InitializeComponent();
    }
    
    private async void ToggleAccountDetails(object sender, EventArgs e)
    {
        if (showAccountDetailsFrame.IsVisible)
        {
            toggleAccountDetailsButton.Text = "Manage your account details";
            await showAccountDetailsFrame.FadeTo(0, 200, Easing.CubicOut);
            showAccountDetailsFrame.IsVisible= false;
            return;
        }

        toggleAccountDetailsButton.Text = "Hide";
        showAccountDetailsFrame.IsVisible = true;
        await showAccountDetailsFrame.FadeTo(1,250,Easing.CubicIn); 
    }

    private async void EditAccountButtonClicked(object sender, EventArgs e)
    {
        Entry[] entriesList = { editableNameEntry, editableUsernameEntry, editableEmailEntry };


        if(editAccountButton.Text=="Edit account information")
        {
            editAccountButton.Text = "Save changes";
            editableUsernameEntry.IsEnabled = true;
            editableNameEntry.IsEnabled = true;

            revertEditChanges.IsVisible = true;
            await revertEditChanges.FadeTo(1,180,Easing.CubicIn); 
            return;
        }
        editAccountButton.Text = "Edit account information";
        editableUsernameEntry.IsEnabled = false;
        editableNameEntry.IsEnabled = false;

        await revertEditChanges.FadeTo(0, 170, Easing.CubicInOut);
        revertEditChanges.IsVisible = false;
    }

}