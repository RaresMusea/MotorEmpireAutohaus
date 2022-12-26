
using MotorEmpireAutohaus.Storage.MySQL;
using MVVM.Services.Interfaces;
using MySqlConnector;

namespace MVVM.Services.Car_Entity_Services
{
    public class CarService:IConnectableDataSource
    {
        private DatabaseConfigurer databaseConfig;
        private static readonly string TableReference = "Car";

        public CarService()
        {
            Connect();
        
        }

        private void Connect()
        {
            IConnectableDataSource.Connenct();
        }

        public List<string> GetAllFuelTypes()
        {
            List<string> result = new();
            MySqlCommand  command = new("SELECT FuelType FROM FuelTypes", IConnectableDataSource.databaseConfigurer.DatabaseConnection);
            command.Prepare();

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(reader.GetString(0));
            }

            reader.Close();
            return result;
        }



    }
}
