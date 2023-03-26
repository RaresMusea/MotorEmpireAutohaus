// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using MVVM.Models.User_Account_Model;
using MVVM.Services.Account_Services;
using MVVM.Services.Interfaces;
using MySqlConnector;
using System.Diagnostics;
using MVVM.View.Landing;
using Tools.Utility.Messages;
using Windows.System;
using MVVM;
using Tools.Handlers;

namespace MotorEmpireAutohaus.WinUI;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
[QueryProperty(nameof(MVVM.Models.User_Account_Model.UserAccount), nameof(MVVM.Models.User_Account_Model.UserAccount))]
[QueryProperty (nameof(Name), nameof(Name))]
[ObservableObject]
public partial class App : MauiWinUIApplication
{
    [ObservableProperty] private UserAccount userAccount;
    [ObservableProperty] private string name;

	static Mutex mutex;
	/// <summary>
	/// Initializes the singleton application object.  This is the first line of authored code
	/// executed, and as such is the logical equivalent of main() or WinMain().
	/// </summary>
	public App()
	{
		this.InitializeComponent();
	}

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
		if (!IsSingleInstance())
		{
			Process.GetCurrentProcess().Kill();
		}
		else
		{
			base.OnLaunched(args);
			string emailAddress = Preferences.Default.Get("email", "");
			/*CrossPlatformMessageRenderer.RenderMessages(user, "ok", 3000);*/
			if(!string.IsNullOrEmpty(emailAddress))
			{
                UserPreferencesProvider.LoggedInAccount = GetUserBasedOnPreferences();
                Logger.CurrentlyLoggedInUuid = UserPreferencesProvider.LoggedInAccount.Uuid;
                await Shell.Current.GoToAsync($"//MotorEmpire?Name={UserPreferencesProvider.LoggedInAccount.Name}",
                    true);
            }
		}
    }

	static bool IsSingleInstance()
	{
		const string appID = "com.companyname.motorempireautohaus";
		mutex=new Mutex(false, appID);

		GC.KeepAlive(mutex);
		try
		{
			return mutex.WaitOne(0,false);
		}
		catch (AbandonedMutexException)
		{
			mutex.ReleaseMutex();
			return mutex.WaitOne(0, false);
		}

    }
	public UserAccount GetUserBasedOnPreferences()
	{
        IConnectableDataSource.Connect();
		string emailAddress = Preferences.Default.Get("email", "");
		const string tableReference = "User";

        MySqlCommand command = new($"SELECT UUID FROM {tableReference} WHERE EmailAddress=@email",
               IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
        command.Prepare();
        command.Parameters.AddWithValue("@email", emailAddress);
        MySqlDataReader reader = command.ExecuteReader();

        string uuid = "";
        while (reader.Read())
        {
            uuid = reader.GetString(0);
        }

        reader.Close();

        MySqlCommand retrievalCommand = new MySqlCommand($"SELECT * FROM {tableReference} WHERE UUID=@uuid",
              IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
        retrievalCommand.Prepare();
        retrievalCommand.Parameters.AddWithValue("@uuid", uuid);

        MySqlDataReader retrievalReader = retrievalCommand.ExecuteReader();

        UserAccount returnValue = null;

        while (retrievalReader.Read())
        {
            string unique = retrievalReader.GetString(1);
            string name = retrievalReader.GetString(2);
            string email = retrievalReader.GetString(3);
            string username = retrievalReader.GetString(4);
            string password = retrievalReader.GetString(5);
            string profileImage = retrievalReader.GetString(6);
            string phoneNumber = retrievalReader.IsDBNull(7) ? null : retrievalReader.GetString(7);
            returnValue = phoneNumber is not null
                ? new UserAccount(unique, name, email, username, password, profileImage, phoneNumber)
                : new UserAccount(unique, name, email, username, password, profileImage);
        }

        retrievalReader.Close();
        return returnValue;
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}

