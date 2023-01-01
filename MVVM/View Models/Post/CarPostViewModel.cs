using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVM.Exceptions;
using MVVM.Models;
using MVVM.Models.Post_Model;
using MVVM.Services.Car_Entity_Services;
using MVVM.Services.Car_Post_Services;
using MVVM.Validations.Car_Post_Validation;
using System.Collections.ObjectModel;
using Storage.Firebase_Storage;
using Tools.Utility.Messages;
using BaseViewModel = MVVM.View_Models.Base.BaseViewModel;
using UserAccount = MVVM.Models.User_Account_Model.UserAccount;

namespace MVVM.View_Models.Post
{
    [QueryProperty(nameof(UserAccount), nameof(UserAccount))]
    public partial class CarPostViewModel : BaseViewModel
    {
        private readonly CarService carService;

        private readonly CarPostService carPostService;

        private readonly UserAccount owner;

        [ObservableProperty] CarPost post;

        [ObservableProperty] private List<string> vehicleTypes;

        [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsVehicleTypeSelected))]
        private string selectedVehicleType;

        public bool IsVehicleTypeSelected => selectedVehicleType != null;


        [ObservableProperty] private List<string> chassisTypes;

        [ObservableProperty] private string selectedChassisType;


        [ObservableProperty] private List<string> manufacturers;

        [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsManufacturerSelected))]
        private string selectedManufacturer;

        [ObservableProperty] private int selectedManufacturerIndex = -1;

        public bool IsManufacturerSelected => selectedManufacturer != null;

        [ObservableProperty] private List<string> models;

        [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsModelSelected))]
        private string selectedModel;

        [ObservableProperty] private int selectedModelIndex = -1;

        public bool IsModelSelected => selectedModel != null;

        [ObservableProperty] [NotifyPropertyChangedFor(nameof(ModelHasGenerations))]
        private List<string> generations;

        [ObservableProperty] private string selectedGeneration;

        public bool ModelHasGenerations => selectedModel != null && generations.Count != 0;

        [ObservableProperty] private int? year;

        [ObservableProperty] private List<string> fuelTypes;

        [ObservableProperty] private string selectedFuelType;

        [ObservableProperty] private int? mileage;

        [ObservableProperty] private string engineCapacity;

        [ObservableProperty] private int? enginePower;

        [ObservableProperty] private int? torque;

        [ObservableProperty] private List<string> transmissionTypes;

        [ObservableProperty] private string selectedTransmissionType;

        [ObservableProperty] private List<string> gearboxConfigurations;

        [ObservableProperty] private string selectedGearboxConfiguration;

        [ObservableProperty] private bool carDetailsFormVisible = true;

        [ObservableProperty] private bool pictureUploadVisible;

        [ObservableProperty] private bool descriptionFormVisible;

        [ObservableProperty] private bool equipmentFormVisible;

        [ObservableProperty] private ObservableCollection<PostPicture> postPicturesObservable = new();

        [ObservableProperty] private bool postPicturesCollectionNotEmpty;

        [ObservableProperty] private bool carouselVisible;

        private string firebaseUrlToFile;

        [ObservableProperty] private string vehicleEquipmentRaw;

        public CarPostViewModel(CarService carService, CarPostService carPostService, CarPost post,
            UserAccount userAccount)
        {
            this.carService = carService;
            this.carPostService = carPostService;
            this.post = post;
            owner = userAccount;
            post.Owner = owner;
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

        private static void InitializePickerValues(List<string> providedList, ref List<string> pickerBackend)
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

        private async Task<List<FileResult>> PickCarPostPictures()
        {
            List<FileResult> files;
            try
            {
                IEnumerable<FileResult> photos = await FilePicker.PickMultipleAsync();
                files = photos.ToList();
            }
            catch (Exception ex)
            {
                CrossPlatformMessageRenderer.RenderMessages(
                    $"An error occurred while attempting to access the files you specified. Please try again.\nMore details:\n${ex.Message}",
                    "Retry", 4);
                return null;
            }

            return files;
        }

        [RelayCommand]
        private void SwitchUploadFormWithPictureUpload()
        {
            VehiclePostUploadValidationResult result = VehiclePostValidator.AreCarDetailsValid(post.Car);
            if (result.ValidationPassed)
            {
                CarDetailsFormVisible = false;
                PictureUploadVisible = true;
            }
            else
            {
                CrossPlatformMessageRenderer.RenderMessages(result.Remark, "Retry", 10);
            }
        }

        [RelayCommand]
        private async void UploadCarPostPictures()
        {
            List<FileResult> photos = await PickCarPostPictures();
            string path = $"Images/VehiclePosts/{selectedVehicleType}/{post.Car.Uuid}";

            foreach (FileResult photo in photos)
            {
                firebaseUrlToFile = await FirebaseCloudStorage.AddFileToFirebaseCloudStorageAsync(photo, path);
                PostPicturesObservable.Add(new PostPicture(firebaseUrlToFile));
                PostPicturesCollectionNotEmpty = true;
                CarouselVisible = true;
            }

            Post.PostPictures = PostPicturesObservable.ToList();
        }

        [RelayCommand]
        private void DeletePicturesAndGoBack()
        {
            try
            {
                //await FirebaseCloudStorage.DeleteFirebaseDataFromAsync($"{post.Car.UUID}"); 
            }
            catch (Exception ex)
            {
                CrossPlatformMessageRenderer.RenderMessages(ex.Message, "Retry", 4);
            }
            finally
            {
                PostPicturesObservable.Clear();
                PostPicturesCollectionNotEmpty = false;
                CarouselVisible = false;
                PictureUploadVisible = false;
                CarDetailsFormVisible = true;
            }
        }

        [RelayCommand]
        private void NavigateToDescription()
        {
            DescriptionFormVisible = true;
            PictureUploadVisible = false;
            CarDetailsFormVisible = false;
        }

        [RelayCommand]
        private void GoToVehicleEquipment()
        {
            EquipmentFormVisible = true;
            DescriptionFormVisible = false;
            CarDetailsFormVisible = false;
            PictureUploadVisible = false;
        }

        [RelayCommand]
        private void BackToPictureUpload()
        {
            DescriptionFormVisible = false;
            PictureUploadVisible = true;
        }

        [RelayCommand]
        private void BackToPostDescription()
        {
            DescriptionFormVisible = true;
            EquipmentFormVisible = false;
        }

        [RelayCommand]
        private async void UploadCarPost()
        {
            if (carService.Save(post.Car) is not null)
            {
                try
                {
                    carPostService.UploadCarPost(Post, carService.RetrieveCarIdentifierByUuid(post.Car.Uuid));
                    CrossPlatformMessageRenderer.RenderMessages("Successfully added a new car !", "Good", 3);
                }
                catch (UploadFailedException uploadFailedExc)
                {
                    CrossPlatformMessageRenderer.RenderMessages(uploadFailedExc.Message, "OK", 4);
                }
                finally
                {
                    CleanUp();
                    await Shell.Current.GoToAsync("//MotorEmpire", true);
                }
            }
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
            try
            {
                Models = carPostService.RetrieveAllModelsFromManufacturer(manufacturers[value]);
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }
        }

        partial void OnSelectedModelChanged(string value)
        {
            try
            {
                post.Car.Model = value;
                Generations = carPostService.RetrieveGenerationsForModel(value);
            }
            catch (ArgumentOutOfRangeException)
            {
                Generations = null!;
            }
        }

        partial void OnSelectedModelIndexChanged(int value)
        {
            try
            {
                Generations = carPostService.RetrieveGenerationsForModel(models[value]);
            }
            catch (ArgumentOutOfRangeException)
            {
                Generations = null!;
            }
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
            try
            {
                if (value != null) post.Car.Year = (int)value;
            }
            catch (InvalidOperationException)
            {
                return;
            }
        }

        partial void OnMileageChanged(int? value)
        {
            try
            {
                if (value != null) post.Car.Mileage = (int)value;
            }
            catch (InvalidOperationException)
            {
                return;
            }
        }

        partial void OnEngineCapacityChanged(string value)
        {
            post.Car.EngineCapacity = value + " cmc";
        }

        partial void OnEnginePowerChanged(int? value)
        {
            try
            {
                if (value != null) post.Car.Horsepower = (int)value;
            }
            catch (InvalidOperationException)
            {
                return;
            }
        }

        partial void OnTorqueChanged(int? value)
        {
            try
            {
                if (value != null) post.Car.Torque = (int)value;
            }
            catch (InvalidOperationException)
            {
                return;
            }
        }

        partial void OnSelectedTransmissionTypeChanged(string value)
        {
            post.Car.Transmission = value;
        }

        partial void OnSelectedGearboxConfigurationChanged(string value)
        {
            post.Car.Gears = value;
        }

        partial void OnPostPicturesObservableChanged(ObservableCollection<PostPicture> value)
        {
            carouselVisible = true;
        }

        private void CleanUp()
        {
            Post.Car = new();
            EquipmentFormVisible = false;
            DescriptionFormVisible = false;
            CarDetailsFormVisible = true;
            PictureUploadVisible = false;
            SelectedChassisType = null!;
            SelectedFuelType = null!;
            SelectedGearboxConfiguration = null!;
            SelectedGeneration = null!;
            SelectedModel = null!;
            SelectedModelIndex = -1;
            SelectedManufacturer = null!;
            SelectedManufacturerIndex = -1;
            SelectedTransmissionType = null!;
            SelectedVehicleType = null!;
            Year = null;
            Mileage = null;
            EngineCapacity = null!;
            EnginePower = null;
            Torque = null;
            PostPicturesObservable = new();
            CarouselVisible = false;

            Post = new()
            {
                PostPictures = new(),
                Car = new(),
                Owner = owner,
            };

            Post.Uuid = Post.Car.Uuid;
        }
    }
}