using MotorEmpireAutohaus.MVVM.Services.Authentication;

namespace MotorEmpireAutohaus.MVVM.Services.Interfaces
{
    public interface IAuthValidator
    {
        public AuthValidationResult ValidateName(string name);

        public AuthValidationResult ValidatePassword(string password);

        public AuthValidationResult ValidatePasswords(string password, string passwordConfirmation);

        public AuthValidationResult ValidateEmailAddress(string emailAddress);

        public bool ArePasswordsMatching(string password, string passwordConfirmation);

        public AuthValidationResult ValidateUsername(string userName);

    }
}
