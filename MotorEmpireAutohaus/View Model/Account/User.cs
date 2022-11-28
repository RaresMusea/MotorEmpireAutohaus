using CommunityToolkit.Mvvm.ComponentModel;
using MotorEmpireAutohaus.Misc.Encryption;
using MotorEmpireAutohaus.View_Model.Base;

namespace MotorEmpireAutohaus.View_Model.Account
{
    public abstract partial class User:Entity
    {
        [ObservableProperty]
        string _name;

        [ObservableProperty]
        protected string password;

        public User(string name, string password) : base()
        {
            this._name = name;
            this.password =Encrypter.EncryptPassword(password);
        }

        public User(string UUID, string name, string password) : base(UUID)
        {
            this._name = name;
            this.password = password;
        }

        public User() { }

        public abstract override bool IsEmpty();
    }
}
