using CommunityToolkit.Mvvm.ComponentModel;
using MVVM.Models.Base;

namespace MVVM.Models.Vehicle_Models.Vehicle_Model
{
    public abstract partial class Vehicle : Entity
    {
        [ObservableProperty]
        private string vehicleType;

        [ObservableProperty]
        private string manufacturer;

        [ObservableProperty]
        private string model;

        [ObservableProperty]
        private string generation;

        [ObservableProperty]
        private int year;

        [ObservableProperty]
        private string fuelType;


        protected Vehicle() { }

        protected Vehicle(string vehicleType, string manufacturer, string model, string generation, int year, string fuelType)
        {
            this.vehicleType = vehicleType;
            this.manufacturer = manufacturer;
            this.model = model;
            this.generation = generation;
            this.year = year;
            this.fuelType = fuelType;
        }

        protected Vehicle(string manufacturer, string model, string generation, int year, string fuelType)
        {
            this.manufacturer = manufacturer;
            this.model = model;
            this.generation = generation;
            this.year = year;
            this.fuelType = fuelType;
        }

        public abstract override bool IsEmpty();

    }
}
