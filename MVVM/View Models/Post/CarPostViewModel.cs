﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MotorEmpireAutohaus.MVVM.Models.User_Account_Model;
using MotorEmpireAutohaus.MVVM.View_Models.Base;
using MotorEmpireAutohaus.Services.Feed;
using MotorEmpireAutohaus.Storage.Firebase_Storage;
using MotorEmpireAutohaus.Tools.Utility.Messages;
using MVVM.Models;
using MVVM.Models.Post_Model;
using MVVM.Services.Car_Entity_Services;
using MVVM.Services.Car_Post_Services;
using MVVM.Validations.Car_Post_Validation;
using System.Collections.ObjectModel;

namespace MVVM.View_Models.Post
{
    [QueryProperty (nameof(UserAccount), nameof(UserAccount))]
    public partial class CarPostViewModel:BaseViewModel
    {
        private readonly CarService carService;

        private readonly CarPostService carPostService;

        private readonly UserAccount owner;

        [ObservableProperty]
        CarPost post;

        [ObservableProperty]
        private List<string> vehicleTypes;

        [ObservableProperty]
        [NotifyPropertyChangedFor (nameof(IsVehicleTypeSelected))]
        private string selectedVehicleType;

        public bool IsVehicleTypeSelected => selectedVehicleType != null;


        [ObservableProperty]
        private List<string> chassisTypes;

        [ObservableProperty]
        private string selectedChassisType;


        [ObservableProperty]
        private List<string> manufacturers;

        [ObservableProperty]
        [NotifyPropertyChangedFor (nameof(IsManufacturerSelected))]
        private string selectedManufacturer;

        [ObservableProperty]
        private int selectedManufacturerIndex=-1;

        public bool IsManufacturerSelected => selectedManufacturer != null;

        [ObservableProperty]
        private List<string> models;

        [ObservableProperty]
        [NotifyPropertyChangedFor (nameof(IsModelSelected))]
        private string selectedModel;

        [ObservableProperty]
        private int selectedModelIndex=-1;

        public bool IsModelSelected => selectedModel != null;

        [ObservableProperty]
        [NotifyPropertyChangedFor (nameof(ModelHasGenerations))]
        private List<string> generations;

        [ObservableProperty]
        private string selectedGeneration;

        public bool ModelHasGenerations => selectedModel!=null && generations.Count!=0;

        [ObservableProperty]
        private int? year=null;

        [ObservableProperty]
        private List<string> fuelTypes;

        [ObservableProperty]
        private string selectedFuelType;

        [ObservableProperty]
        private int? mileage =null;

        [ObservableProperty]
        private string engineCapacity;

        [ObservableProperty]
        private int? enginePower;

        [ObservableProperty]
        private int? torque;

        [ObservableProperty]
        private List<string> transmissionTypes;

        [ObservableProperty]
        private string selectedTransmissionType;

        [ObservableProperty]
        private List<string> gearboxConfigurations;

        [ObservableProperty]
        private string selectedGearboxConfiguration;

        [ObservableProperty]
        private bool carDetailsFormVisible = true;

        [ObservableProperty]
        private bool pictureUploadVisible = false;

        [ObservableProperty]
        private ObservableCollection<PostPicture> postPictures = new();

        [ObservableProperty]
        private bool carouselVisible;

        public CarPostViewModel(CarService carService, CarPostService carPostService, CarPost post, UserAccount userAccount)
        {
            this.carService= carService;
            this.carPostService= carPostService;
            this.post = post;
            owner = userAccount;
            post.Owner= owner;
            post.Car = new Models.Vehicle_Models.Car.Car_Model.Car();
            InitializeCarPostUploadComponent();
            gearboxConfigurations = new List<string>
            {
                "3 + 1",
                "4 + 1",
                "5 + 1",
                "6 + 1",
                "7 + 1",
            };
            carouselVisible = false;
        }

        private void InitializeCarPostUploadComponent() 
        {
            InitializePickerValues(GetAllVehicleTypes(), ref vehicleTypes);
            InitializePickerValues(GetAllChassis(), ref chassisTypes);
            InitializePickerValues(GetCarManufacturers(), ref manufacturers);
            InitializePickerValues(GetModelsFromManufacturer(), ref models);
            InitializePickerValues(GetModelGenerations(), ref generations);
            InitializePickerValues(GetAllFuelTypes(), ref fuelTypes);
            InitializePickerValues(GetAllTransmissionTypes(), ref transmissionTypes);
        }

        private void InitializePickerValues (List<string>providedList, ref List<string>pickerBackend)
        {
            pickerBackend = providedList;
        }

        private List<string> GetAllVehicleTypes()
        {
            return carPostService.RetrieveAllVehicleTypes();
        }

        private List<string> GetAllChassis()
        {
            return carPostService.RetrieveAllChassisTypes();
        }

