using MotorEmpireAutohaus.Misc.Common;
using MotorEmpireAutohaus.Misc.Encryption;
using MotorEmpireAutohaus.Misc.Exceptions;
using MotorEmpireAutohaus.Storage;
using MotorEmpireAutohaus.Storage.Firebase_Storage;
using MotorEmpireAutohaus.View_Model.Account;
using MotorEmpireAutohaus.View_Model.Base;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.Services.Account_Services
{
    public class AccountService : IAuthenticate, IStorable
    {
        private DatabaseConfigurer databaseConfig;
        private static readonly string tableReference = "User";

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
            Shell.Current.GoToAsync("//Feed", true);
            return true;
        }

        public bool Login(UserAccount user)
        {
            string pass = Encrypter.EncryptPassword(user.Password);
            MySqlCommand command = new($"SELECT * FROM {tableReference} WHERE EmailAddress=@email and Password=@pass", databaseConfig.DatabaseConnection);
            command.Prepare();

            command.Parameters.AddWithValue("@email", user.EmailAddress);
            command.Parameters.AddWithValue("@pass", pass);

            MySqlDataReader reader = command.ExecuteReader();

            List<UserAccount> accounts = new();
            while (reader.Read())
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
                CrossPlatformMessageRenderer.RenderMessages("The provided credentials do not match any existing account on our platform! Please try again!", "Retry", 4);
                user.EmailAddress = "";
                user.Password = "";
                return false;
            }
            else
            {
                CrossPlatformMessageRenderer.DisplayMobileSnackbar($"Welcome back, {accounts.ElementAt(0).Name}! You are being redirected to the main application...", "", 5);
                Shell.Current.GoToAsync("//Feed", true);
                return true;
            }

        }

        bool IAuthenticate.SignUp(UserAccount user)
        {
            throw new NotImplementedException();
        }

        private async Task<string> PrepareDefaultProfilePicture(string firebasePath)
        {
            FileResult file = new(@"\MotorEmpireAutohaus\Resources\Images\defaultprofilepic.png");
            string res=await FirebaseCloudStorage.AddFileToFirebaseCloudStorageAsync(file, firebasePath);
            return res;
        }

        private bool TrySave(Entity e) {

            UserAccount userAccount = (UserAccount)e;
            if (e.IsEmpty())
            {
                throw new InvalidSignUpCredentialsException("Cannot complete registration because of an input mismatch. Check the Sign Up form and try again!");
            }

            try
            {
                
                string profileImgURL=PrepareDefaultProfilePicture($"/users/{userAccount.UUID}").Result;
                userAccount.ProfileImageURL = profileImgURL;
                MySqlCommand command = new($"INSERT INTO {tableReference} (UUID, Name, EmailAddress, Username,Password,ProfileImageURL)" +
                    $"VALUES (@uuid, @name, @email, @username, @password, @imageURL", databaseConfig.DatabaseConnection);

                command.Prepare();
                string[] keys = { "@uuid", "@name", "@email", "@username", "@password", "@imageURL" };
                string[] values = { userAccount.UUID, userAccount.Name, userAccount.Username, Encrypter.EncryptPassword(userAccount.Password), profileImgURL };
            
                for(int i=0; i < keys.Length; i++)
                {
                    command.Parameters.AddWithValue(keys[i], values[i]);    
                }

                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException mySqlexc)
            {
                CrossPlatformMessageRenderer.RenderMessages($"Cannot register your account! Make sure that your credentials were introduced correctly and try again!\n\n\nError details:{mySqlexc.Message}\n", "Retry now", 10);
                return false;
            }
        }


        public Entity Save(Entity e)
        {
            bool result;
            UserAccount user = (UserAccount)e;
            try 
            {
                result = TrySave(e);
                if (result)
                {
                    return user;
                }
            }
            catch(InvalidSignUpCredentialsException ex)
            {
                CrossPlatformMessageRenderer.RenderMessages($"{ex.Message}","Ok",7);
                return null;
            }
            CrossPlatformMessageRenderer.RenderMessages("Cannot sign you up because of an input mismatch. Verify the form and try again later.", "Ok", 10);
            return null;
        }
    }
}
