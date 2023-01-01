using MVVM.Models.Post_Model;
using MVVM.Models.Vehicle_Models.Car.Car_Model;
using MVVM.Services.Car_Entity_Services;
using MVVM.Services.Car_Post_Services;
using MVVM.Services.Interfaces;

namespace MVVM.Services
{
    public class PostFeedService:IConnectableDataSource
    {

        private readonly CarService carService;

        private readonly CarPostService postService;

        public PostFeedService(CarService carService, CarPostService postService) 
        {
            IConnectableDataSource.Connect();
            this.carService = carService;
            this.postService = postService;
        }

        public List<CarPost> RetrieveAllPosts()
        {
            List<CarPost> carPosts = postService.RetrieveAllPosts();
            List<Car> cars = carService.RetrieveAllCars(); 

            if(carPosts is null || cars is null)
            {
                return null;
            }

            for(int i=0;i<carPosts.Count;i++)
            {
                carPosts[i].Car = cars[i];
                carPosts[i].PostPictures = postService.RetrieveCarPostPictures(carPosts[i].Car.Uuid);
                carPosts[i].MainPostPicture = carPosts[i].PostPictures.ElementAt(0);
            }

            return carPosts;
        }


    }
}
