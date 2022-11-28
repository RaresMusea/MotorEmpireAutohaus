namespace MotorEmpireAutohaus.Storage.MySQL
{
    public interface IDataSource
    {
        public void OpenConnection();
        public void CloseConnection();
    }
}