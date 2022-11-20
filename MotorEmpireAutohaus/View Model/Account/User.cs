using CommunityToolkit.Mvvm.ComponentModel;
using MotorEmpireAutohaus.Misc.Encryption;
using MotorEmpireAutohaus.View_Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.View_Model.Account
{
    public abstract partial class User:Entity
    {
        [ObservableProperty]
        string name;

        [ObservableProperty]
        protected string password;

        public User(string name, string password) : base()
        {
            this.name = name;
            this.password =Encrypter.EncryptPassword(password);
        }

        public User() { }

        public abstract override bool IsEmpty();
    }
}
