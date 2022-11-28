namespace MotorEmpireAutohaus.Misc.Exceptions
{
    sealed class DatabaseConnectionFailedException : Exception
    {
        public DatabaseConnectionFailedException(string message) : base(message)
        {
        }
    }

    sealed class InvalidSignUpCredentialsException : Exception
    {
        public InvalidSignUpCredentialsException(string message) : base(message)
        {
        }
    }
}