﻿using MotorEmpireAutohaus.Misc.Prebuilt_Components;
using MotorEmpireAutohaus.View_Model.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.Misc.Common
{
    public enum OperationType{
            UppercaseCheck,
            LowercaseCheck,
            DigitCheck,
            SymbolCheck
    };

    public class AuthValidation : ISignUpValidator, ILogInValidator
    {

        readonly Action<string, string> displayMobileSnackbar = async (message, actionButtonText) =>
        {
            await SnackbarComponent.GenerateSnackbar(
                message,
                actionButtonText,
                Color.FromArgb("#dbdbdb"),
                Color.FromArgb("#414141"),
                Colors.Black,
                Colors.White,
                Color.FromArgb("#AF0404"),
                Color.FromArgb("#AF0404"),
                15, 14, 0, 8,
                null
                );
        };

        readonly Action<string, string> displayDesktopAlert = async (message, actionButton) =>
        {
            await Application.Current.MainPage.DisplayAlert("Motor Empire Autohaus Authentication", message, actionButton);
        };


        public async void RenderErrorMessages(string message, string buttonText)
        {

            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                displayDesktopAlert(message, buttonText);
            }
            else
            {
                await Task.Run(() => displayMobileSnackbar(message, buttonText));
            }
        }


        bool ISignUpValidator.ArePasswordsMatching(string password, string matchingPassword)
        {
            return password == matchingPassword;
        }

        public AuthValidationResult ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new AuthValidationResult(false, "Name cannot be empty!");
            }

            if (!name.Contains(" "))
            {
                return new AuthValidationResult(false, "The Name field should contain both your first and last name, separated by a space");
            }

            if (name.Length < 5) {
                return new AuthValidationResult(false, "The input provided does not describe a name!");
            }

            return new AuthValidationResult(true, "");
        }

        string ISignUpValidator.FormatName(string name)
        {
            if (!ValidateName(name).ValidationPassed)
            {
                string formatted = string.Empty;
                formatted = name.ToLower();

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

        public AuthValidationResult IsEmailValid(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return new AuthValidationResult(false, "The email address cannot be empty!");
            }

            string trimmed = email.Trim();
            try
            {
                var mail = new MailAddress(trimmed);
            }
            catch
            {
                return new AuthValidationResult(false, "The provided email address is invalid!");
            }

            return new AuthValidationResult(true,trimmed.ToLower());
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
                    return (charToVerify=='@' || charToVerify=='$' || charToVerify=='^'||charToVerify=='&'||charToVerify=='*'||charToVerify=='('||charToVerify==')');
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

        public AuthValidationResult IsPassowrdValid(string password)
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

        public bool ValidateLogin(ref string emailAddress, ref string password)
        {
            var emailValidation = IsEmailValid(emailAddress);
            if (!emailValidation.ValidationPassed)
            {
                emailAddress = string.Empty;
            }

            var passwordValidation = IsPassowrdValid(password);
            if (!passwordValidation.ValidationPassed)
            {
                password= string.Empty;
            }

            if (emailValidation.ValidationPassed == false)
            {
                emailAddress = "";
                RenderErrorMessages(emailValidation.Remark, "Retry");
                return false;
            }

            if (passwordValidation.ValidationPassed == false)
            {
                RenderErrorMessages(passwordValidation.Remark, "Retry");
                password = "";
                return false;
            }

            return true;
        }

        public bool ValidateSignUp()
        {
            return true;
        }

        AuthValidationResult ISignUpValidator.IsNameValid(string name)
        {
            throw new NotImplementedException();
        }

        AuthValidationResult ILogInValidator.ValidateLogin()
        {
            throw new NotImplementedException();
        }

        AuthValidationResult ILogInValidator.IsPassowrdValid()
        {
            throw new NotImplementedException();
        }

        AuthValidationResult ILogInValidator.IsEmailValid()
        {
            throw new NotImplementedException();
        }
    }
}