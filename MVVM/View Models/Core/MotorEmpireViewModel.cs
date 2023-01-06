using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVM.Services.Car_Filter_Services;
using MVVM.View.Post_Feed;
using MVVM.View.Post_Upload;
using System.Diagnostics;
using Tools.Utility.CarFilterAndValidator;
using Tools.Utility.Messages;
using BaseViewModel = MVVM.View_Models.Base.BaseViewModel;
using CarFilter = MVVM.Models.Vehicle_Models.Car.Car_Filter_Model.CarFilter;
using UserAccount = MVVM.Models.User_Account_Model.UserAccount;

namespace MVVM.View_Models.Core;

[QueryProperty (nameof(MVVM.Models.User_Account_Model.UserAccount), nameof(MVVM.Models.User_Account_Model.UserAccount))]
[QueryProperty (nameof(CarFilter), nameof(CarFilter))]
[QueryProperty (nameof(SearchQueryText), nameof(SearchQueryText))]
[QueryProperty (nameof(Name), nameof(Name))]
[QueryProperty (nameof(UpdateNeeded), nameof(UpdateNeeded))]
public partial class MotorEmpireViewModel : BaseViewModel
{
    [ObservableProperty] [NotifyPropertyChangedFor(nameof(GreetingMessage))]
    private string name;

    public string GreetingMessage => $"Hello, {userAccount.Name}";


    [ObservableProperty]
    //[NotifyPropertyChangedFor (nameof(GreetingMessage))]
    private CarFilter filter;

    [ObservableProperty]
    private bool updateNeeded;

    [ObservableProperty] private UserAccount userAccount;

    [ObservableProperty] private string loggedInUser;

    private readonly CarFilterService carFilterService;

    [ObservableProperty] private string searchQueryText;

    private bool filtersWereApplied;

    [ObservableProperty] private List<string> carBodyType;

    [ObservableProperty] private List<string> manufacturers;

    [ObservableProperty] private List<string> models;

    [ObservableProperty] private string selectedCarBodyType;

    [ObservableProperty] private string selectedManufacturer;

    [ObservableProperty] private int selectedManufacturerIndex;

    [ObservableProperty] private bool modelHasGenerations;

    [ObservableProperty] private string selectedModel;

    [ObservableProperty] private int selectedModelIndex;

    [ObservableProperty] private List<string> generations;

    [ObservableProperty] private string selectedGeneration;

    [ObservableProperty] private List<string> lowerPriceBound;

    private List<int> lowerPrice;

    [ObservableProperty] private List<string> upperPriceBound;

    private List<int> upperPrice;

    [ObservableProperty] private string selectedLowerPriceBound;

    [ObservableProperty] private int selectedLowerPriceIndex;

    private int lowerBound = 1000;

    [ObservableProperty] private string selectedUpperPriceBound;

    [ObservableProperty] private int selectedUpperPriceIndex;

    private int upperBound = 100000;

    [ObservableProperty] private List<int> lowerYear;

    [ObservableProperty] private int selectedLowerYear = 1950;

    [ObservableProperty] private List<int> upperYear;

    [ObservableProperty] private int selectedUpperYear = 2023;

    [ObservableProperty] private List<string> fuelTypes;

    [ObservableProperty] private string selectedFuelType;

    private List<int> minMileage;

    [ObservableProperty] private List<string> minMileageBounds;

    [ObservableProperty] private string selectedMinMileage;

    private List<int> maxMileage;

    [ObservableProperty] private List<string> maxMileageBounds;

    [ObservableProperty] private string selectedMaxMileage;

    [ObservableProperty] private bool searchBarEnabled;

    private int lowerMileage = 500;
    private int upperMileage = 300000;

    public MotorEmpireViewModel()
    {
    }

    public MotorEmpireViewModel(CarFilterService carFilterService, UserAccount userAccount, CarFilter carFilter)
    {
        //filter = new();
        this.carFilterService = carFilterService;
        filter = carFilter;
        InitializeProps();
        this.userAccount = userAccount;
        LoggedInUser = userAccount.Uuid;
    }


    private void InitializeProps()
    {
        SearchBarEnabled = true;
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
        LowerYear = CarFilterFormatter.InitializeYears(2022, 1950);
        LowerYear = LowerYear.OrderBy(x => x).ToList();
        UpperYear = CarFilterFormatter.InitializeYears(2023, 1971);
        fuelTypes = new()
            { "Gasoline", "Gasoline + CNG", "Gasoline + LPG", "Diesel", "Electric", "Ethanol", "Hybrid", "Hydrogen" };
        InitializeMileageBounds();
    }

