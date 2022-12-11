using MotorEmpireAutohaus.MVVM.Services.Authentication;

namespace MotorEmpireAutohaus.MVVM.Services.Interfaces
{
    public interface ILogInValidator
    {
        public AuthValidationResult IsPassowrdValid();
        public AuthValidationResult IsEmailValid();
        public AuthValidationResult ValidateLogin();
    }
}
