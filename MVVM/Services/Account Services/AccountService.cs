using MVVM.Models.Base;
using MVVM.Services.Interfaces;
using MySqlConnector;
using Storage.Exceptions;
using Tools.Encryption;
using Tools.Handlers;
using Tools.Utility.Messages;
using UserAccount = MVVM.Models.User_Account_Model.UserAccount;

namespace MVVM.Services.Account_Services
{
    public class AccountService : IAuthenticate, IStorable, IConnectableDataSource
    {
        private const string TableReference = "User";

        public AccountService()
        {
            IConnectableDataSource.Connect();
            //Debug.WriteLine("Connection successful!");
            CrossPlatformMessageRenderer.RenderMessages("Connected successfully!", "Close", 3);
        }

        public bool SignUp(UserAccount user)
        {
            return Save(user) != null;
        }

        public bool Login(UserAccount user)
        {
            string pass = Encrypter.EncryptPassword(user.Password);
            MySqlCommand command = new($"SELECT * FROM {TableReference} WHERE EmailAddress=@email and Password=@pass",
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
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
                string phoneNumber = reader.GetValue(7) == DBNull.Value ? null : (string)reader.GetValue(7);

                UserAccount result;
                if (phoneNumber is not null)
                {
                    result = new(uuid, name, emailAddress, username, password, profileImageUrl, phoneNumber);
                }
                else
                {
                    result = new(uuid, name, emailAddress, username, password, profileImageUrl);
                }

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
                user.Uuid = accounts[0].Uuid;
                user.PhoneNumber = accounts[0].PhoneNumber;
                CrossPlatformMessageRenderer.DisplayMobileSnackbar(
                    $"Welcome back, {accounts.ElementAt(0).Name}!", "Close", 5);

                Logger.CurrentlyLoggedInUuid = user.Uuid;
                Logger.CurrentlyLoggedInEmail = user.EmailAddress;
                Logger.CurrentlyLoggedInName = user.Name;
                Logger.CurrentlyLoggedInProfileImage = user.ProfileImageUrl;
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
            if (userAccount.Uuid == "")
            {
                userAccount.GenerateUuid();
            }

            if (e.IsEmpty())
            {
                throw new InvalidSignUpCredentialsException(
                    "Cannot complete registration because of an input mismatch. Check the Sign Up form and try again!");
            }

            try
            {
                MySqlCommand command = new(
                    $"INSERT INTO {TableReference} (UUID, Name, EmailAddress, Username,Password,ProfileImageURL)" +
                    $"VALUES (@uuid, @name, @email, @username, @password, @imageURL)",
                    IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);

                command.Prepare();
                string[] keys = { "@uuid", "@name", "@email", "@username", "@password", "@imageURL" };
                string[] values =
                {
                    userAccount.Uuid, userAccount.Name, userAccount.EmailAddress, userAccount.Username,
                    Encrypter.EncryptPassword(userAccount.Password),
                    @"https://firebasestorage.googleapis.com/v0/b/motor-empire-autohaus.appspot.com/o/defaultprofilepic.png?alt=media&token=256db5c0-c612-42f7-bc4f-d6595a99da80"
                };
                userAccount.ProfileImageUrl =
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
            UserAccount user = (UserAccount)e;
            try
            {
                var result = TrySave(e);
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
            MySqlCommand command = new($"UPDATE {TableReference} SET LastSeen=NOW() WHERE EmailAddress=@email",
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
            command.Prepare();
            command.Parameters.AddWithValue("@email", user.EmailAddress);
            command.ExecuteNonQuery();
        }


        public Entity RetrieveByUuid(string uuid)
        {
            MySqlCommand command = new MySqlCommand($"SELECT * FROM {TableReference} WHERE UUID=@uuid",
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
            command.Prepare();
            command.Parameters.AddWithValue("@uuid", uuid);

            MySqlDataReader reader = command.ExecuteReader();

            Entity returnValue = null;

            while (reader.Read())
            {
                string unique = reader.GetString(1);
                string name = reader.GetString(2);
                string email = reader.GetString(3);
                string username = reader.GetString(4);
                string password = reader.GetString(5);
                string profileImage = reader.GetString(6);
                string phoneNumber = reader.IsDBNull(7) ? null : reader.GetString(7);
                returnValue = phoneNumber is not null
                    ? new UserAccount(unique, name, email, username, password, profileImage, phoneNumber)
                    : new UserAccount(unique, name, email, username, password, profileImage);
            }

            reader.Close();
            return returnValue;
        }

        public void UpdateProfilePictureForUser(string uuid, string pathToImage)
        {
            MySqlCommand command = new($"UPDATE {TableReference} SET ProfileImageURL=@image WHERE UUID=@uuid",
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
            command.Prepare();
            command.Parameters.AddWithValue("@image", pathToImage);
            command.Parameters.AddWithValue("@uuid", uuid);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException exc)
            {
                CrossPlatformMessageRenderer.RenderMessages(
                    $"Unable to update your profile picture due to an unexpected error." +
                    $" Please try again!\nMore details: {exc.Message}", "OK", 4);
                return;
            }

            CrossPlatformMessageRenderer.RenderMessages("Successfully updated your profile picture!", "OK", 3);
        }

        public string GetUuidByEmail(string emailAddress)
        {
            MySqlCommand command = new($"SELECT UUID FROM {TableReference} WHERE EmailAddress=@email",
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
            return uuid;
        }

        public int RetrieveIdForUserUuid(string uuid)
        {
            MySqlCommand command = new($"SELECT ID FROM {TableReference} WHERE UUID = @uuid",
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
            command.Prepare();
            command.Parameters.AddWithValue("@uuid", uuid);

            MySqlDataReader reader = command.ExecuteReader();
            int id = -1;

            while (reader.Read())
            {
                id = reader.GetInt32(0);
            }

            reader.Close();
            return id;
        }

        public Entity Update(Entity entity)
        {
            UserAccount user = (UserAccount)entity;
            MySqlCommand command =
                new($"UPDATE {TableReference} SET Name=@name, Username=@username, EmailAddress=@email WHERE UUID=@uuid",
                    IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
            command.Prepare();
            command.Parameters.AddWithValue("@name", user.Name);
            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@email", user.EmailAddress);
            command.Parameters.AddWithValue("@uuid", user.Uuid);
            command.ExecuteNonQuery();

            entity = (UserAccount)RetrieveByUuid(user.Uuid);

            return entity;
        }

        public string RetrievePasswordByUuid(string uuid)
        {
            MySqlCommand command = new($"SELECT password FROM {TableReference} WHERE UUID=@uuid",
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
            command.Prepare();
            command.Parameters.AddWithValue("@uuid", uuid);

            MySqlDataReader reader = command.ExecuteReader();
            string password = "";
            while (reader.Read())
            {
                password = reader.GetString(0);
            }

            reader.Close();
            return password;
        }

        public void UpdatePasswordForUser(UserAccount userAccount, string newPassword)
        {
            MySqlCommand command = new($"UPDATE {TableReference} SET Password=@pass WHERE UUID=@uuid",
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
            command.Prepare();
            command.Parameters.AddWithValue("@pass", Encrypter.EncryptPassword(newPassword));
            command.Parameters.AddWithValue("@uuid", userAccount.Uuid);
            command.ExecuteNonQuery();

            userAccount.Password = newPassword;
        }

        private string RetrievePhoneNumberForUser(string uuid)
        {
            MySqlCommand command = new($"SELECT PhoneNumber FROM {TableReference} WHERE UUID=@uuid",
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
            command.Prepare();
            command.Parameters.AddWithValue("@uuid", uuid);

            MySqlDataReader reader = command.ExecuteReader();

            string phoneNumber = null;
            while (reader.Read())
            {
                phoneNumber = reader.GetString(0);
            }

            reader.Close();
            return phoneNumber;
        }

        public bool HasPhoneNumber(string uuid)
        {
            return RetrievePhoneNumberForUser(uuid) != null;
        }

        public bool SetPhoneNumber(string uuid, string phoneNumber)
        {
            MySqlCommand command = new MySqlCommand($"UPDATE {TableReference} SET PhoneNumber=@number WHERE UUID=@uuid",
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
            command.Prepare();
            command.Parameters.AddWithValue("@number", phoneNumber);
            command.Parameters.AddWithValue("@uuid", uuid);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException exc)
            {
                CrossPlatformMessageRenderer.RenderMessages(
                    $"Cannot add phone number due to un unexpected error!\nMore details: {exc.Message}", "Retry", 4);
                return false;
            }

            CrossPlatformMessageRenderer.RenderMessages($"Added phone number successfully!", "OK", 3);

            return true;
        }

        public bool RemovePhoneNumber(string uuid)
        {
            MySqlCommand command = new($"UPDATE {TableReference} SET PhoneNumber=null WHERE UUID=@uuid",
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
            command.Prepare();
            command.Parameters.AddWithValue("@uuid", uuid);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException exc)
            {
                CrossPlatformMessageRenderer.RenderMessages(
                    $"An unexpected error occurred while attempting to remove your phone" +
                    $" number. Please try again later.\nMore details:\n{exc.Message}", "Retry", 5);

                return false;
            }

            CrossPlatformMessageRenderer.RenderMessages("Successfully removed your phone number!", "OK", 3);
            return true;
        }

        public bool Delete(Entity entity)
        {
            MySqlCommand command = new($"DELETE FROM {TableReference} WHERE UUID=@uuid",
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
            command.Prepare();
            command.Parameters.AddWithValue("@uuid", entity.Uuid);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException exc)
            {
                CrossPlatformMessageRenderer.RenderMessages(
                    $"The deletion could not be accomplished due to an unexpected error. Please try again later.\n" +
                    $"More details: {exc.Message}", "Retry", 5);
                return false;
            }

            CrossPlatformMessageRenderer.RenderMessages(
                $"Account deleted successfully!\n You are being redirected to the Sign Up page...", "OK", 4);
            return true;
        }

        public UserAccount GetUserById(int id)
        {
            MySqlCommand command = new($"SELECT UUID FROM {TableReference} WHERE ID=@id",
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);

            command.Prepare();
            command.Parameters.AddWithValue("@id", id);

            MySqlDataReader reader = command.ExecuteReader();
            string uuid = "";
            while (reader.Read())
            {
                uuid = reader.GetString(0);
            }

            reader.Close();
            return string.IsNullOrEmpty(uuid) ? null : (UserAccount)RetrieveByUuid(uuid);
        }
    }
}