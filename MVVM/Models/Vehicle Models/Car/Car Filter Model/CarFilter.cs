
using CommunityToolkit.Mvvm.ComponentModel;
using MotorEmpireAutohaus.MVVM.View_Models.Base;

namespace MotorEmpireAutohaus.MVVM.Models.Vehicle_Models.Car.Car_Filter_Model;

public partial class CarFilter : BaseViewModel
{
    [ObservableProperty]
    private string manufacturer;

    [ObservableProperty]
    private string modelName;

    [ObservableProperty]
    private string generation;

    [ObservableProperty]
    private Tuple<int, int> priceRange;

    [ObservableProperty]
    private Tuple<int, int> yearRange;

    [ObservableProperty]
    private Tuple<int, int> mileageRange;

    [ObservableProperty]
    private string fuelType;

    public CarFilter() { }

    public bool IsEmpty()
    {
        return string.IsNullOrEmpty(manufacturer) && string.IsNullOrEmpty(modelName) && priceRange != null
            && mileageRange != null && string.IsNullOrEmpty(fuelType);
    }
}