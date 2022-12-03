﻿using CommunityToolkit.Mvvm.ComponentModel;
using MotorEmpireAutohaus.Misc.Encryption;
using MotorEmpireAutohaus.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.Models
{
    public abstract partial class Account: Entity
    {
        [ObservableProperty]
        private string name;

        public string Password { get; set; }

        public Account(string name, string password) : base()
        {
            Name = name;
            Password = Encrypter.EncryptPassword(password);
        }

        public Account(string UUID, string name, string password) : base(UUID)
        {
            Name = name;
            Password = password;
        }

        public Account() { }

        public abstract override bool IsEmpty();
    }
}
