using MotorEmpireAutohaus.Misc.Common;
using MotorEmpireAutohaus.Misc.Encryption;
using MotorEmpireAutohaus.Storage;
using MotorEmpireAutohaus.View_Model.Account;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.Services.Account_Services
{
    public class AccountService:IAuthenticate
    {
        private DatabaseConfigurer databaseConfig;
        private static string tableReference = "User";

        public AccountService()
        {
            Connect();
        }

        private void Connect()
        {
            databaseConfig = new DatabaseConfigurer();
            databaseConfig.OpenConnection();
        }

        public bool SignUp(UserAccount user)
        {
             Shell.Current.GoToAsync("//Feed",true);
            return true; 
        }

        public bool Login(UserAccount user)
        {
            string pass = Encrypter.EncryptPassword(user.Password);
            MySqlCommand command = new($"SELECT * FROM {tableReference} WHERE EmailAddress=@email and Password=@pass", databaseConfig.DatabaseConnection);
            command.Prepare();

            command.Parameters.AddWithValue("@email", user.EmailAddress);
            command.Parameters.AddWithValue("@pass", pass);

            MySqlDataReader reader= command.ExecuteReader();
            
            List<UserAccount> accounts=new();
            while(reader.Read())
            {
                if (reader != null)
                {
                    string uuid = (string)reader.GetValue(1);
                    string name = (string)reader.GetValue(2);
                    string emailAddress = (string)reader.GetValue(3);
                    string username = (string)reader.GetValue(4);
                    string password = (string)reader.GetValue(5);
                    string profileImageUrl = (string)reader.GetValue(7);
                    UserAccount result = new(uuid, name, emailAddress, username, password, profileImageUrl);
                    accounts.Add(result);
                }
            }

            if (accounts.Count == 0)
            {
                CrossPlatformMessageRenderer.RenderMessages("The provided credentials do not match any existing account on our platform! Please try again!","Retry",4);
                user.EmailAddress = "";
                user.Password = "";
                return false;
            }
            else
            {
                CrossPlatformMessageRenderer.DisplayMobileSnackbar($"Welcome back, {accounts.ElementAt(0).Name}! You are being redirected to the main application...","",5);
                return true;
            }

        }

        bool IAuthenticate.SignUp(UserAccount user)
        {
            throw new NotImplementedException();
        }
    }
}
