using System.Diagnostics;
using MySqlConnector;
using Tools.Prebuilt_Components;

namespace Storage.MySQL
{
    public class DatabaseConfigurer : IDataSource
    {
        private readonly string connectionString;

        private bool ConnectionOpen { get; set; }

        public MySqlConnection DatabaseConnection { get; private set; }

        public DatabaseConfigurer()
        {
            ConnectionOpen = false;
            connectionString =
                @"server=192.168.100.34;userid=root;password=password;database=motor_empire_autohaus_database;SSL Mode=None;";
            EstablishConnection();
        }

        private async void EstablishConnection()
        {
            try
            {
                DatabaseConnection = new MySqlConnection(connectionString);
            }
            catch (MySqlException ex)
            {
                bool choice = await Application.Current!.MainPage!.DisplayAlert("Motor Empire Authohaus",
                    "An error occured. Please try again later", "Error Details", "Ok");
                if (choice)
                {
                    await Application.Current.MainPage.DisplayAlert("Motor Empire Autohaus", $"{ex.Message}", "Ok");
                }
            }
        }

        public async void OpenConnection()
        {
            if (!ConnectionOpen)
            {
                try
                {
                    DatabaseConnection?.Open();
                    /* Debug.WriteLine("Connection successful!");
                     await SnackbarComponent.GenerateSnackbar("Connection successful!",
                         "Close",
                         Color.FromArgb("#dbdbdb"),
                         Color.FromArgb("#414141"),
                         Colors.Grey,
                         Colors.White,
                         Colors.Green,
                         Color.FromArgb("#252525"),
                         15,
                         14,
                         0,
                         4,
                         null);*/
                    ConnectionOpen = true;
                }
                catch (MySqlException mySqlEx)
                {
                    async void ErrorHandler()
                    {
                        await DisplayErrorMessage("Motor Empire Autohaus-Internal Server Error",
                            $"The application did not started correctly. In order to use the application properly, you need a connection with our server. Check your internet connection and restart the app, or try again.\n\n\nDetails:\n{mySqlEx.Message}",
                            "Close app");
                        Application.Current!.Quit();
                    }


                    Debug.WriteLine($"Unable to establish connection with the database!\n {mySqlEx.Message}");
                    await SnackbarComponent.GenerateSnackbar("Error! Cannot establish a connection with the database!",
                        "More details",
                        Color.FromArgb("#DC3535"),
                        Color.FromArgb("#DC3535"),
                        Colors.Black,
                        Colors.White,
                        Colors.Black,
                        Colors.Black,
                        14,
                        14,
                        0,
                        5,
                        ErrorHandler);
                }
            }
        }

        public void CloseConnection()
        {
            if (ConnectionOpen == false)
            {
                DatabaseConnection?.Close();
                ConnectionOpen = false;
            }
        }

        private static async Task DisplayErrorMessage(string appTitle, string errorMsg, string optButton1)
        {
            await Application.Current!.MainPage!.DisplayAlert(appTitle, errorMsg, optButton1);
        }
    }
}