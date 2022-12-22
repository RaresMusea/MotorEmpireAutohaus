using MotorEmpireAutohaus.Tools.Encryption;
using MySqlConnector;
using MotorEmpireAutohaus.Storage.MySQL;
using MotorEmpireAutohaus.Storage.Exceptions;
using MotorEmpireAutohaus.Tools.Utility.Messages;
using MotorEmpireAutohaus.MVVM.Services.Interfaces;
using MotorEmpireAutohaus.MVVM.Models.User_Account_Model;
using MotorEmpireAutohaus.MVVM.Models.Base;
using System.Data;

namespace MotorEmpireAutohaus.Services.Account_Services
{
    public class AccountService : IAuthenticate,IStorable
    {
        private DatabaseConfigurer _databaseConfig;
        private static readonly string TableReference = "User";

        public  AccountService()
        {
            Connect();
            //Debug.WriteLine("Connection successful!");
            CrossPlatformMessageRenderer.RenderMessages("Connected successfully!", "Close", 3);
        }

        private void Connect()
        {
            _databaseConfig = new DatabaseConfigurer();
            _databaseConfig.OpenConnection();
        }

        public bool SignUp(UserAccount user)
        {
            return Save(user) != null;
        }

        public bool Login(UserAccount user)
        {
            string pass = Encrypter.EncryptPassword(user.Password);
            MySqlCommand command = new($"SELECT * FROM {TableReference} WHERE EmailAddress=@email and Password=@pass",
                _databaseConfig.DatabaseConnection);
            command.Prepare();

            command.Parameters.AddWithValue("@email", user.EmailAddress);
            command.Parameters.AddWithValue("@pass", pass);

            MySqlDataReader reader = command.ExecuteReader();

            List<UserAccount> accounts = new();
            while (reader.Read())
            {
                string uuid = (string)reader.GetValue(1);
                string name = (string)reader.GetValue(2);
                string emailAddress = (string)reader.GetValue(3);
                string username = (string)reader.GetValue(4);
                string password = (string)reader.GetValue(5);
                string profileImageUrl = (string)reader.GetValue(6);
                UserAccount result = new(uuid, name, emailAddress, username, password, profileImageUrl);
                accounts.Add(result);
            }

            reader.Close();

            if (accounts.Count == 0)
            {
                CrossPlatformMessageRenderer.RenderMessages(
                    "The provided credentials do not match any existing account on our platform! Please try again!",
                    "Retry", 4);
                user.EmailAddress = "";
                user.Password = "";
                return false;
            }
            else
            {
                UpdateLastSeenFor(user);
                user.Name = accounts[0].Name;
                user.EmailAddress = accounts[0].EmailAddress;
                user.Username = accounts[0].Username;
                user.ProfileImageUrl = accounts[0].ProfileImageUrl;
                user.UUID = accounts[0].UUID;
                CrossPlatformMessageRenderer.DisplayMobileSnackbar(
                    $"Welcome back, {accounts.ElementAt(0).Name}!","Close", 5);
                return true;
            }
        }

        bool IAuthenticate.SignUp(UserAccount user)
        {
            return Save(user) != null;
        }

