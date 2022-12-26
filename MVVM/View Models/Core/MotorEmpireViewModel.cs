using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MotorEmpireAutohaus.Services.Feed;
using MotorEmpireAutohaus.Tools.Utility.CarFilterAndValidator;
using MotorEmpireAutohaus.MVVM.View_Models.Base;
using MotorEmpireAutohaus.MVVM.Models.Vehicle_Models.Car.Car_Filter_Model;
using MotorEmpireAutohaus.MVVM.View_Models.Account;
using MotorEmpireAutohaus.MVVM.Models.User_Account_Model;
using MVVM.View.Post_Upload;

namespace MotorEmpireAutohaus.MVVM.View_Models.Core;

[QueryProperty (nameof(UserAccount),nameof(UserAccount))]
[QueryProperty (nameof(CarFilter),nameof(CarFilter))]
[QueryProperty (nameof(Name),nameof(Name))]
public partial class MotorEmpireViewModel : BaseViewModel
{
    [ObservableProperty]
    [NotifyPropertyChangedFor (nameof(GreetingMessage))]
    private string name;

    public string GreetingMessage => $"Hello, {userAccount.Name}"; 


    [ObservableProperty]
    [NotifyPropertyChangedFor (nameof(GreetingMessage))]
    private CarFilter carFilter;

    [ObservableProperty]
    private UserAccount userAccount;

    private readonly CarFilterService carFilterService;

    private bool filtersWereApplied;

    [ObservableProperty]
    private List<string> carBodyType;

    [ObservableProperty]
    private List<string> manufacturers;

    [ObservableProperty]
    private List<string> models;

    [ObservableProperty]
    private string selectedCarBodyType;

    [ObservableProperty]
    private string selectedManufacturer;

    [ObservableProperty]
    private int selectedManufacturerIndex;

    [ObservableProperty]
    private bool modelHasGenerations = false;

    [ObservableProperty]
    private string selectedModel;

    [ObservableProperty]
    private int selectedModelIndex;

    [ObservableProperty]
    private List<string> generations;

    [ObservableProperty]
    private string selectedGeneration;

    [ObservableProperty]
    private List<string> lowerPriceBound;

    private List<int> lowerPrice;

    [ObservableProperty]
    private List<string> upperPriceBound;

    private List<int> upperPrice;

    [ObservableProperty]
    private string selectedLowerPriceBound;

    [ObservableProperty]
    private int selectedLowerPriceIndex;

    private int lowerBound=1000;

    [ObservableProperty]
    private string selectedUpperPriceBound;

    [ObservableProperty]
    private int selectedUpperPriceIndex;

    private int upperBound=100000;

    [ObservableProperty]
    private List<int> lowerYear;

    [ObservableProperty]
    private int selectedLowerYear=2000;

    [ObservableProperty]
    private List<int> upperYear;

    [ObservableProperty]
    private int selectedUpperYear=2022;

    [ObservableProperty]
    private List<string> fuelTypes;

    [ObservableProperty]
    private string selectedFuelType;

    private List<int> minMileage;

    [ObservableProperty]
    private List<string> minMileageBounds;

    [ObservableProperty]
    private string selectedMinMileage;

    private List<int> maxMileage;

    [ObservableProperty]
    private List<string> maxMileageBounds;

    [ObservableProperty]
    private string selectedMaxMileage;

    private int lowerMileage=500;
    private int upperMileage=300000;

    public MotorEmpireViewModel() { }

    public MotorEmpireViewModel(CarFilterService carFilterService, UserAccount userAccount)
    {
        this.carFilterService = carFilterService;
        InitializeProps();
        this.userAccount = userAccount;
    }


