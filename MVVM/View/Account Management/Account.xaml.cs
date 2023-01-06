using UserAccountViewModel = MVVM.View_Models.Account.UserAccountViewModel;

namespace MVVM.View.Account_Management;

public partial class Account : ContentPage
{
    public Account(UserAccountViewModel usr)
    {
        BindingContext = usr;
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        MainFrame.Opacity = 0;
        MainStack.Opacity = 0;

        base.OnAppearing();
        if (this.AnimationIsRunning("AppearingAnimation"))
        {
            return;
        }

        Animation appearingAnimation = new Animation()
        {
            { 0, 0.4, new Animation(v => MainFrame.Opacity = v, 0, 1, Easing.CubicIn) },
            { 0.4, 0.8, new Animation(v => MainStack.Opacity = v, 0, 1, Easing.CubicIn) },
        };

        appearingAnimation.Commit(this, "TransitionAnimation", 16, 2000);
    }

    private async void ToggleAccountDetails(object sender, EventArgs e)
    {
        HorizontalStackLayout[] horizontalStackLayouts = { EditStack1, EditStack2, EditStack3, PhoneNumberFrame };
        List<HorizontalStackLayout> editLayouts = horizontalStackLayouts.ToList();

        if (ShowAccountDetailsFrame.IsVisible)
        {
            ToggleAccountDetailsButton.Text = "Manage your account details";
            editLayouts.Reverse();
            editLayouts.ToList().ForEach(async elem => await elem.FadeTo(0, 200, Easing.CubicOut));

            await ShowAccountDetailsFrame.FadeTo(0, 200, Easing.CubicOut);
            ShowAccountDetailsFrame.IsVisible = false;
            return;
        }

        ToggleAccountDetailsButton.Text = "Hide";
        ShowAccountDetailsFrame.IsVisible = true;
        await ShowAccountDetailsFrame.FadeTo(1, 250, Easing.CubicIn);
        editLayouts.ForEach(async elem => { await elem.FadeTo(1, 200, Easing.CubicIn); });
    }

    private async void EditAccountButtonClicked(object sender, EventArgs e)
    {
        if (EditAccountButton.Text == "Edit account information")
        {
            EditAccountButton.Text = "Save changes";
            EditableUsernameEntry.IsEnabled = true;
            EditableNameEntry.IsEnabled = true;
            EditablePhoneNumberEntry.IsEnabled = true;

            RevertEditChanges.IsVisible = true;
            await RevertEditChanges.FadeTo(1, 180, Easing.CubicIn);
            return;
        }

        EditAccountButton.Text = "Edit account information";
        EditableUsernameEntry.IsEnabled = false;
        EditableNameEntry.IsEnabled = false;
        EditablePhoneNumberEntry.IsEnabled = false;

        await RevertEditChanges.FadeTo(0, 170, Easing.CubicInOut);
        RevertEditChanges.IsVisible = false;
    }
}