        private bool TrySave(Entity e)
        {
            UserAccount userAccount = (UserAccount)e;
            if (e.IsEmpty())
            {
                throw new InvalidSignUpCredentialsException(
                    "Cannot complete registration because of an input mismatch. Check the Sign Up form and try again!");
            }

            try
            {
                /* string profileImgURL=PrepareDefaultProfilePicture($"/users/{userAccount.UUID}").Result;
                 userAccount.ProfileImageURL = profileImgURL;*/


                /*var stream = File.Open("C:\\Users\\rares\\OneDrive\\Desktop\\An 3 Facultate\\Medii si instrumente de programare\\Repos\\MotorEmpireAutohaus\\MotorEmpireAutohaus\\Resources\\Images\\defaultprofilepic.png",FileMode.Open);
                var firebase = new FirebaseStorage("gs://motor-empire-autohaus.appspot.com").Child($"/users/{userAccount.UUID}/defaultprofilepic").PutAsync(stream);*/


                MySqlCommand command = new(
                    $"INSERT INTO {TableReference} (UUID, Name, EmailAddress, Username,Password,ProfileImageURL)" +
                    $"VALUES (@uuid, @name, @email, @username, @password, @imageURL)",
                    _databaseConfig.DatabaseConnection);

                command.Prepare();
                string[] keys = { "@uuid", "@name", "@email", "@username", "@password", "@imageURL" };
                string[] values =
                {
                    userAccount.UUID, userAccount.Name, userAccount.EmailAddress, userAccount.Username,
                    Encrypter.EncryptPassword(userAccount.Password),
                    @"https://firebasestorage.googleapis.com/v0/b/motor-empire-autohaus.appspot.com/o/defaultprofilepic.png?alt=media&token=256db5c0-c612-42f7-bc4f-d6595a99da80"
                };
                userAccount.ProfileImageUrl=
                    @"https://firebasestorage.googleapis.com/v0/b/motor-empire-autohaus.appspot.com/o/defaultprofilepic.png?alt=media&token=256db5c0-c612-42f7-bc4f-d6595a99da80";

                for (int i = 0; i < keys.Length; i++)
                {
                    command.Parameters.AddWithValue(keys[i], values[i]);
                }

                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException mySqlexc)
            {
                CrossPlatformMessageRenderer.RenderMessages(
                    $"Cannot register your account! Make sure that your credentials were introduced correctly and try again!\n\n\nError details:{mySqlexc.Message}\n",
                    "Retry now", 10);
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
                    CrossPlatformMessageRenderer.RenderMessages(
                        $"Account created succesfully, {user.Name}!\nHappy searching and safe roads!\nYou will be automatically redirected to the login page once the register process ends.",
                        "Okay", 8);

                    return user;
                }
            }
            catch (InvalidSignUpCredentialsException ex)
            {
                CrossPlatformMessageRenderer.RenderMessages($"{ex.Message}", "Ok", 7);
                return null;
            }

            CrossPlatformMessageRenderer.RenderMessages(
                "Cannot sign you up because of an input mismatch. Verify the form and try again later.", "Ok", 10);
            return null;
        }

        private void UpdateLastSeenFor(UserAccount user)
        {
            MySqlCommand command = new($"UPDATE {TableReference} SET LastSeen=NOW() WHERE EmailAddress=@email",_databaseConfig.DatabaseConnection);
            command.Prepare();
            command.Parameters.AddWithValue("@email", user.EmailAddress);
            command.ExecuteNonQuery();
        }


        public Entity RetrieveByUuid(string uuid)
        {
            MySqlCommand command = new MySqlCommand($"SELECT * FROM {TableReference} WHERE UUID=@uuid", _databaseConfig.DatabaseConnection);
            command.Prepare();
            command.Parameters.AddWithValue("@uuid", uuid);

            MySqlDataReader reader = command.ExecuteReader();

            Entity returnValue=null;

            while (reader.Read())
            {
                string unique = reader.GetString(1);
                string name=reader.GetString(2);
                string email=reader.GetString(3);
                string username=reader.GetString(4);
                string password=reader.GetString(5);
                string profileImage=reader.GetString(6);
                returnValue = new UserAccount(unique, name, email, username, password, profileImage);
            }
            reader.Close();
            return returnValue;
        }

        public void UpdateAccountBindings(UserAccount oldUser, UserAccount newUser)
        {
            oldUser.Name= newUser.Name;
            oldUser.Password = newUser.Password;
            oldUser.Username=newUser.Username;
            oldUser.EmailAddress=newUser.EmailAddress;
            oldUser.UUID = newUser.UUID;
            oldUser.ProfileImageUrl=newUser.ProfileImageUrl;
        }

        public Entity Update(Entity entity)
        {
            UserAccount user = (UserAccount)entity;
            MySqlCommand command = new($"UPDATE {TableReference} SET Name=@name, Username=@username, EmailAddress=@email WHERE UUID=@uuid", _databaseConfig.DatabaseConnection);
            command.Prepare();
            command.Parameters.AddWithValue("@name", user.Name);
            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@email", user.EmailAddress);
            command.Parameters.AddWithValue("@uuid", user.UUID);
            command.ExecuteNonQuery();

            entity= (UserAccount)RetrieveByUuid(user.UUID);
            
            return entity;
        }

    }
}