using CommunityToolkit.Mvvm.ComponentModel;
using MVVM.Models.Vehicle_Models.Car.Car_Model;
using System.Globalization;
using MVVM.Models.Base;
using UserAccount = MVVM.Models.User_Account_Model.UserAccount;
using CommunityToolkit.Mvvm.Input;
using Tools.Utility.CarFilterAndValidator;

namespace MVVM.Models.Post_Model
{
    [QueryProperty(nameof(UserAccount), nameof(UserAccount))]
    [QueryProperty(nameof(Car), nameof(Car))]
    public partial class CarPost : Entity
    {
        [ObservableProperty] private UserAccount owner;

        [ObservableProperty] private Car car;

        [ObservableProperty] private string description;

        [ObservableProperty] private string headingTitle;

        [ObservableProperty] private string carEquipment;

        [ObservableProperty] private string carSpecsOverview;

        [ObservableProperty] private int? price;

        [ObservableProperty] private List<Vehicle_Models.Picture_Model.PostPicture> postPictures = new();

        [ObservableProperty] private Vehicle_Models.Picture_Model.PostPicture mainPostPicture;

        [ObservableProperty] private int views;

        [ObservableProperty] private string dateTimeAdded;

        [ObservableProperty] private CarSpecs carSpecs;

        [ObservableProperty] private CarPost post;

        [ObservableProperty] private bool addedToFavoritesByCurrentUser;

        public CarPost(UserAccount owner, string description, string carEquipment, int? price,
            List<Vehicle_Models.Picture_Model.PostPicture> postPictures, int views, string dateTimeAdded)
        {
            carSpecs = new();
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
            MainPostPicture = postPictures.IndexOf(mainPostPicture) + 1 == postPictures.Count
                ? postPictures[0]
                : postPictures[postPictures.IndexOf(MainPostPicture) + 1];
        }

        [RelayCommand]
        private void PreviousPicture()
        {
            MainPostPicture = postPictures.IndexOf(mainPostPicture) - 1 < 0
                ? postPictures[^1]
                : postPictures[postPictures.IndexOf(MainPostPicture) - 1];
        }


        partial void OnCarChanged(Car value)
        {
            CarSpecs = new();
            if (!value.IsEmpty())
            {
                CarSpecsOverview = $"• {value.Year}" +
                                   $" • {CarFilterFormatter.FormatMileage(value.Mileage)}" +
                                   $" • {value.EngineCapacity.Replace("cmc", "cm3")}" +
                                   $" • {value.Horsepower} hp" +
                                   $" • {value.FuelType} •";

                if (DeviceInfo.Platform == DevicePlatform.WinUI || DeviceInfo.Platform == DevicePlatform.MacCatalyst)
                {
                    carSpecs.UploadInformation = $"Uploaded by {Owner.Name} ({Owner.Username})";
                }
                else
                {
                    carSpecs.UploadInformation = $"Uploaded by {Owner.Name}";
                }

                carSpecs.ViewedBy = $"{Views}" + (Views == 1 ? " view" : " views");
                carSpecs.ModelBinding = $"Model: {value.Model}";
                carSpecs.ManufacturerBinding = $"Manufacturer: {value.Manufacturer}";
                carSpecs.YearBinding = "Year: " + value.Year.ToString();
                carSpecs.FuelTypeBinding = $"Fuel Type: {value.FuelType}";
                carSpecs.EngineCapacityBinding = $"Cylindric capacity: {value.EngineCapacity.Replace("cmc", "cm3")}";
                carSpecs.HorsepowerBinding = $"Engine power (horsepower): {value.Horsepower.ToString()} hp";
                carSpecs.MileageBinding = $"Mileage: {CarFilterFormatter.FormatMileage(value.Mileage)}";
                carSpecs.PriceBinding = $"{CarFilterFormatter.FormatPrice((int)Price)}";

                carSpecs.DescriptionBinding = description.Replace(headingTitle, "");

                if (value.Generation is not null)
                {
                    carSpecs.GenerationBinding = $"Generation: {value.Generation}";
                    carSpecs.HasGeneration = true;
                }


                if (car.Torque != 0)
                {
                    carSpecs.HasTorque = true;
                    carSpecs.TorqueBinding = $"Torque: {Car.Torque} Nm";
                }

                carSpecs.TransmissionBinding = $"Transmission: {car.Gears} speed {car.Transmission} gearbox";
            }
        }
    }
}