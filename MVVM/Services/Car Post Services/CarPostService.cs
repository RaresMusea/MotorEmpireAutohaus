using MVVM.Exceptions;
using MVVM.Models;
using MVVM.Models.Post_Model;
using MVVM.Services.Account_Services;
using MVVM.Services.Car_Entity_Services;
using MVVM.Services.Car_Filter_Services;
using MVVM.Services.Interfaces;
using MySqlConnector;
using Storage.MySQL;
using Tools.Handlers;
using UserAccount = MVVM.Models.User_Account_Model.UserAccount;

namespace MVVM.Services.Car_Post_Services
{
    public class CarPostService : IConnectableDataSource
    {
        private const string TableReference = "CarPost";
        private const string PostPicturesTableReference = "PostPictures";
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
            List<string> result = new();
            MySqlCommand command = new("SELECT Type FROM VehicleType",
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
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

        public List<string> RetrieveGenerationsForModel(string model)
        {
            return carFilterService.GetGenerationBasedOnModel(model);
        }

        public List<string> RetrieveTransmissionTypes()
        {
            List<string> result = new();
            MySqlCommand command = new("SELECT Type FROM TransmissionType",
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
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
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);

            command.Prepare();
            command.Parameters.AddWithValue("@uuid", post.Car.Uuid);
            command.Parameters.AddWithValue("@owner", accountService.RetrieveIdForUserUuid(post.Owner.Uuid));
            command.Parameters.AddWithValue("@car", carIdentifier);
            command.Parameters.AddWithValue("@description", post.Description);
            command.Parameters.AddWithValue("@carequipment", post.CarEquipment);
            command.Parameters.AddWithValue("@price", post.Price);

            post.SetDateTimeAdded();
            command.Parameters.AddWithValue("@datetimeadded", post.DateTimeAdded);


            try
            {
                command.ExecuteNonQuery();

                post.PostPictures.ForEach(picture => { UploadCarPostPicture(post.Car.Uuid, picture); });
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
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);

            command.Prepare();
            command.Parameters.AddWithValue("@post", carService.RetrieveCarIdentifierByUuid(postUuid));
            command.Parameters.AddWithValue("@imageurl", pictureRef.Picture);

            command.ExecuteNonQuery();
        }

        private bool DeleteAssociatedPicturesForCarPost(string postUuid)
        {

            MySqlCommand command = new($"DELETE FROM {PostPicturesTableReference} WHERE Post=@carid",
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);

            command.Prepare();
            command.Parameters.AddWithValue("@carid", carService.RetrieveCarIdentifierByUuid(postUuid));

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

        public List<PostPicture> RetrieveCarPostPictures(string carPostUuid)
        {
            List<PostPicture> postPictures = new();
            MySqlCommand command = new MySqlCommand(
                $"SELECT ImageURL FROM {PostPicturesTableReference} WHERE Post = @uuid ",
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);

            command.Prepare();
            command.Parameters.AddWithValue("@uuid", carService.RetrieveCarIdentifierByUuid(carPostUuid));

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string url = reader.GetString(0);
                postPictures.Add(new PostPicture(url));
            }

            reader.Close();
            return postPictures.Count != 0 ? postPictures : null;
        }

        public List<CarPost> RetrieveAllPosts()
        {
            List<CarPost> posts = new();
            List<int> ownersId = new();

            MySqlCommand command = new($"SELECT * FROM {TableReference}",
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
            command.Prepare();

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string uuid = reader.GetString(1);
                ownersId.Add(reader.GetInt32(2));
                string description = reader.GetString(4);
                string carEquipment = reader.GetString(5);
                int price = reader.GetInt32(6);
                int views = reader.GetInt32(7);
                string dateTimeAdded = reader.GetString(8);
             
                CarPost carPost = new(null, description, carEquipment, price,
                    null, views, dateTimeAdded)
                {
                    Uuid = uuid
                };

                posts.Add(carPost);
            }

            reader.Close();

            if (ownersId.Count != 0)
            {
                for (int i = 0; i < ownersId.Count; i++)
                {
                    posts[i].Owner = accountService.GetUserById(ownersId[i]);
                }

                return posts;

            }

            return null;
        }

        public void IncreaseViewsForPost(string postUuid)
        {
            MySqlCommand command = new($"UPDATE {TableReference} SET Views=Views+1 WHERE UUID=@uuid",
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);

            command.Prepare();
            command.Parameters.AddWithValue("@uuid", postUuid);
            command.ExecuteNonQuery();
        }


        public bool AddToFavorites(string postUuid, string userUuid)
        {
            MySqlCommand command = new("INSERT INTO FavoritePosts(PostUUID, UserUUID) " +
                "VALUES (@postuuid, @useruuid)",
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);

            command.Prepare();
            command.Parameters.AddWithValue("@postuuid", postUuid);
            command.Parameters.AddWithValue("@useruuid", userUuid);
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

        public bool WasPostAddedToFavoritesBy(string postUuid,string userUuid)
        {
            MySqlCommand command = new("SELECT COUNT(*) FROM FavoritePosts WHERE PostUUID=@postuuid AND UserUUID=@useruuid",
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
            command.Prepare();
            command.Parameters.AddWithValue("@postuuid", postUuid);
            command.Parameters.AddWithValue("useruuid", userUuid);

            MySqlDataReader reader = command.ExecuteReader();

            int result=0;
            while (reader.Read())
            {
                result = reader.GetInt32(0);
            }

            reader.Close();
            return result != 0;
        }
        
        public bool DeletePost(CarPost carPost)
        {
            if (WasPostAddedToFavoritesBy(carPost.Uuid, Logger.CurrentlyLoggedInUuid))
            {
                bool added = RemoveFromFavorites(carPost.Uuid);
            }

            /*return carService.Delete(carPost.Car);*/
            MySqlCommand command = new($"DELETE FROM {TableReference} WHERE UUID=@uuid",
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);

            command.Prepare();
            command.Parameters.AddWithValue("@uuid", carPost.Uuid);
            
            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException) 
            {
                return false;
            }

            return true && DeleteAssociatedPicturesForCarPost(carPost.Uuid) && carService.Delete(carPost.Car);
        }

        public bool RemoveFromFavorites(string postUuid)
        {
            MySqlCommand command = new("DELETE FROM FavoritePosts WHERE PostUUID=@uuid",
                IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);

            command.Prepare();
            command.Parameters.AddWithValue("@uuid", postUuid);

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

        private List<string> GetFavoritesUuids(string userUuid)
        {
            List<string> result = new();

            MySqlCommand command = new MySqlCommand("SELECT PostUUID FROM FavoritePosts WHERE" +
                " UserUUID=@uuid", IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);

            command.Prepare();
            command.Parameters.AddWithValue("@uuid", userUuid);

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(reader.GetString(0));
            }

            reader.Close();

            if (result.Count == 0)
            {
                return null;
            }

            return result;
        }

        public CarPost RetrieveCarPostByUuid(string postUuid)
        {
            CarPost result = new();
            int ownerId = -1;

            MySqlCommand command = new($"SELECT * FROM {TableReference} WHERE UUID=@uuid",
                    IConnectableDataSource.DatabaseConfigurer.DatabaseConnection);
            command.Prepare();
            command.Parameters.AddWithValue("@uuid", postUuid);
            
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string uuid = reader.GetString(1);
                ownerId = reader.GetInt32(2);
                string description = reader.GetString(4);
                string carEquipment = reader.GetString(5);
                int price = reader.GetInt32(6);
                int views = reader.GetInt32(7);
                string dateTimeAdded = reader.GetString(8);
                CarPost carPost = new(null, description, carEquipment, price,
                    null, views, dateTimeAdded)
                {
                        Uuid = uuid
                };

                result = carPost;
            }
                
            reader.Close();

                if (ownerId!=-1)
                {

                    result.Owner = accountService.GetUserById(ownerId);
                    result.Car = (Models.Vehicle_Models.Car.Car_Model.Car)carService.RetrieveByUuid(result.Uuid);
                    result.PostPictures = RetrieveCarPostPictures(result.Uuid);
                    result.MainPostPicture = result.PostPictures.ElementAt(0);
                    result.AddedToFavoritesByCurrentUser = WasPostAddedToFavoritesBy(result.Uuid,
                    Logger.CurrentlyLoggedInUuid);
                    return result;
                }

                return null;
        }

        public List<CarPost> RetrieveFavoritesForUser(string userUuid)
        {
            List<string> postUuids = GetFavoritesUuids(userUuid);
            if(postUuids is null)
            {
                return null;
            }

            List<CarPost> result = new();

            foreach(string postUuid in postUuids)
            {
                result.Add(RetrieveCarPostByUuid(postUuid));
            }

            return result;

        }
    }
}