using MVVM.Services.Authentication;

namespace MVVM.Services.Interfaces
{
    public interface ILogInValidator
    {
        public AuthValidationResult IsPassowrdValid();
        public AuthValidationResult IsEmailValid();
        public AuthValidationResult ValidateLogin();
    }
}