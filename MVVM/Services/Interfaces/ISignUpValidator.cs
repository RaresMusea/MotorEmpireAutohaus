using MotorEmpireAutohaus.MVVM.Services.Authentication;

namespace MotorEmpireAutohaus.MVVM.Services.Interfaces
{
    public interface ISignUpValidator
    {
        public string FormatName(string name);

        public bool ArePasswordsMatching(string password, string matchingPassword);

        public AuthValidationResult IsNameValid(string name);

        public AuthValidationResult SignUpPasswordValidation(string password, string passwordConfirmation);
    }
}
