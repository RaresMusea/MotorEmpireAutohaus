using CommunityToolkit.Mvvm.ComponentModel;
using MVVM.Models.Vehicle_Models.Car.Car_Model;
using System.Globalization;
using MVVM.Models.Base;
using UserAccount = MVVM.Models.User_Account_Model.UserAccount;
using CommunityToolkit.Mvvm.Input;
using Tools.Utility.CarFilterAndValidator;

namespace MVVM.Models.Post_Model
{
    public partial class CarPost : Entity
    {
        [ObservableProperty] private UserAccount owner;

        [ObservableProperty] private Car car;

        [ObservableProperty] private string description;

        [ObservableProperty] private string headingTitle;

        [ObservableProperty] private string carEquipment;

        [ObservableProperty] private string carSpecsOverview;

        [ObservableProperty] private int? price;

        [ObservableProperty] private List<PostPicture> postPictures = new();

        [ObservableProperty] private PostPicture mainPostPicture;

        [ObservableProperty] private int views;

        [ObservableProperty] private string dateTimeAdded;

        [ObservableProperty] private CarDetails carDetails;

        public CarPost(UserAccount owner, string description, string carEquipment, int? price,
            List<PostPicture> postPictures, int views, string dateTimeAdded)
        {
            carDetails = new();
            this.owner = owner;
            this.description = description;
            HeadingTitle = description.Split("\r")[0];
            this.carEquipment = carEquipment;
            this.price = price;
            this.postPictures = postPictures;
            this.views = views;
            this.dateTimeAdded = dateTimeAdded;
           
        }

        public CarPost()
        {
        }


        public new void GenerateUuid()
        {
            Uuid = car.Uuid;
        }

        public override bool IsEmpty()
        {
            return owner.IsEmpty() || car.IsEmpty() || string.IsNullOrEmpty(car.Uuid) ||
                   string.IsNullOrEmpty(description) || string.IsNullOrEmpty(carEquipment) || price == 0;
        }

        public void SetDateTimeAdded()
        {
            DateTime dateTime = DateTime.Now;
            string stringDateTime = dateTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            string[] tokens = stringDateTime.Split(" ");

            DateTimeAdded = $"Added on {tokens[0]} at {tokens[1]}";
        }

        [RelayCommand]
        private void NextPicture()
        {
            MainPostPicture = postPictures.IndexOf(mainPostPicture) + 1 == postPictures.Count ? postPictures[0] : postPictures[postPictures.IndexOf(MainPostPicture) + 1];
        }

        [RelayCommand]
        private void PreviousPicture()
        {
            MainPostPicture = postPictures.IndexOf(mainPostPicture) - 1 < 0 ? postPictures[postPictures.Count - 1] : postPictures[postPictures.IndexOf(MainPostPicture) - 1];
        }

        partial void OnCarChanged(Car value)
        {
            CarDetails = new();
            if (!value.IsEmpty())
            {
                CarSpecsOverview = $"• {value.Year}" +
                    $" • {CarFilterFormatter.FormatMileage(value.Mileage)}" +
                    $" • {value.EngineCapacity.Replace("cmc", "cm3")}" +
                    $" • {value.Horsepower} hp" +
                    $" • {value.FuelType} •";

                carDetails.UploadInformation = $"Uploaded by {Owner.Name} ({Owner.Username})";
                carDetails.ViewedBy = $"{Views} views";
                carDetails.ModelBinding = $"Model: {value.Model}";
                carDetails.ManufacturerBinding = $"Manufacturer: {value.Manufacturer}";
                carDetails.YearBinding = "Year: " + value.Year.ToString();
                carDetails.FuelTypeBinding = $"Fuel Type: {value.FuelType}";
                carDetails.EngineCapacityBinding = $"Cylindric capacity: {value.EngineCapacity.Replace("cmc", "cm3")}";
                carDetails.HorsepowerBinding = $"Engine power (horsepower): {value.Horsepower.ToString()} hp";
                carDetails.MileageBinding = $"Mileage: {value.Mileage.ToString()} km";
                carDetails.PriceBinding = $"{CarFilterFormatter.FormatPrice((int)Price)}";



                if (value.Generation is not null)
                {
                    carDetails.GenerationBinding = $"Generation: {value.Generation}";
                    carDetails.HasGeneration = true;
                }
            }
        }
    }
}