    private void InitializeProps()
    {
        filtersWereApplied = false;
        RetrieveCarBodyTypes();
        RetrieveAllManufacturers();
        models = carFilterService.GetAllModelsFromManufacturer(selectedManufacturer);
        generations = carFilterService.GetGenerationBasedOnModel(selectedModel);
        if (generations.Count != 0)
        {
            ModelHasGenerations = true;
        }
        InitializePriceBounds(CarFilterFormatter.FormatPrice);
        LowerYear = CarFilterFormatter.InitializeYears(2021, 2000);
        LowerYear = LowerYear.OrderBy(x => x).ToList();
        UpperYear = CarFilterFormatter.InitializeYears(2022, 2001);
        fuelTypes = new() { "Gasoline", "Gasoline + CNG", "Gasoline + LPG", "Diesel", "Electric", "Ethanol", "Hybrid", "Hydrogen" };
        InitializeMileageBounds();
    }

    private void InitializePriceBounds(Func<int, string> formatter)
    {
        lowerPrice = new() { 1000, 1500, 2000, 2500, 3000, 3500, 4000, 4500, 5000, 5500, 6000, 6500, 7000, 7500, 8000, 8500, 9000, 9500, 10000, 15000, 20000, 25000, 30000, 35000, 40000, 45000, 50000, 55000, 60000, 65000, 70000, 75000, 80000, 85000, 90000, 95000 };
        upperPrice = new() { 1500, 2000, 2500, 3000, 3500, 4000, 4500, 5000, 5500, 6000, 6500, 7000, 7500, 8000, 8500, 9000, 9500, 10000, 15000, 20000, 25000, 30000, 35000, 40000, 45000, 50000, 55000, 60000, 65000, 70000, 75000, 80000, 85000, 90000, 95000,100000};
        upperPrice = upperPrice.OrderByDescending(i => i).ToList();
        lowerPriceBound = new();
        upperPriceBound = new();
        lowerPrice.ForEach(price => lowerPriceBound.Add(formatter(price)));
        upperPrice.ForEach(price => upperPriceBound.Add(formatter(price)));
    }

    private void InitializeMileageBounds() 
    {
        minMileage = new() { 500, 1000, 1500, 2000, 2500, 3000, 3500, 4000, 4500, 5000, 5500, 6000, 6500, 7000, 7500, 8000, 8500, 9000, 9500, 10000, 15000, 20000, 25000, 30000, 35000, 40000, 45000, 50000, 55000, 60000, 65000, 70000, 75000, 80000, 85000, 90000, 95000, 100000, 150000, 200000, 250000 };
        maxMileage = new(minMileage);
        maxMileage.Remove(500);
        maxMileage.Add(300000);
        maxMileage = maxMileage.OrderByDescending(mile => mile).ToList();
        minMileageBounds = new();
        maxMileageBounds = new();

        minMileageBounds = (from m in minMileage select CarFilterFormatter.FormatMileage(m)).ToList();
        maxMileageBounds = (from m in maxMileage select CarFilterFormatter.FormatMileage(m)).ToList();
    }


    private void RetrieveCarBodyTypes()
    {
        carBodyType = carFilterService.RetrieveCarBodyTypes();
    }

    private void RetrieveAllManufacturers()
    {
        manufacturers=carFilterService.GetManufacturers();
    }

    public void RetrieveAllModelsFromManufacturer()
    {
        if(selectedManufacturer is not null) {
            models = carFilterService.GetAllModelsFromManufacturer(selectedManufacturer);
        }
    }

    partial void OnSelectedManufacturerChanged(string value)
    {
        Models = carFilterService.GetAllModelsFromManufacturer(selectedManufacturer);
        filtersWereApplied = true;
    }

    partial void OnSelectedManufacturerIndexChanged(int value)
    {
        Models = carFilterService.GetAllModelsFromManufacturer(manufacturers[value]);
        filtersWereApplied = true;
    }

    partial void OnSelectedModelChanged(string value)
    {
        Generations = carFilterService.GetGenerationBasedOnModel(value);
        ModelHasGenerations = Generations.Count != 0;
        filtersWereApplied = true;

    }

    partial void OnSelectedModelIndexChanged(int value)
    {
        Generations = carFilterService.GetGenerationBasedOnModel(models[value]);
        ModelHasGenerations = Generations.Count != 0;
        filtersWereApplied = true;
    }

