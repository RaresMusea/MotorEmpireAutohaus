using MotorEmpireAutohaus.Misc.Prebuilt_Components;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.Storage
{
    public class DatabaseConfigurer : IDataSource
    {
        private readonly string _connectionString;

        private bool ConnectionOpen { get; set; }

        public MySqlConnection DatabaseConnection { get; set; }

        public DatabaseConfigurer()
        {
            ConnectionOpen = false;
            _connectionString = @"server=192.168.222.190;userid=root;password=password;database=motor_empire_autohaus_database";
            EstablishConnection();

        }

        private async void EstablishConnection()
        {
            try
            {
                DatabaseConnection = new MySqlConnection(_connectionString);
            }
            catch (MySqlException ex)
            {
                bool choice = await Application.Current.MainPage.DisplayAlert("Motor Empire Authohaus", "An error occured. Please try again later", "Error Details", "Ok");
                if (choice == true)
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
                    Debug.WriteLine("Connection successfull!");
                    await SnackbarComponent.GenerateSnackbar("Connection with the database established!",
                                                             "Close",
                                                              Color.FromArgb("#dbdbdbdb"),
                                                              Colors.Grey,
                                                              Colors.White,
                                                              Colors.Black,
                                                              Colors.Green,
                                                              Color.FromArgb("#333652"),
                                                              15,
                                                              14,
                                                              0,
                                                              5,
                                                              null);

                    ConnectionOpen = true;
                }
                catch (MySqlException mySqlEx)
                {


                    Action errorHandler = async ()=>
                    {
                       await DisplayErrorMessage("Motor Empire Autohaus-Internal Server Error", $"The application did not started correctly. In order to use the application properly, you need a connection with our server. Check your internet connection and restart the app, or try again.\n\n\nDetails:\n{mySqlEx.Message}", "Close app"); 
                       Application.Current.Quit();
                        

                    };

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
                                                             errorHandler);


                }
            }
        }

        public void CloseConnection ()
        {
            if (ConnectionOpen == false)
            {
                DatabaseConnection?.Close();
                ConnectionOpen = false;
            }
        }

        private async Task DisplayErrorMessage(string appTitle, string errorMsg, string optButton1)
        {
            await Application.Current.MainPage.DisplayAlert(appTitle, errorMsg, optButton1);
        }
    }
}
