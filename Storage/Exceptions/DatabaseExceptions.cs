namespace MotorEmpireAutohaus.Storage.Exceptions
{
    public sealed class DatabaseConnectionFailedException : Exception
    {
        public DatabaseConnectionFailedException(string message) : base(message)
        {
        }
    }

    public sealed class InvalidSignUpCredentialsException : Exception
    {
        public InvalidSignUpCredentialsException(string message) : base(message)
        {
        }
    }
}