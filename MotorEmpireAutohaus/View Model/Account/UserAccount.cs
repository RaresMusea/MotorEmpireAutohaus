using CommunityToolkit.Mvvm.ComponentModel;
using MotorEmpireAutohaus.Services.Account_Services;
using CommunityToolkit.Mvvm.Messaging;
using System;
using Microsoft.Toolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Input;
using System.Net.Mail;
using MotorEmpireAutohaus.Misc.Prebuilt_Components;
using System.Reflection.Metadata.Ecma335;
using MotorEmpireAutohaus.Misc;
using MotorEmpireAutohaus.Misc.Common;

namespace MotorEmpireAutohaus.View_Model.Account
{
    public partial class UserAccount : User
    {
        private readonly AccountService accountService;
        private readonly AuthValidation authValidation;

        [ObservableProperty]
        private string emailAddress;

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string passwordConfirmation;

        [ObservableProperty]
        private string profileImageURL;

        public UserAccount(string name, string emailAddress, string username, string password) : base(name, password)
        {
            this.emailAddress = emailAddress;
            this.username = username;
        }

        //Dependency injection
        public UserAccount(AuthValidation authValidation, AccountService accountService)
        {
            this.accountService = accountService;
            this.authValidation = authValidation;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is UserAccount)) return false;

            UserAccount castedProp = obj as UserAccount;
            if (Username == castedProp.Username && EmailAddress == castedProp.EmailAddress)
                return true;

            return false;

        }

        public static bool operator ==(UserAccount a, UserAccount b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(UserAccount a, UserAccount b)
        {
            return !(a == b);
        }

        public override bool IsEmpty()
        {
            return (UUID == "" || Name == "" || EmailAddress == "" || Password == "" || Username == "");
        }

        public override int GetHashCode()
        {
            object o = new();
            return o.GetHashCode();
        }


        private bool ValidPassword()
        {
            if (Password.Length == 0)
            {
                /*RenderErrorMessages("Password field cannot be empty!", "Retry");*/
                return false;

            }
            return true;
        }

        [RelayCommand]
        public void Login()
        {
            if (authValidation.ValidateLogin(ref emailAddress,ref password) == true)
            {
                EmailAddress = EmailAddress.ToLower();
                authValidation.RenderErrorMessages("Login Success!", "OK");
            }
            else
            {
                EmailAddress = string.Empty;
                Password = string.Empty;
            }
        }

        [RelayCommand]
        public void OnWrongEmailInputFocus()
        {
            if (!authValidation.IsEmailValid(EmailAddress).ValidationPassed)
            {
                EmailAddress = string.Empty;
            }
        }

        [RelayCommand]
        public void OnWrongPasswordInputFocus()
        {
            if (!authValidation.IsPassowrdValid(Password).ValidationPassed)
            {
                Password = string.Empty;
            }
        }
    }
}
