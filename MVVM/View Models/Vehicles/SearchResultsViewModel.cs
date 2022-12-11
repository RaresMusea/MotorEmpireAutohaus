using CommunityToolkit.Mvvm.ComponentModel;
using MotorEmpireAutohaus.MVVM.Models.Vehicle_Models.Car.Car_Filter_Model;
using MotorEmpireAutohaus.MVVM.View_Models.Base;

namespace MotorEmpireAutohaus.View_Model.Vehicles
{
    [QueryProperty (nameof(CarFilter),nameof(CarFilter))]
    public partial class SearchResultsViewModel:BaseViewModel
    {
        [ObservableProperty]
        CarFilter carFilter;
    }
}
