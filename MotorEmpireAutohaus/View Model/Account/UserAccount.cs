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

        public UserAccount(string UUID, string name, string emailAddress, string username, string password,string profileImageURL) : base(UUID, name, password)
        {
            this.emailAddress = emailAddress;
            this.username = username;
            this.profileImageURL= profileImageURL;
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
            if (this == null)
            {
                return true;
            }

            return (UUID == "" || Name == "" || EmailAddress == "" || Password == "" || Username == "");
        }

        public override int GetHashCode()
        {
            object o = new();
            return o.GetHashCode();
        }

        [RelayCommand]
        public async void Login()
        {
            if (authValidation.ValidateLogin(this))
            {
                EmailAddress = EmailAddress.ToLower();
                if (accountService.Login(this))
                {
                    //CrossPlatformMessageRenderer.RenderMessages("Login Success!", "OK",2);
                    await Shell.Current.GoToAsync("//Feed", true);
                }
            }
        }

        [RelayCommand]
        public async void Register()
        {
            if (authValidation.ValidateSignUp(this))
            {
                if (accountService.SignUp(this))
                {
                    await Shell.Current.GoToAsync("//LogIn", true);
                }
            }
        }
    }
}

