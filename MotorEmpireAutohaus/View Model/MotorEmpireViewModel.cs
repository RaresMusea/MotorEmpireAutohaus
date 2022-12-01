using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MotorEmpireAutohaus.Misc.Common;
using MotorEmpireAutohaus.Services.Feed;
using MotorEmpireAutohaus.View_Model.Base;
using System.Collections.ObjectModel;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace MotorEmpireAutohaus.View_Model
{
    public partial class MotorEmpireViewModel:BaseViewModel
    {
        private readonly CarFilterService carFilterService;

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
        private bool modelHasGenerations=false;

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
        private string selectedLowerPriceBound= "1 000 €";

        [ObservableProperty]
        private int selectedLowerPriceIndex;

        private int lowerBound;

        [ObservableProperty]
        private string selectedUpperPriceBound= "1 500 €";

        private int upperBound;




        public MotorEmpireViewModel(CarFilterService carFilterService)
        {
            this.carFilterService = carFilterService;
            RetrieveCarBodyTypes();
            RetrieveAllManufacturers();
            models = carFilterService.GetAllModelsFromManufacturer(selectedManufacturer);
            generations = carFilterService.GetGenerationBasedOnModel(selectedModel);
            if (generations.Count != 0)
            {
                ModelHasGenerations = true;
            }
            InitializePriceBounds(FormatPrice);
        }

        private void InitializePriceBounds(Func<int,string>formatter)
        {
            lowerPrice = new(){1000,1500,2000,2500,3000,3500,4000,4500,5000,5500,6000,6500,7000,7500,8000,8500,9000,9500,10000,15000,20000,25000,30000,35000,40000,45000,50000,55000,60000,65000,70000,75000,80000,85000,90000,95000};
            upperPrice = new() { 1500, 2000, 2500, 3000, 3500, 4000, 4500, 5000, 5500, 6000, 6500, 7000, 7500, 8000, 8500, 9000, 9500, 10000, 15000, 20000, 25000, 30000, 35000, 40000, 45000, 50000, 55000, 60000, 65000, 70000, 75000, 80000, 85000, 90000, 95000 };
            lowerPriceBound = new();
            upperPriceBound = new();
            lowerPrice.ForEach(price=>lowerPriceBound.Add(formatter(price)));
            upperPrice.ForEach(price => upperPriceBound.Add(formatter(price)));
        }

        Func<int, string> FormatPrice = (int price) =>
        {
            string currency = "€";
            string casted = price.ToString();

            StringBuilder formatted = new StringBuilder();
            if (casted.Length == 4)
            {
                formatted.Append(casted[0]);
                formatted.Append(' ');
                formatted.Append(casted.Substring(1));
            }
            else
            {
                formatted.Append(casted[0]);
                formatted.Append(casted[1]);
                formatted.Append(' ');
                formatted.Append(casted.Substring(2));
            }
            formatted.Append($" {currency}");
            return formatted.ToString();
        };

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

        Func<int, List<int>, List<int>> BuildUpperBoundPriceList = (index, list) =>
        {
            List<int> res = new();
            for (int i = index; i < list.Count; i++)
            {
                res.Add(list[i] < 10000 ? list[i] + 500 : list[i] + 5000);
            }
            return res;
        };

        partial void OnSelectedManufacturerChanged(string value)
        {
            Models = carFilterService.GetAllModelsFromManufacturer(selectedManufacturer);
        }

        partial void OnSelectedManufacturerIndexChanged(int value)
        {
            Models = carFilterService.GetAllModelsFromManufacturer(manufacturers[value]);
        }

        partial void OnSelectedModelChanged(string value)
        {
            Generations = carFilterService.GetGenerationBasedOnModel(value);
            ModelHasGenerations = Generations.Count != 0 ? true : false;
        }

        partial void OnSelectedModelIndexChanged(int value)
        {
            Generations = carFilterService.GetGenerationBasedOnModel(models[value]);
            ModelHasGenerations = Generations.Count != 0 ? true : false;
        }


        partial void OnSelectedLowerPriceBoundChanged(string value)
        {
            lowerBound = lowerPrice[LowerPriceBound.IndexOf(value)];
        }

        partial void OnSelectedUpperPriceBoundChanged(string value)
        {
            upperBound = upperPrice[UpperPriceBound.IndexOf(value)];
            if (lowerBound > upperBound && upperBound != 0)
            {
                CrossPlatformMessageRenderer.RenderMessages("Cannot set a lower price bound that is greater than the upper price bound!", "Retry", 5);
                SelectedLowerPriceBound = "1 000 €";
                SelectedUpperPriceBound = "1 500 €";
            }
            if (lowerBound == upperBound && upperBound != 0)
            {
                CrossPlatformMessageRenderer.RenderMessages("The price bounds cannot be equal!", "Retry", 4);
                SelectedLowerPriceBound = "1 000 €";
                SelectedUpperPriceBound = "1 500 €";
            }
        }


    }
}