        private List<string> GetCarManufacturers()
        {
            return carPostService.RetrieveAllCarManufacturers();
        }

        private List<string> GetModelsFromManufacturer()
        {
            return carPostService.RetrieveAllModelsFromManufacturer(selectedManufacturer);
        }

        private List<string> GetModelGenerations()
        {
            return carPostService.RetrieveGenerationsForModel(selectedModel);
        }

        private List<string> GetAllFuelTypes()
        {
            return carService.GetAllFuelTypes();
        }

        private List<string> GetAllTransmissionTypes()
        {
            return carPostService.RetrieveTransmissionTypes();
        }


        private PickOptions ConfigureFileTypes()
        {
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "jpg","png","svg" } }, 
                    //{ /*DevicePlatform.Android, new[] { "jpg", "png", "svg" } }*/ 
                    { DevicePlatform.WinUI, new[] { "jpg", "png", "svg" } },                     
                    { DevicePlatform.Tizen, new[] {"jpg", "png", "svg" } },
                    { DevicePlatform.macOS, new[] {"jpg", "png", "svg" } }, 
                });

            PickOptions options = new()
            {
                PickerTitle = "Please select at least one image of your car to upload",
                FileTypes = customFileType,
            };

            return options;
        }

        private async Task<List<FileResult>> PickCarPostPictures()
        {
            List<FileResult> files = new();
            try
            {
                IEnumerable<FileResult> photos = await FilePicker.PickMultipleAsync();
                files = photos.ToList();
            }
            catch (Exception ex)
            {
                CrossPlatformMessageRenderer.RenderMessages($"An error occurred while attempting to access the files you specified. Please try again.\nMore details:\n${ex.Message}", "Retry", 4);
                return null;
            }

            return files;
        }

        [RelayCommand]
        public void SwitchUploadFormWithPictureUpload()
        {
            VehiclePostUploadValidationResult result = VehiclePostValidator.AreCarDetailsValid(post.Car);
            if (result.ValidationPassed)
            {
                carDetailsFormVisible = false;
                pictureUploadVisible = true;
            }
            else
            {
                CrossPlatformMessageRenderer.RenderMessages(result.Remark, "Retry", 10);
            }
        }

        [RelayCommand]
        public async void UploadCarPostPictures()
        {
            List<FileResult> photos = await PickCarPostPictures();
            string path = $"Images/VehiclePosts/{selectedVehicleType}/{post.Car.UUID}";

            photos.ForEach(async photo =>
            {
                string urlToFile=await FirebaseCloudStorage.AddFileToFirebaseCloudStorageAsync(photo, path);
                PostPictures.Add(new PostPicture(urlToFile));
                CarouselVisible = true;
            });
        }

        [RelayCommand]
        public void DeletePicturesAndGoBack()
        {
            PostPictures.Clear();
            CarouselVisible = false;
        }

        partial void OnSelectedVehicleTypeChanged(string value)
        {
            post.Car.VehicleType = value;
        }

        partial void OnSelectedChassisTypeChanged(string value)
        {
            post.Car.ChassisType = value;
        }

        partial void OnSelectedManufacturerChanged(string value)
        {
            post.Car.Manufacturer = value;
            models = carPostService.RetrieveAllModelsFromManufacturer(value);
        }

        partial void OnSelectedManufacturerIndexChanged(int value)
        {
            Models = carPostService.RetrieveAllModelsFromManufacturer(manufacturers[value]);
        }

        partial void OnSelectedModelChanged(string value)
        {
            post.Car.Model=value;
            Generations = carPostService.RetrieveGenerationsForModel(value);
        }

        partial void OnSelectedModelIndexChanged(int value)
        {
            Generations = carPostService.RetrieveGenerationsForModel(models[value]);
        }

        partial void OnSelectedGenerationChanged(string value)
        {
            post.Car.Generation = value;
        }

        partial void OnSelectedFuelTypeChanged(string value)
        {
            post.Car.FuelType = value;
        }

        partial void OnYearChanged(int? value)
        {
            post.Car.Year = (int)value;
        }

        partial void OnMileageChanged(int? value)
        {
            post.Car.Mileage= (int)value;
        }

        partial void OnEngineCapacityChanged(string value)
        {
            post.Car.EngineCapacity= value + " cmc";
        }

        partial void OnEnginePowerChanged(int? value)
        {
            post.Car.Horsepower = (int)value;
        }

        partial void OnTorqueChanged(int? value)
        {
            post.Car.Torque= (int)value;
        }

        partial void OnSelectedTransmissionTypeChanged(string value)
        {
            post.Car.Transmission = value;
        }

        partial void OnSelectedGearboxConfigurationChanged(string value)
        {
            post.Car.Gears = value;
        }

        partial void OnPostPicturesChanged(ObservableCollection<PostPicture> value)
        {
            carouselVisible = true;
        }
    }
}