    private void InitializePriceBounds(Func<int, string> formatter)
    {
        lowerPrice = new()
        {
            1000, 1500, 2000, 2500, 3000, 3500, 4000, 4500, 5000, 5500, 6000, 6500, 7000, 7500, 8000, 8500, 9000, 9500,
            10000, 15000, 20000, 25000, 30000, 35000, 40000, 45000, 50000, 55000, 60000, 65000, 70000, 75000, 80000,
            85000, 90000, 95000, 100000, 150000, 200000, 250000, 300000,350000
        };
        upperPrice = new()
        {
            1500, 2000, 2500, 3000, 3500, 4000, 4500, 5000, 5500, 6000, 6500, 7000, 7500, 8000, 8500, 9000, 9500, 10000,
            15000, 20000, 25000, 30000, 35000, 40000, 45000, 50000, 55000, 60000, 65000, 70000, 75000, 80000, 85000,
            90000, 95000, 100000, 150000, 200000, 250000, 300000,350000,400000
        };
        upperPrice = upperPrice.OrderByDescending(i => i).ToList();
        lowerPriceBound = new();
        upperPriceBound = new();
        lowerPrice.ForEach(price => lowerPriceBound.Add(formatter(price)));
        upperPrice.ForEach(price => upperPriceBound.Add(formatter(price)));
    }

    private void InitializeMileageBounds()
    {
        minMileage = new()
        {
            500, 1000, 1500, 2000, 2500, 3000, 3500, 4000, 4500, 5000, 5500, 6000, 6500, 7000, 7500, 8000, 8500, 9000,
            9500, 10000, 15000, 20000, 25000, 30000, 35000, 40000, 45000, 50000, 55000, 60000, 65000, 70000, 75000,
            80000, 85000, 90000, 95000, 100000, 150000, 200000, 250000
        };
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
        manufacturers = carFilterService.GetManufacturers();
    }

    public void RetrieveAllModelsFromManufacturer()
    {
        if (selectedManufacturer is not null)
        {
            models = carFilterService.GetAllModelsFromManufacturer(selectedManufacturer);
        }
    }

    private void ResetFilter()
    {
        Filter.ChassisType = null;
        Filter.Manufacturer = null;
        Filter.YearRange = null;
        Filter.PriceRange = null;
        Filter.MileageRange= null;
        Filter.FuelType = null;
        Filter.Generation = null;
        Filter.ModelName = null;
    }

    private void ResetSelections()
    {
        ResetFilter();
        SelectedCarBodyType = null;
        SelectedManufacturer = null;
        SelectedModel = null;
        SelectedLowerPriceBound = null;
        SelectedUpperPriceBound = null;
        SelectedFuelType = null;
        SelectedLowerYear = 1950;
        SelectedUpperYear = 2023;
        SelectedGeneration = null;
        ModelHasGenerations = false;
        SelectedMinMileage = null;
        SelectedMaxMileage = null;

        UpdateNeeded = false;

    }

    partial void OnSelectedCarBodyTypeChanged(string value)
    {
        filtersWereApplied = true;
    }

    partial void OnSelectedManufacturerChanged(string value)
    {
        Models = carFilterService.GetAllModelsFromManufacturer(selectedManufacturer);
        filtersWereApplied = true;
    }

    partial void OnSelectedManufacturerIndexChanged(int value)
    {
        try
        {
            Models = carFilterService.GetAllModelsFromManufacturer(manufacturers[value]);
            filtersWereApplied = true;
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Debugger.Log(1, "Exception Thrown", $"Argument Out Of Range\nDetails:{ex.Message}");
            filtersWereApplied = false;
        }
    }

    partial void OnSelectedModelChanged(string value)
    {
        Generations = carFilterService.GetGenerationBasedOnModel(value);
        ModelHasGenerations = Generations.Count != 0;
        filtersWereApplied = true;
    }

    partial void OnSelectedModelIndexChanged(int value)
    {
        try
        {
            Generations = carFilterService.GetGenerationBasedOnModel(models[value]);
            ModelHasGenerations = Generations.Count != 0;
            filtersWereApplied = true;
        }
        catch (ArgumentOutOfRangeException ex) 
        {
            Debugger.Log(1, "Exception Thrown", $"Argument Out Of Range\nDetails:{ex.Message}");
            filtersWereApplied = false;
        }
    }

    partial void OnSelectedLowerPriceIndexChanged(int value)
    {
        try
        {
            lowerBound = lowerPrice[value];
            CarFilterFormatter.CompareValuesAndGenerateErrorsIfExisting(lowerBound, upperBound, UpperLowerFilter.Price);
            filtersWereApplied = true;
        }
        catch(ArgumentOutOfRangeException ex)
        {
            Debugger.Log(1, "Exception Thrown", $"Argument Out Of Range\nDetails:{ex.Message}");
            filtersWereApplied = false;
        }
    }

    partial void OnSelectedUpperPriceIndexChanged(int value)
    {
        try
        {
            upperBound = upperPrice[value];
            CarFilterFormatter.CompareValuesAndGenerateErrorsIfExisting(lowerBound, upperBound, UpperLowerFilter.Price);
            filtersWereApplied = true;
        }
        catch(ArgumentOutOfRangeException ex)
        {
            Debugger.Log(1, "Exception Thrown", $"Argument Out Of Range\nDetails:{ex.Message}");
            filtersWereApplied = false;
        }
    }


    /*        partial void OnSelectedUpperPriceBoundChanged(string value)
            {
                upperBound = upperPrice[UpperPriceBound.IndexOf(value)];

            }*/