    partial void OnSelectedLowerPriceIndexChanged(int value)
    {
        lowerBound = lowerPrice[value];
        CarFilterFormatter.CompareValuesAndGenerateErrorsIfExisting(lowerBound, upperBound, UpperLowerFilter.Price);
        filtersWereApplied = true;
    }

    partial void OnSelectedUpperPriceIndexChanged(int value)
    {
        upperBound = upperPrice[value];
        CarFilterFormatter.CompareValuesAndGenerateErrorsIfExisting(lowerBound, upperBound, UpperLowerFilter.Price);
        filtersWereApplied = true;
    }


    /*        partial void OnSelectedUpperPriceBoundChanged(string value)
            {
                upperBound = upperPrice[UpperPriceBound.IndexOf(value)];

            }*/


    partial void OnSelectedLowerYearChanged(int value)
    {
        CarFilterFormatter.CompareValuesAndGenerateErrorsIfExisting(SelectedLowerYear, SelectedUpperYear, UpperLowerFilter.Year);
        filtersWereApplied = true;
        /*          SelectedLowerYear = 2021;
                    SelectedUpperYear = 2022;*/

    }

    partial void OnSelectedUpperYearChanged(int value)
    {
        CarFilterFormatter.CompareValuesAndGenerateErrorsIfExisting(SelectedLowerYear, SelectedUpperYear, UpperLowerFilter.Year);
        /*            SelectedLowerYear = 2021;
                    SelectedUpperYear = 2022;*/
        filtersWereApplied = true;
    }

    partial void OnSelectedMinMileageChanged(string value)
    {
        lowerMileage = minMileage[MinMileageBounds.IndexOf(value)];
        CarFilterFormatter.CompareValuesAndGenerateErrorsIfExisting(lowerMileage, upperMileage, UpperLowerFilter.Mileage);
        filtersWereApplied = true;
    }

    partial void OnSelectedMaxMileageChanged(string value)
    {
        upperMileage = maxMileage[MaxMileageBounds.IndexOf(value)];
        CarFilterFormatter.CompareValuesAndGenerateErrorsIfExisting(lowerMileage, upperMileage, UpperLowerFilter.Mileage);
        filtersWereApplied = true;
    }

    private CarFilter ApplyFilters()
    {
        if (filtersWereApplied)
        {
            CarFilter carFilter = new();
            carFilter.Manufacturer = SelectedManufacturer;
            carFilter.ModelName = SelectedModel;
            carFilter.Generation= SelectedGeneration;
            carFilter.FuelType = SelectedFuelType; 
            carFilter.PriceRange=new(lowerBound,upperBound);

            if (!CarFilterValidator.IsRangeValid(carFilter.PriceRange))
            {
                CarFilterValidator.InvalidRangeSpecifier(UpperLowerFilter.Price, "1 000€ - 100 000 €");
                carFilter.PriceRange = new(1000, 100000);
            }

            carFilter.YearRange=new(selectedLowerYear, selectedUpperYear);
            if (!CarFilterValidator.IsRangeValid(carFilter.YearRange))
            {
                CarFilterValidator.InvalidRangeSpecifier(UpperLowerFilter.Year, "1999 - 2022");
                carFilter.YearRange = new(1999, 2022);
            }

            carFilter.MileageRange = new(lowerMileage, upperMileage);
            if (!CarFilterValidator.IsRangeValid(carFilter.MileageRange))
            {
                CarFilterValidator.InvalidRangeSpecifier(UpperLowerFilter.Mileage, "500 km - 300 000 km");
                carFilter.MileageRange = new(500, 300000);
            }

            return carFilter;

        }
        return null;
    }

    [RelayCommand]
    public async void NavigateToFeed()
    {
        carFilter = ApplyFilters();
        await Shell.Current.GoToAsync(nameof(MotorEmpireAutohaus.Services.Feed), true, new Dictionary<string, object> { ["CarFilter"] = carFilter });
    }


    [RelayCommand]
    public async void NavigateToPostUpload()
    {
        await Shell.Current.GoToAsync($"{nameof(UploadPost)}?Name", true, new Dictionary<string, object> { ["UserAccount"] = userAccount });
    }

}

