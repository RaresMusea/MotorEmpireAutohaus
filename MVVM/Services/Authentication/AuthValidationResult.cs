namespace MVVM.Services.Authentication
{
    public class AuthValidationResult
    {
        public bool ValidationPassed { get; }
        public string Remark { get; }

        public AuthValidationResult(bool passed, string message)
        {
            ValidationPassed = passed;
            Remark = message;
        }
    }
}