    partial void OnSelectedLowerYearChanged(int value)
    {
        CarFilterFormatter.CompareValuesAndGenerateErrorsIfExisting(SelectedLowerYear, SelectedUpperYear,
            UpperLowerFilter.Year);
        filtersWereApplied = true;
        /*          SelectedLowerYear = 2021;
                    SelectedUpperYear = 2022;*/
    }

    partial void OnSelectedUpperYearChanged(int value)
    {
        CarFilterFormatter.CompareValuesAndGenerateErrorsIfExisting(SelectedLowerYear, SelectedUpperYear,
            UpperLowerFilter.Year);
        /*            SelectedLowerYear = 2021;
                    SelectedUpperYear = 2022;*/
        filtersWereApplied = true;
    }

    partial void OnSelectedMinMileageChanged(string value)
    {
        try
        {
            lowerMileage = minMileage[MinMileageBounds.IndexOf(value)];
            CarFilterFormatter.CompareValuesAndGenerateErrorsIfExisting(lowerMileage, upperMileage,
                UpperLowerFilter.Mileage);
            filtersWereApplied = true;

        }
        catch (ArgumentOutOfRangeException ex)
        {
            Debugger.Log(1, "Exception Thrown", $"Argument Out Of Range\nDetails:{ex.Message}");
            filtersWereApplied = false;
        }
    }

    partial void OnSelectedMaxMileageChanged(string value)
    {
        try
        {
            upperMileage = maxMileage[MaxMileageBounds.IndexOf(value)];
            CarFilterFormatter.CompareValuesAndGenerateErrorsIfExisting(lowerMileage, upperMileage,
                UpperLowerFilter.Mileage);
            filtersWereApplied = true;
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Debugger.Log(1, "Exception Thrown", $"Argument Out Of Range\nDetails:{ex.Message}");
            filtersWereApplied = false;
        }
    }

    private void ApplyFilters()
    {
        if (filtersWereApplied)
        {
            //Filter = new();
            Filter.ChassisType = selectedCarBodyType;
            Filter.Manufacturer = SelectedManufacturer;
            Filter.ModelName = SelectedModel;
            Filter.Generation = SelectedGeneration;
            Filter.FuelType = SelectedFuelType;
            Filter.PriceRange = new(lowerBound, upperBound);

            if (!CarFilterValidator.IsRangeValid(Filter.PriceRange))
            {
                CarFilterValidator.InvalidRangeSpecifier(UpperLowerFilter.Price, "1 000€ - 100 000 €");
                Filter.PriceRange = new(1000, 100000);
            }

            Filter.YearRange = new(selectedLowerYear, selectedUpperYear);
            if (!CarFilterValidator.IsRangeValid(Filter.YearRange))
            {
                CarFilterValidator.InvalidRangeSpecifier(UpperLowerFilter.Year, "1999 - 2022");
                Filter.YearRange = new(1999, 2022);
            }

            Filter.MileageRange = new(lowerMileage, upperMileage);
            if (!CarFilterValidator.IsRangeValid(Filter.MileageRange))
            {
                CarFilterValidator.InvalidRangeSpecifier(UpperLowerFilter.Mileage, "500 km - 300 000 km");
                Filter.MileageRange = new(500, 300000);
            }
        }
    }

    [RelayCommand]
    private async void NavigateToFeed()
    {
        ApplyFilters();
        UpdateNeeded = true;
        await Shell.Current.GoToAsync($"{nameof(PostFeed)}?SearchQueryText={SearchQueryText}", true,
            new Dictionary<string, object> { ["CarFilter"] = filter,
                ["UpdateNeeded"]=UpdateNeeded,
            });

        SearchQueryText = "";
        ResetSelections();
    }


    [RelayCommand]
    private async void NavigateToPostUpload()
    {
        await Shell.Current.GoToAsync($"{nameof(UploadPost)}?Name", true,
            new Dictionary<string, object> { ["UserAccount"] = userAccount });
    }

    [RelayCommand]
    private async void OnSearchButtonPressed()
    {
        if (!string.IsNullOrEmpty(SearchQueryText))
        {
            ApplyFilters();
            UpdateNeeded = true;
            await Shell.Current.GoToAsync($"{nameof(PostFeed)}?SearchQueryText={SearchQueryText}", true,
                new Dictionary<string, object>
                {
                    ["CarFilter"] = filter,
                    ["UpdateNeeded"] = UpdateNeeded,
                });

            SearchQueryText = "";
            ResetSelections();
            UpdateNeeded = false;
            return;
        }

        CrossPlatformMessageRenderer.RenderMessages("The search bar cannot be empty!", "Retry", 4);
    }

    [RelayCommand]
    private async void ViewAllVehicles()
    {
        UpdateNeeded = true;
        await Shell.Current.GoToAsync(nameof(PostFeed), true, new Dictionary<string, object>
        {
            ["UpdateNeeded"]=UpdateNeeded,
        });

        UpdateNeeded = false;
    }

}