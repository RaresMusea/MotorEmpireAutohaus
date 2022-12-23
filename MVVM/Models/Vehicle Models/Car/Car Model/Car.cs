using CommunityToolkit.Mvvm.ComponentModel;
using MVVM.Models.Vehicle_Models.Vehicle_Model;

namespace MVVM.Models.Vehicle_Models.Car.Car_Model
{
    public partial class Car: Vehicle
    {
        [ObservableProperty]
        private int mileage;

        [ObservableProperty]
        private string engineCapacity;

        [ObservableProperty]
        private int horsepower;

        [ObservableProperty]
        private int torque;

        [ObservableProperty]
        private string transmission;

        [ObservableProperty]
        private string gears;

        
        public Car(string vehicleType, string manufacturer, string model, string generation,
            int year, string fuelType, int price, int mileage, string engineCapacity, 
            int horsepower, int torque, string transmission, string gears):base(vehicleType,manufacturer,model,generation,
                year,fuelType)
        {
            Mileage = mileage;
            EngineCapacity = engineCapacity;
            Horsepower = horsepower;
            Torque = torque;
            Transmission = transmission;
            Gears = gears;
        }

        public override bool IsEmpty()
        {
            if (this is null)
            {
                return true;
            }

            return Manufacturer == "" || Model == "" || Year == 0 || FuelType == "" || Mileage==0 || EngineCapacity=="";
        }


    }
}
