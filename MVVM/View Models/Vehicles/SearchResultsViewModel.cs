using CommunityToolkit.Mvvm.ComponentModel;
using BaseViewModel = MVVM.View_Models.Base.BaseViewModel;
using CarFilter = MVVM.Models.Vehicle_Models.Car.Car_Filter_Model.CarFilter;

namespace MVVM.View_Models.Vehicles
{
    [QueryProperty(nameof(Models.Vehicle_Models.Car.Car_Filter_Model.CarFilter),
        nameof(Models.Vehicle_Models.Car.Car_Filter_Model.CarFilter))]
    public partial class SearchResultsViewModel : BaseViewModel
    {
        [ObservableProperty] private CarFilter carFilter;
    }
}