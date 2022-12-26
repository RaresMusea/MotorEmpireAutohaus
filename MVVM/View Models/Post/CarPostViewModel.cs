using CommunityToolkit.Mvvm.ComponentModel;
using MotorEmpireAutohaus.MVVM.Models.User_Account_Model;
using MotorEmpireAutohaus.MVVM.View_Models.Base;
using MotorEmpireAutohaus.Services.Feed;
using MVVM.Models.Post_Model;
using MVVM.Services.Car_Entity_Services;
using MVVM.Services.Car_Post_Services;


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

        public CarPostViewModel(CarService carService, CarPostService carPostService, CarPost post, UserAccount userAccount)
        {
            this.carService= carService;
            this.carPostService= carPostService;
            this.post = post;
            owner = userAccount;
            post.Owner= owner;
            post.Car = new Models.Vehicle_Models.Car.Car_Model.Car();
            InitializeCarPostUploadComponent();
        }

        private void InitializeCarPostUploadComponent() 
        {
            InitializePickerValues(GetAllVehicleTypes(), ref vehicleTypes);
            InitializePickerValues(GetAllChassis(), ref chassisTypes);
            InitializePickerValues(GetCarManufacturers(), ref manufacturers);
            InitializePickerValues(GetModelsFromManufacturer(), ref models);
            InitializePickerValues(GetModelGenerations(), ref generations);
            InitializePickerValues(GetAllFuelTypes(), ref fuelTypes);
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
            generations = carPostService.RetrieveGenerationsForModel(value);
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
    }
}
