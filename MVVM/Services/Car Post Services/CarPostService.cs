using MotorEmpireAutohaus.Services.Feed;
using MotorEmpireAutohaus.Storage.MySQL;
using MVVM.Services.Interfaces;
using MySqlConnector;

namespace MVVM.Services.Car_Post_Services
{
    public class CarPostService:IConnectableDataSource
    {

        private static readonly string TableReference = "CarPost";
        private readonly CarFilterService carFilterService;

        public CarPostService(CarFilterService carFilter)
        {
            IConnectableDataSource.Connenct();
            carFilterService = carFilter;
        }

        public List<string> RetrieveAllVehicleTypes()
        {
            List<string> result=new();
            MySqlCommand command = new("SELECT Type FROM VehicleType",IConnectableDataSource.databaseConfigurer.DatabaseConnection);
            command.Prepare();

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(reader.GetString(0));
            }

            reader.Close();
            return result;
        }

        public List<string> RetrieveAllChassisTypes()
        {
            return carFilterService.RetrieveCarBodyTypes();
        }

        public List<string> RetrieveAllCarManufacturers()
        {
            return carFilterService.GetManufacturers();
        }

        public List<string> RetrieveAllModelsFromManufacturer(string manufacturer)
        {
            return carFilterService.GetAllModelsFromManufacturer(manufacturer);
        }

        public List<string>RetrieveGenerationsForModel(string model)
        {
            return carFilterService.GetGenerationBasedOnModel(model);
        }

    }
}
