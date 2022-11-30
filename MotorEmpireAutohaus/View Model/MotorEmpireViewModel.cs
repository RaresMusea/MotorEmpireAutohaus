using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MotorEmpireAutohaus.Services.Feed;
using MotorEmpireAutohaus.View_Model.Base;
using System.Collections.ObjectModel;

namespace MotorEmpireAutohaus.View_Model
{
    public partial class MotorEmpireViewModel:BaseViewModel
    {
        private readonly FeedService feedService;

        [ObservableProperty]
        private List<string> carBodyType;

        [ObservableProperty]
        private List<string> manufacturers;

        [ObservableProperty]
        private List<string> models;

        [ObservableProperty]
        private string selectedCarBodyType;

        [ObservableProperty]
        private string selectedManufacturer="Abarth";

        [ObservableProperty]
        private int selectedManufacturerIndex;

        [ObservableProperty]
        private bool userHasSelectedManufacturer=false;

        [ObservableProperty]
        private string selectedModel;


        public MotorEmpireViewModel(FeedService feedService)
        {
            this.feedService = feedService;
            RetrieveCarBodyTypes();
            RetrieveAllManufacturers();
            models = feedService.GetAllModelsFromManufacturer(selectedManufacturer);
        }

        private void RetrieveCarBodyTypes()
        {
            carBodyType = feedService.RetrieveCarBodyTypes();
        }

        private void RetrieveAllManufacturers()
        {
            manufacturers=feedService.GetManufacturers();
        }

        public void RetrieveAllModelsFromManufacturer()
        {
            if(selectedManufacturer is not null) {
                userHasSelectedManufacturer = true;
                models = feedService.GetAllModelsFromManufacturer(selectedManufacturer);
            }
        }

        partial void OnSelectedManufacturerChanged(string value)
        {
            Models = feedService.GetAllModelsFromManufacturer(selectedManufacturer);
        }

        partial void OnSelectedManufacturerIndexChanged(int value)
        {
            Models = feedService.GetAllModelsFromManufacturer(manufacturers[value]);
        }



    }
}
