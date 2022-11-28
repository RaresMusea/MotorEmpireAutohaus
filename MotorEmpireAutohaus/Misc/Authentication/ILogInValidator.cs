namespace MotorEmpireAutohaus.Misc.Authentication
{
    public interface ILogInValidator
    {
        public AuthValidationResult IsPassowrdValid();
        public AuthValidationResult IsEmailValid();
        public AuthValidationResult ValidateLogin();
    }
}
