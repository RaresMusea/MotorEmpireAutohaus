using MVVM.Models.User_Account_Model;
using MVVM.Services.Interfaces;
using MySqlConnector;
using UserAccountViewModel = MVVM.View_Models.Account.UserAccountViewModel;

namespace MVVM.View.Account_Management;

public partial class Account : ContentPage
{
    public Account(UserAccountViewModel usr)
    {
        if(Preferences.Default.Get("email","") != "")
        {
            usr.User.PhoneNumber = GetPhoneNumber();
            usr.User.EmailAddress = Preferences.Default.Get("email", "");
            if (string.IsNullOrEmpty(usr.User.PhoneNumber))
            {
                usr.RemovePhoneNumberVisible = false;
            }
        }

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

    private static string GetPhoneNumber()
    {
        string email = Preferences.Default.Get("email", "");
        MySqlCommand command = new($"SELECT PhoneNumber FROM user WHERE EmailAddress=@email",
               IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
        command.Prepare();
        command.Parameters.AddWithValue("@email", email);
        string phoneNumber = "";
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
             phoneNumber = reader.IsDBNull(0) ? "" : reader.GetString(0);
        }

        reader.Close();
        return phoneNumber;
    }

    private static UserAccount RetrieveRequiredAccountDetails()
    {
        string email = Preferences.Default.Get("email", "");
        MySqlCommand command = new($"SELECT UUID FROM user WHERE EmailAddress=@email",
               IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
        command.Prepare();
        command.Parameters.AddWithValue("@email", email);
        MySqlDataReader reader = command.ExecuteReader();

        string uuid = "";
        while (reader.Read())
        {
            uuid = reader.GetString(0);
        }


        reader.Close();

        MySqlCommand resCommand = new MySqlCommand($"SELECT * FROM user WHERE UUID=@id",
               IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
        resCommand.Prepare();
        resCommand.Parameters.AddWithValue("@id", uuid);

        MySqlDataReader resReader = resCommand.ExecuteReader();

        UserAccount returnValue = null;

        while (resReader.Read())
        {
            string unique = resReader.GetString(1);
            string name = resReader.GetString(2);
            string username = resReader.GetString(4);
            string password = resReader.GetString(5);
            string profileImage = resReader.GetString(6);
            string phoneNumber = resReader.IsDBNull(7) ? null : reader.GetString(7);
            returnValue = phoneNumber is not null
                ? new UserAccount(unique, name, email, username, password, profileImage, phoneNumber)
                : new UserAccount(unique, name, email, username, password, profileImage);
        }

        resReader.Close();
        return returnValue;
    }
}