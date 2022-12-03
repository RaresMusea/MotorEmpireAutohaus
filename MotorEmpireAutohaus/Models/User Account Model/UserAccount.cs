﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.Models.User_Account_Model
{
    public class UserAccount:Account
    {
        public string EmailAddress { get; set; }

        public string Username { get;  set; }
        
        public string ProfileImageUrl { get; set; }

        public string PasswordConfirmation { get; set; }

        public UserAccount() { }

        public UserAccount(string name, string emailAddress, string username, string password): base(name,password)
        {
            EmailAddress = emailAddress;
            Username = username;
        }
        public UserAccount(string UUID, string name, string emailAddress, string username, string password, string profileImageURL) : base(UUID, name, password)
        {
            EmailAddress = emailAddress;
            Username = username;
            ProfileImageUrl = profileImageURL;
        }

        public override bool Equals(object obj)
        {
            if (obj is not UserAccount) return false;

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
            if (this is null)
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

    }
}