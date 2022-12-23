
using MotorEmpireAutohaus.Storage.MySQL;

namespace MVVM.Services.Car_Entity_Services
{
    public class CarService
    {
        private DatabaseConfigurer databaseConfig;
        private static readonly string TableReference = "Car";

        public CarService()
        {
            Connect();
        
        }

        private void Connect()
        {
            databaseConfig = new DatabaseConfigurer();
            databaseConfig.OpenConnection();
        }



    }
}
