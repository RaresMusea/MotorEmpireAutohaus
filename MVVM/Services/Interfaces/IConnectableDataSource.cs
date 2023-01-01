using Storage.MySQL;

namespace MVVM.Services.Interfaces
{
    public interface IConnectableDataSource
    {
        static readonly DatabaseConfigurer DatabaseConfigurer = new DatabaseConfigurer();

        static void Connect()
        {
            DatabaseConfigurer.OpenConnection();
        }
    }
}