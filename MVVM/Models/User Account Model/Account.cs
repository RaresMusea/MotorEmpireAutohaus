using CommunityToolkit.Mvvm.ComponentModel;
using MotorEmpireAutohaus.MVVM.Models.Base;
using MotorEmpireAutohaus.Tools.Encryption;


namespace MotorEmpireAutohaus.MVVM.Models.User_Account_Model
{
    public abstract partial class Account: Entity
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        public string password;

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
