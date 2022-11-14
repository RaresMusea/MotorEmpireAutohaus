using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.Storage
{
    public class DatabaseConfigurer
    {
        private readonly string _connectionString;

        public bool ConnectionOpen { get;}
        
        public MySqlConnection? DatabaseConnection { get; set; }

        public  DatabaseConfigurer()
        {
            ConnectionOpen = false;
            _connectionString = @"server=localhost;userid=root;password=password;database=motor_empire_autohaus_database";
            EstablishConnection();
            
        }

        private async void EstablishConnection()
        {
            try
            {
                DatabaseConnection = new MySqlConnection(_connectionString);
            }
            catch (MySqlException mySqlEx)
            {
                await App.Current.MainPage.DisplayAlert("MotorEmpireAutohaus", $"ERROR!\n Could not establish connection with the database!\n Error details:{mySqlEx.Message}\n", "OK");
            }
        }
    }
}
