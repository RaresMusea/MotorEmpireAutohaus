using MotorEmpireAutohaus.MVVM.Models.User_Account_Model;
using MotorEmpireAutohaus.Services.Account_Services;
using MotorEmpireAutohaus.Services.Feed;
using MotorEmpireAutohaus.Tools.Utility.Messages;
using MVVM.Exceptions;
using MVVM.Models;
using MVVM.Models.Post_Model;
using MVVM.Services.Car_Entity_Services;
using MVVM.Services.Interfaces;
using MySqlConnector;

namespace MVVM.Services.Car_Post_Services
{
    public class CarPostService:IConnectableDataSource
    {

        private static readonly string TableReference = "CarPost";
        private static readonly string PostPicturesTableReference = "PostPictures";
        private readonly CarFilterService carFilterService;
        private readonly AccountService accountService;
        private readonly CarService carService;

        public CarPostService(CarFilterService carFilter, AccountService accountService, CarService carService)
        {
            IConnectableDataSource.Connect();
            carFilterService = carFilter;
            this.accountService = accountService;
            this.carService = carService;
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

        public List<string> RetrieveTransmissionTypes()
        {
            List<string> result = new();
            MySqlCommand command = new("SELECT Type FROM TransmissionType", IConnectableDataSource.databaseConfigurer.DatabaseConnection);
            command.Prepare();

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(reader.GetString(0));
            }

            reader.Close();
            return result;
        }

        public void UploadCarPost(CarPost post, int carIdentifier)
        {
            MySqlCommand command = new MySqlCommand($"INSERT INTO {TableReference} (" +
                $"UUID, Owner, Car, Description, CarEquipment, Price, DateTimeAdded) VALUES " +
                $"(@uuid, @owner, @car, @description, @carequipment, @price, @datetimeadded)",
                IConnectableDataSource.databaseConfigurer.DatabaseConnection);

            command.Prepare();
            command.Parameters.AddWithValue("@uuid", post.Car.UUID);
            command.Parameters.AddWithValue("@owner",accountService.RetrieveIdForUserUuid(post.Owner.UUID));
            command.Parameters.AddWithValue("@car", carIdentifier);
            command.Parameters.AddWithValue("@description", post.Description);
            command.Parameters.AddWithValue("@carequipment", post.CarEquipment);
            command.Parameters.AddWithValue("@price", post.Price);

            post.SetDateTimeAdded();
            command.Parameters.AddWithValue("@datetimeadded", post.DateTimeAdded);

            

            try
            {
                command.ExecuteNonQuery();

                post.PostPictures.ForEach(picture=>
                {
                    UploadCarPostPicture(post.Car.UUID, picture);
                });

            }

            catch (MySqlException mySqlExc)
            {
                throw new UploadFailedException($"Cannot upload your car post due to an unexpected error!\n" +
                    $"More details:\n{mySqlExc.Message} ");
            }
        
        }

        private void UploadCarPostPicture(string postUuid, PostPicture pictureRef)
        {
            MySqlCommand command = new($"INSERT INTO {PostPicturesTableReference} " +
                $"(Post, ImageURL) VALUES" +
                $"(@post, @imageurl)",
                IConnectableDataSource.databaseConfigurer.DatabaseConnection);

            command.Prepare();
            command.Parameters.AddWithValue("@post", carService.retrieveCarIdentifierByUuid(postUuid));
            command.Parameters.AddWithValue("@imageurl", pictureRef.Picture);

            command.ExecuteNonQuery();
        }

        private List<PostPicture> RetrieveCarPostPictures(string carPostUUID)
        {
            List<PostPicture> postPictures = new();
            MySqlCommand command = new MySqlCommand($"SELECT ImageURL FROM {PostPicturesTableReference} WHERE Post = @uuid ",
                IConnectableDataSource.databaseConfigurer.DatabaseConnection);

            command.Prepare();
            command.Parameters.AddWithValue("@uuid", carPostUUID);

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string url = reader.GetString(1);
                postPictures.Add(new PostPicture(url));
            }

            reader.Close();
            return postPictures.Count != 0 ? postPictures : null;

        }

        public List<CarPost> RetrieveAllPosts()
        {
            List<CarPost> posts = new();

            MySqlCommand command = new($"SELECT * FROM {TableReference}",
                IConnectableDataSource.databaseConfigurer.DatabaseConnection);
            command.Prepare();

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string UUID = reader.GetString(1);
                UserAccount owner = accountService.GetUserById(reader.GetInt32(2));
                string description = reader.GetString(4);
                string carEquipment = reader.GetString(5);
                int price = reader.GetInt32(6);
                string dateTimeAdded = reader.GetString(7);

                CarPost carPost = new(owner, description, carEquipment, price,
                    RetrieveCarPostPictures(UUID), dateTimeAdded);
                carPost.UUID = UUID;

                posts.Add(carPost);
            }

            reader.Close();
            
            return posts.Count != 0 ? posts : null;

        }
    }
}
