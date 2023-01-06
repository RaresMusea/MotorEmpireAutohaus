using CommunityToolkit.Mvvm.ComponentModel;
using MVVM.Models.Base;
using Tools.Encryption;

namespace MVVM.Models.User_Account_Model
{
    public abstract partial class Account : Entity
    {
        [ObservableProperty] private string name;

        [ObservableProperty] private string password;

        protected Account(string name, string password)
        {
            Name = name;
            Password = Encrypter.EncryptPassword(password);
        }

        protected Account(string uuid, string name, string password) : base(uuid)
        {
            Name = name;
            Password = password;
        }

        protected Account()
        {
        }

        public abstract override bool IsEmpty();
    }
}