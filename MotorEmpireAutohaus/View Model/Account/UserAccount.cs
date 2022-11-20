using CommunityToolkit.Mvvm.ComponentModel;
using MotorEmpireAutohaus.Services.Account_Services;
using CommunityToolkit.Mvvm.Messaging;
using System;
using Microsoft.Toolkit.Mvvm.Input;

namespace MotorEmpireAutohaus.View_Model.Account
{
    public partial class UserAccount:User
    {
        private readonly AccountService accountService;

        [ObservableProperty]
        private string emailAddress;

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string passwordConfirmation;


        public UserAccount(string name, string emailAddress, string username,string password) : base(name, password)
        {
            this.emailAddress = emailAddress;
            this.username = username;
        }

        //Dependency injection
        public UserAccount(AccountService accountService) => this.accountService = accountService;

        public override bool Equals(object obj)
        {
            if(!(obj is UserAccount)) return false;
            
            UserAccount castedProp=obj as UserAccount;
            if(Username==castedProp.Username && EmailAddress==castedProp.EmailAddress)
                return true;

            return false;
       
        }

        public static bool operator == (UserAccount a, UserAccount b)
        {
            return a.Equals(b);
        }

        public static bool operator != (UserAccount a, UserAccount b)
        {
            return !(a == b);
        }

        public override bool IsEmpty()
        {
            return (UUID == "" || Name == "" || EmailAddress == "" || Password == "" || Username == "");
        }

        public override int GetHashCode()
        {
            object o=new();
            return o.GetHashCode();
        }

        [ICommand]
        private void SignUp()
        {
             accountService.SignUp();
        }
    }
}
