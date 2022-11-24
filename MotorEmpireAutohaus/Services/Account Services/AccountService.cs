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
        private static string tableReference = "user";

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

        public void Login(UserAccount user)
        {
            user.Password = Encrypter.EncryptPassword(user.Password);
            MySqlCommand command = new($"SELECT * FROM {tableReference} WHERE Email=@email and Password=@pass", databaseConfig.DatabaseConnection);
            command.Prepare();

            command.Parameters.AddWithValue("@email", user.EmailAddress);
            command.Parameters.AddWithValue("@pass", user.Password);

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
                    UserAccount result = new UserAccount(uuid, name, emailAddress, username, password, profileImageUrl);
                    accounts.Add(result);
                }
            }

            if (accounts.Count == 0)
            {

            }


        }

        bool IAuthenticate.Login(UserAccount user)
        {
            throw new NotImplementedException();
        }

        bool IAuthenticate.SignUp(UserAccount user)
        {
            throw new NotImplementedException();
        }
    }
}
