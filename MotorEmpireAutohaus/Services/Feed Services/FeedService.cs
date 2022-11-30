using MotorEmpireAutohaus.Services.Feed_Services.Filters;
using MotorEmpireAutohaus.Storage.MySQL;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.Services.Feed
{
    public class FeedService:ICarFilter
    {
        private DatabaseConfigurer _databaseConfigurer;

        public FeedService()
        {
            _databaseConfigurer= new DatabaseConfigurer();
            _databaseConfigurer.OpenConnection();
        }

        public List<string> RetrieveCarBodyTypes()
        {
            List<string> types = new();

            MySqlCommand command = new("SELECT * FROM CarBodyType", _databaseConfigurer.DatabaseConnection);
            command.Prepare();

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                types.Add(reader.GetString(0));
            }
            reader.Close();

            return types;
        }

        public int GetIdByManufacturerName(string name)
        {
            int id=-1;
            MySqlCommand command = new("SELECT ID from Manufacturer WHERE Name=@name",_databaseConfigurer.DatabaseConnection);
            command.Prepare();
            command.Parameters.AddWithValue("@name", name);
            MySqlDataReader reader=command.ExecuteReader();
            while (reader.Read())
            {
                id = reader.GetInt32(0);
            }

            reader.Close();
            return id;
        }


        public List<string> GetManufacturers()
        {
            List<string> result = new();
            MySqlCommand command = new("SELECT Name FROM Manufacturer",_databaseConfigurer.DatabaseConnection);
            command.Prepare();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(reader.GetString(0));
            }

            reader.Close();
            return result;
        }


        public List<string> GetAllModelsFromManufacturer(string manufacturer)
        {
            List<string> result = new();

            int manufacturerId = GetIdByManufacturerName(manufacturer);
            MySqlCommand command = new("SELECT ModelName from Model WHERE Brand=@id",_databaseConfigurer.DatabaseConnection);
            command.Prepare();
            command.Parameters.AddWithValue("@id", manufacturerId);
            MySqlDataReader reader=command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(reader.GetString(0));
            }

            reader.Close();
            return result;
        }
    }
}
