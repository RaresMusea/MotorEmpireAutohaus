
using MotorEmpireAutohaus.MVVM.Models.Base;
using MotorEmpireAutohaus.MVVM.Services.Interfaces;
using MotorEmpireAutohaus.Storage.MySQL;
using MVVM.Exceptions;
using MVVM.Models.Vehicle_Models.Car.Car_Model;
using MVVM.Services.Interfaces;
using MySqlConnector;

namespace MVVM.Services.Car_Entity_Services
{
    public class CarService:IConnectableDataSource,IStorable
    {
        private static readonly string TableReference = "Car";

        public CarService()
        {
            IConnectableDataSource.Connect();
        
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

        public bool Delete(Entity entity)
        {
            MySqlCommand command = new MySqlCommand($"DELETE FROM {TableReference} WHERE UUID=@identifier", IConnectableDataSource.databaseConfigurer.DatabaseConnection);
            command.Prepare();
            command.Parameters.AddWithValue("@identifier", entity.UUID);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                return false;
            }

            return true;

        }

        public Entity RetrieveByUuid(string uuid)
        {
            MySqlCommand command = new($"SELECT * FROM {TableReference} WHERE UUID=@uuid", IConnectableDataSource.databaseConfigurer.DatabaseConnection);
            command.Prepare();
            command.Parameters.AddWithValue("@uuid", uuid);

            Car responseEntity = new();

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                responseEntity.ChassisType = reader.GetString(2);
                responseEntity.VehicleType = reader.GetString(3);
                responseEntity.Manufacturer = reader.GetString(4);
                responseEntity.Model = reader.GetString(5);
                responseEntity.Generation = reader.GetString(6);
                responseEntity.Year = reader.GetInt32(7);
                responseEntity.FuelType = reader.GetString(8);
                responseEntity.Mileage = reader.GetInt32(9);
                responseEntity.EngineCapacity = reader.GetString(10) + " cm3";
                responseEntity.Horsepower = reader.GetInt32(11);
                responseEntity.Torque = reader.GetInt32(12);
                responseEntity.Transmission = reader.GetString(13);
                responseEntity.Gears = reader.GetString(14);
            }

            responseEntity.UUID = uuid;
            return responseEntity;

        }

        public Entity Save(Entity entity)
        {
            Car car = (Car)entity;
            MySqlCommand command = new MySqlCommand($"INSERT INTO {TableReference} (UUID, ChassisType, VehicleType, Manufacturer, Model, " +
                                                    $"Generation, Year, FuelType, Mileage, EngineCapacity, Horsepower," +
                                                    $" Torque, Transmission, Gears) VALUES (@uuid, @chassis, @vehicleType, @manufacturer, @model," +
                                                    $" @generation, @year, @fuelType, @mileage, @engineCapacity, @horsepower, @torque, " +
                                                    $"@transmission, @gears)", IConnectableDataSource.databaseConfigurer.DatabaseConnection);
            command.Prepare();
            command.Parameters.AddWithValue("@uuid", car.UUID);
            command.Parameters.AddWithValue("@chassis", car.ChassisType);
            command.Parameters.AddWithValue("@vehicleType", car.VehicleType);
            command.Parameters.AddWithValue("@manufacturer", car.Manufacturer);
            command.Parameters.AddWithValue("@model", car.Model);

            if (car.Generation is null)
            {
                command.Parameters.AddWithValue("@generation", null);
            }
            else
            {
                command.Parameters.AddWithValue("@generation", car.Generation);
            }

            command.Parameters.AddWithValue("@year", car.Year);
            command.Parameters.AddWithValue("@fuelType", car.FuelType);
            command.Parameters.AddWithValue("@mileage", car.Mileage);
            command.Parameters.AddWithValue("@engineCapacity", car.EngineCapacity);
            command.Parameters.AddWithValue("@horsepower", car.Horsepower);
            command.Parameters.AddWithValue("@torque", car.Torque == 0 ? null : car.Torque);
            command.Parameters.AddWithValue("@transmission", car.Transmission);
            command.Parameters.AddWithValue("@gears", car.Gears);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException mySqlExc)
            {
                throw new UploadFailedException($"Cannot upload your car post due to an unexpected error!\n" +
                    $"More details:\n{mySqlExc.Message} ");
            }

            return car;

        }

        public Entity Update(Entity entity)
        {
            throw new NotImplementedException();
        }

        public int retrieveCarIdentifierByUuid(string uuid)
        {
            MySqlCommand command = new MySqlCommand($"SELECT id FROM {TableReference} WHERE UUID = @uuid",
                IConnectableDataSource.databaseConfigurer.DatabaseConnection);
            command.Prepare();
            command.Parameters.AddWithValue("@uuid", uuid);

            MySqlDataReader reader = command.ExecuteReader();
            int id=-1;
            while (reader.Read())
            {
                id = reader.GetInt32(0);
            }

            reader.Close();
            return id;
        }

    }
}
