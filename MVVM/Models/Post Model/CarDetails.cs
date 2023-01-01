using CommunityToolkit.Mvvm.ComponentModel;
using MVVM.View_Models.Base;


namespace MVVM.Models.Post_Model
{
    public partial class CarDetails:BaseViewModel
    {
        [ObservableProperty]
        private string manufacturerBinding;

        [ObservableProperty]
        private string modelBinding;

        [ObservableProperty]
        private string generationBinding;

        [ObservableProperty]
        private bool hasGeneration;

        [ObservableProperty]
        private string yearBinding;

        [ObservableProperty]
        private string fuelTypeBinding;

        [ObservableProperty]
        private string engineCapacityBinding;

        [ObservableProperty]
        private string horsepowerBinding;

        [ObservableProperty]
        private string mileageBinding;

        [ObservableProperty]
        private string priceBinding;

        [ObservableProperty]
        private bool hasTorque;

        [ObservableProperty]
        private string uploadInformation;

        [ObservableProperty]
        private string viewedBy;

    }
}
