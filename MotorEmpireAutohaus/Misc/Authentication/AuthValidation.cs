using System.Net.Mail;
using MotorEmpireAutohaus.Misc.Common;
using MotorEmpireAutohaus.View_Model.Account;

namespace MotorEmpireAutohaus.Misc.Authentication
{
    public enum OperationType{
            UppercaseCheck,
            LowercaseCheck,
            DigitCheck,
            SymbolCheck
    };

    public class AuthValidation : IAuthValidator
    {
        /// <summary>
        /// Checks if the password matches its password confirmation field
        /// </summary>
        /// <param name="password">password-The initial password</param>
        /// <param name="matchingPassword">matchingPassword-The password confirmation</param>
        /// <returns>True, if the password matches its confirmation, False otherwise</returns>
        public bool ArePasswordsMatching(string password, string matchingPassword)
        {
            return password == matchingPassword;
        }

        public AuthValidationResult ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new AuthValidationResult(false, "Name cannot be empty!");
            }

            if (!name.Contains(' '))
            {
                return new AuthValidationResult(false, "The Name field should contain both your first and last name, separated by a space!");
            }

            if (name.Length < 5) {
                return new AuthValidationResult(false, "The input provided does not describe a name!");
            }

            return new AuthValidationResult(true, "");
        }

        private string FormatName(string name)
        {
            if (!ValidateName(name).ValidationPassed)
            {
                string formatted = name.ToLower();

                string[] tokens = formatted.Split(" ");
                formatted = "";

                foreach (string token in tokens)
                {
                    for (int idx = 0; idx < token.Length; idx++)
                    {
                        if (idx == 0)
                        {
                            formatted += tokens[idx].ToUpper();
                        }
                        else
                        {
                            formatted += token[idx];
                        }
                    }
                }
                return formatted;
            }
            return name;
        }

        public AuthValidationResult ValidateEmailAddress(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return new AuthValidationResult(false, "The email address cannot be empty!");
            }

            //string trimmed = email.Trim();
            try
            {
                var unused = new MailAddress(email);
            }
            catch (Exception e)
            {
                return new AuthValidationResult(false, $"The provided email address is invalid! \n\n\n{e.Message}");
            }

            return new AuthValidationResult(true,email.ToLower());
        }

        private delegate bool Util(OperationType choice, char charToVerify);

        private static bool DelegateMethod(OperationType choice, char charToVerify)
        {
            switch (choice)
            {
                case OperationType.UppercaseCheck:
                {
                    return char.IsUpper(charToVerify);
                }
                case OperationType.LowercaseCheck:
                {
                    return char.IsLower(charToVerify);
                }
                case OperationType.DigitCheck:
                {
                    return char.IsDigit(charToVerify);
                }
                case OperationType.SymbolCheck:
                {
                    return (charToVerify=='@' || charToVerify=='$' || charToVerify=='^'||charToVerify=='&'||charToVerify=='*'||charToVerify=='('||charToVerify==')' || charToVerify=='_');
                }

            }
            return true;
        }

        private static bool ContainsSpecificChar(string input, Util d, OperationType choice)
        {
            foreach(char c in input)
            {
                if (d(choice,c))
                    return true;

            }
            return false;
        }

        public AuthValidationResult ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return new AuthValidationResult(false, "Password cannot be empty!");
            }

            if (password.Length < 8)
            {
                return new AuthValidationResult(false, "The Password field should contain at least 8 characters!");
            }

            if (ContainsSpecificChar(password,DelegateMethod,OperationType.LowercaseCheck) && ContainsSpecificChar(password,DelegateMethod,OperationType.UppercaseCheck) && ContainsSpecificChar(password,DelegateMethod,OperationType.SymbolCheck) && ContainsSpecificChar(password,DelegateMethod,OperationType.DigitCheck) && password.Length >= 8){
                return new AuthValidationResult(true, "");
            }

            return new AuthValidationResult(false, "The password is invalid!");
        }

        public AuthValidationResult ValidatePasswords(string password, string passwordConfirmation)
        {
            if(string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passwordConfirmation))
            {
                return new AuthValidationResult(false, "The Password fields cannot be empty!");
            }
            if(password.Length<8 || passwordConfirmation.Length < 8)
            {
                return new AuthValidationResult(false, "The password field should contain at least 8 characters, including:\n\tAt least one capital letter.\n\tAt least one lowercase letter.\n\tOne or more digits.\n\tOne ore more special characters/symbols[like %^&#@&()]");
            }

            if (!ArePasswordsMatching(password, passwordConfirmation))
            {
                return new AuthValidationResult(false, "The passwords do not match!");
            }

            if(ContainsSpecificChar(password,DelegateMethod,OperationType.LowercaseCheck) && ContainsSpecificChar(password,DelegateMethod,OperationType.UppercaseCheck) && ContainsSpecificChar(password,DelegateMethod,OperationType.SymbolCheck) && ContainsSpecificChar(password,DelegateMethod,OperationType.DigitCheck) && password.Length >= 8)
            {
                return new AuthValidationResult(true, "");
            }

            return new AuthValidationResult(false, "Invalid password provided!");
        }

        public AuthValidationResult ValidateUsername(string username)
        {
            if(string.IsNullOrEmpty(username))
            {
                return new AuthValidationResult(false, "The username field cannot be empty!");
            }

            if (username.Length < 4)
            {
                return new AuthValidationResult(false, "Your username should have at least 4 characters!");
            }

            if(username.Contains(' '))
            {
                return new AuthValidationResult(false, "Your username cannot contain whitespaces!");
            }

            return new AuthValidationResult(true, "");
        }

        public bool ValidateLogin(UserAccount u)
        {
            var emailValidation = ValidateEmailAddress(u.EmailAddress);
            if (emailValidation.ValidationPassed==false)
            {
                u.EmailAddress = string.Empty;
                CrossPlatformMessageRenderer.RenderMessages(emailValidation.Remark, "Retry", 6);
                return false;
            }

            var passwordValidation = ValidatePassword(u.Password);
            if (passwordValidation.ValidationPassed==false)
            {
                u.Password = string.Empty;
                CrossPlatformMessageRenderer.RenderMessages(passwordValidation.Remark, "Retry", 6);
                return false;
            }
            return true;
        }

        public bool ValidateSignUp(UserAccount user){
            var nameValidation = ValidateName(user.Name);
            var usernameValidation = ValidateUsername(user.Username);
            var emailValidation = ValidateEmailAddress(user.EmailAddress);
            var passwordsValidation = ValidatePasswords(user.Password, user.PasswordConfirmation);

            if (!nameValidation.ValidationPassed)
            {
                user.Name = string.Empty;
                CrossPlatformMessageRenderer.RenderMessages(nameValidation.Remark, "Retry", 5);
                return false;
            }

            if(!usernameValidation.ValidationPassed)
            {
                user.Username=string.Empty;
                CrossPlatformMessageRenderer.RenderMessages(usernameValidation.Remark, "Retry", 5);
                return false;
            }

            if (!emailValidation.ValidationPassed)
            {
                user.EmailAddress=string.Empty;
                CrossPlatformMessageRenderer.RenderMessages(emailValidation.Remark, "Retry", 5);
                return false;
            }

            if (!passwordsValidation.ValidationPassed)
            {
                user.Password=string.Empty;
                user.PasswordConfirmation=string.Empty;
                CrossPlatformMessageRenderer.RenderMessages(passwordsValidation.Remark, "Retry", 8);
                return false;
            }

            user.Name = FormatName(user.Name);
            return true;
        }
    }
}
