namespace MotorEmpireAutohaus.MVVM.Services.Authentication
{
   public class AuthValidationResult
    {
        public bool ValidationPassed { get; private set; }
        public string Remark { get; private set; }

        public AuthValidationResult(bool passed, string message)
        {
            ValidationPassed= passed;
            Remark= message;
        }
    }
}